using System;

namespace UNKO.ServiceLocator
{
    /// <summary>
    /// <see cref="FromServiceLocatorAttribute"/>와 기능 동일
    /// </summary>
    public class FromSLAttribute : Attribute, IFromServiceLocatorAttribute
    {
        FromWhere _where; public FromWhere Where => _where;
        bool _lazyable; public bool Lazyable => _lazyable;

        public FromSLAttribute(FromWhere where = FromWhere.Global, bool lazy = true)
        {
            _where = where;
            _lazyable = lazy;
        }
    }
}
