using System;

namespace UNKO.ServiceLocator
{
    public class FromServiceLocatorAttribute : Attribute, IFromServiceLocatorAttribute
    {
        FromWhere _where; public FromWhere Where => _where;
        bool _lazyable; public bool Lazyable => _lazyable;

        public FromServiceLocatorAttribute(FromWhere where = FromWhere.Global, bool lazy = true)
        {
            _where = where;
            _lazyable = lazy;
        }
    }
}
