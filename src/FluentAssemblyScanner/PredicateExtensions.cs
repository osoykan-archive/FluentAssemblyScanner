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

        /// <summary>
        ///     Ands the specified additional predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="existingPredicate">The existing predicate.</param>
        /// <param name="additionalPredicate">The additional predicate.</param>
        /// <returns></returns>
        [NotNull]
        public static Predicate<T> And<T>([NotNull] this Predicate<T> existingPredicate, [NotNull] Predicate<T> additionalPredicate)
        {
            Check.NotNull(existingPredicate, nameof(existingPredicate));

            existingPredicate += additionalPredicate;
            return existingPredicate;
        }
    }
}
