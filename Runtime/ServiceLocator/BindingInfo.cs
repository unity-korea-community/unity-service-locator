using System;

namespace UNKO.ServiceLocator
{
    [System.Serializable]
    public struct BindingInfo
    {
        public ServiceScope RegistWhere { get; private set; }
        public Type Type { get; private set; }

        public BindingInfo(ServiceScope registWhere, Type type)
        {
            RegistWhere = registWhere;
            Type = type;
        }
    }
}
