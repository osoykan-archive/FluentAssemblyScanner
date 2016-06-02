using System;

namespace FluentAssemblyScanner.Extensions
{
    public static class PredicateExtensions
    {
        public static bool ApplyTo<T>(this Predicate<T> predicate, T itemToFilter)
        {
            if (predicate == null)
            {
                return true;
            }

            foreach (var filterDelegate in predicate.GetInvocationList())
            {
                var filter = (Predicate<T>)filterDelegate;
                if (filter(itemToFilter) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}