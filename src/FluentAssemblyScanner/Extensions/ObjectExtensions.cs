namespace FluentAssemblyScanner.Extensions
{
    public static class ObjectExtensions
    {
        public static T As<T>(this object @this) where T : class
        {
            return (T)@this;
        }
    }
}