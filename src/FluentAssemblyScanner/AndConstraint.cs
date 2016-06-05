namespace FluentAssemblyScanner
{
    public class AndConstraint<T>
    {
        public AndConstraint(T parentConstraint)
        {
            And = parentConstraint;
        }

        public T And { get; }

        public IAfterFilter Then()
        {
            return (IAfterFilter)And;
        }
    }
}