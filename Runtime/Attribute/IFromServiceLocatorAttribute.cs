namespace UNKO.ServiceLocator
{
    public interface IFromServiceLocatorAttribute
    {
        public FromWhere Where { get; }
        public bool Lazyable { get; }
    }
}
