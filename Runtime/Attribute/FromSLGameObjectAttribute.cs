using System;

namespace UNKO.ServiceLocator
{
    /// <summary>
    /// <see cref="FromServiceLocatorAttribute"/>에서 <see cref="FromWhere.GameObject"/>로 지정한 것, 휴먼 에러가 많아서 만듦
    /// </summary>
    public class FromSLGameObjectAttribute : Attribute, IFromServiceLocatorAttribute
    {
        public FromWhere Where => FromWhere.GameObject;
        bool _lazyable; public bool Lazyable => _lazyable;

        public FromSLGameObjectAttribute(bool lazy = true)
        {
            _lazyable = lazy;
        }
    }
}
