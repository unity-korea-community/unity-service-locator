using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UNKO.ServiceLocator
{
    public enum ServiceScope
    {
        Global,
        Scene,
        GameObject,
        None,
    }

    public enum RegistOption
    {
        /// <summary>
        /// 구체적인 클래스 한개만 등록
        /// </summary>
        ConcreteTypeOnly,

        /// <summary>
        /// 구체적인 클래스와 모든 인터페이스들을 등록
        /// </summary>
        ConcreteTypeAndInterfaces,
        InterfacesOnly,
    }

    public enum UnregistOption
    {
        OnDestroy,
        OnDisable,
    }

    /// <summary>
    /// service locator에 서비스를 등록하는 컴포넌트
    /// </summary>
    [DefaultExecutionOrder(Constants.EXECUTION_ORDER_REGISTER)]
    public class ServiceLocatorRegister : MonoBehaviour
    {
        [Serializable]
        public struct RegistInfo
        {
            [SerializeField]
            ServiceScope _registWhere; public ServiceScope RegistWhere => _registWhere;
            [SerializeField]
            RegistOption _registOption; public RegistOption RegistOption => _registOption;
            [SerializeField]
            UnregistOption _unregistOption; public UnregistOption UnregistOption => _unregistOption;
            [SerializeField]
            bool _callDisposeWhenUnRegist; public bool CallDisposeWhenUnRegist => _callDisposeWhenUnRegist;
            [SerializeField]
            Component[] _registServices; public Component[] RegistServices => _registServices;

            public RegistInfo(ServiceScope registWhere, RegistOption registOption, UnregistOption unregistOption, Component service, bool callDisposeWhenUnRegist = false)
            {
                _registWhere = registWhere;
                _registOption = registOption;
                _unregistOption = unregistOption;
                _callDisposeWhenUnRegist = callDisposeWhenUnRegist;
                _registServices = new Component[] { service };
            }
        }

        [Serializable]
        public struct RegistSOWrap
        {
            [SerializeField]
            ServiceScope _registWhere; public ServiceScope RegistWhere => _registWhere;
            [SerializeField]
            List<ServiceLocatorRegistSOBase> _registSOs; public List<ServiceLocatorRegistSOBase> RegistSOs => _registSOs;
        }

        public struct RegistedInfo
        {
            public ServiceScope RegistWhere => _registInfo.RegistWhere;
            public RegistOption RegistOption => _registInfo.RegistOption;
            public UnregistOption UnregistOption => _registInfo.UnregistOption;
            public bool CallDisposeWhenUnRegist => _registInfo.CallDisposeWhenUnRegist;

            RegistInfo _registInfo;
            Type[] _bindTypes; public Type[] BindTypes => _bindTypes;

            public RegistedInfo(RegistInfo registInfo, Type[] bindTypes)
            {
                _registInfo = registInfo;
                _bindTypes = bindTypes;
            }
        }

        [SerializeField]
        ServiceScope _registWhere = ServiceScope.Global;
        [SerializeField]
        RegistOption _registOption = RegistOption.ConcreteTypeOnly;
        [SerializeField]
        UnregistOption _unregistOption = UnregistOption.OnDestroy;
        [SerializeField]
        bool _callDisposeWhenUnRegist;
        [SerializeField]
        Component _registService;

        [SerializeField]
        List<RegistInfo> _whenMoreRequireRegists = new List<RegistInfo>();

        [SerializeField]
        List<RegistSOWrap> _registSOs = new List<RegistSOWrap>();

        List<RegistedInfo> _registedInfos = new List<RegistedInfo>();

        protected virtual void Awake()
        {
            if (ServiceLocator.Global == null)
            {
                Debug.LogError("ServiceLocatorScene.Awake: ServiceLocator.Global is null", this);
                return;
            }

            if (_registService != null)
            {
                RegistService(new RegistInfo(_registWhere, _registOption, _unregistOption, _registService, _callDisposeWhenUnRegist));
            }
            _whenMoreRequireRegists.ForEach(info => RegistService(info));
            _registSOs.ForEach(soWrap => soWrap.RegistSOs.ForEach(so => so.RegisterServices(GetServiceLocator(soWrap.RegistWhere))));
        }

        private void RegistService(RegistInfo registInfo)
        {
            var sl = GetServiceLocator(registInfo.RegistWhere);
            var registOption = registInfo.RegistOption;
            var where = registInfo.RegistWhere;
            foreach (var service in registInfo.RegistServices)
            {
                if (service == null)
                {
                    Debug.LogError($"ServiceLocatorRegister.RegistService: service is null. where={where}", this);
                    continue;
                }

                Type targetType = service.GetType();
                Type[] bindTypes = null;
                switch (registOption)
                {
                    case RegistOption.ConcreteTypeOnly:
                        bindTypes = new Type[] { targetType };
                        break;

                    case RegistOption.ConcreteTypeAndInterfaces:
                        bindTypes = targetType.GetInterfaces().Concat(new Type[] { targetType }).ToArray();
                        break;

                    case RegistOption.InterfacesOnly:
                        bindTypes = targetType.GetInterfaces();
                        break;
                }

                _registedInfos.Add(new RegistedInfo(registInfo, bindTypes));
                sl.RegisterService(service, bindTypes);
            }
        }

        private void OnDisable()
        {
            foreach (var info in _registedInfos)
            {
                if (info.UnregistOption == UnregistOption.OnDisable)
                {
                    var sl = GetServiceLocator(info.RegistWhere);
                    foreach (var bindType in info.BindTypes)
                    {
                        sl.UnregisterService(bindType, info.CallDisposeWhenUnRegist);
                    }
                }
            }
            _registedInfos.RemoveAll(info => info.UnregistOption == UnregistOption.OnDisable);
        }

        protected void OnDestroy()
        {
            foreach (var info in _registedInfos)
            {
                if (info.UnregistOption == UnregistOption.OnDestroy)
                {
                    var sl = GetServiceLocator(info.RegistWhere);
                    foreach (var bindType in info.BindTypes)
                    {
                        sl.UnregisterService(bindType, info.CallDisposeWhenUnRegist);
                    }
                }
            }
            _registedInfos.RemoveAll(info => info.UnregistOption == UnregistOption.OnDestroy);
        }

        IServiceLocator GetServiceLocator(ServiceScope registWhere)
        {
            switch (registWhere)
            {
                case ServiceScope.Global:
                    return ServiceLocator.Global;
                case ServiceScope.Scene:
                    return ServiceLocator.SceneOf(this);
                case ServiceScope.GameObject:
                    return ServiceLocator.GameObjectOf(this);
            }

            return null;
        }
    }
}
