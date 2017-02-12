using System;

using JetBrains.Annotations;

namespace FluentAssemblyScanner
{
    internal static class PredicateExtensions
    {
        /// <summary>
        ///     Applies to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="itemToFilter">The item to filter.</param>
        /// <returns></returns>
        public static bool ApplyTo<T>([CanBeNull] this Predicate<T> predicate, T itemToFilter)
        {
            if (predicate == null)
            {
                return true;
            }

            foreach (Delegate filterDelegate in predicate.GetInvocationList())
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
