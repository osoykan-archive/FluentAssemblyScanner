using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssemblyScanner
{
    internal static class Extensions
    {
        public static string GetActualDomainPath(this AppDomain @this)
        {
            return @this.RelativeSearchPath ?? @this.BaseDirectory;
        }

        /// <summary>
        ///     Casts the specified this.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this">The this.</param>
        /// <returns></returns>
        public static T As<T>(this object @this) where T : class
        {
            Check.NotNull(@this, nameof(@this));

            return (T) @this;
        }

        /// <summary>
        ///     Applies to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <param name="itemToFilter">The item to filter.</param>
        /// <returns></returns>
        public static bool ApplyTo<T>(this Predicate<T> predicate, T itemToFilter)
        {
            if (predicate == null) return true;

            foreach (var filterDelegate in predicate.GetInvocationList())
            {
                var filter = (Predicate<T>) filterDelegate;
                if (filter(itemToFilter) == false) return false;
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
        public static Predicate<T> And<T>(this Predicate<T> existingPredicate, Predicate<T> additionalPredicate)
        {
            Check.NotNull(existingPredicate, nameof(existingPredicate));

            existingPredicate += additionalPredicate;
            return existingPredicate;
        }

        /// <summary>
        ///     Whereifies the specified predicate funcs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this">The this.</param>
        /// <param name="predicateFuncs">The predicate funcs.</param>
        /// <returns></returns>
        public static IEnumerable<T> Whereify<T>(this IEnumerable<T> @this, IEnumerable<Func<T, bool>> predicateFuncs)
        {
            using (var enumerator = @this.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var currentObject = enumerator.Current;

                    if (predicateFuncs.As<IEnumerable<Func<T, bool>>>().All(x => x.Invoke(currentObject)))
                        yield return currentObject;
                }
            }
        }

         /// <summary>
        ///     Converts all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="transformation">The transformation.</param>
        /// <returns></returns>
        public static TResult[] ConvertAll<T, TResult>(this T[] items, Converter<T, TResult> transformation)
        {
            return Array.ConvertAll(items, transformation);
        }

        /// <summary>
        ///     Finds the specified predicate.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static T Find<T>(this T[] items, Predicate<T> predicate)
        {
            return Array.Find(items, predicate);
        }

        /// <summary>
        ///     Finds all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static T[] FindAll<T>(this T[] items, Predicate<T> predicate)
        {
            return Array.FindAll(items, predicate);
        }

        /// <summary>
        ///     Fors the each.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="action">The action.</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null) return;

            foreach (var obj in items) action(obj);
        }

        /// <summary>
        ///     Checks whether or not collection is null or empty. Assumes colleciton can be safely enumerated multiple times.
        /// </summary>
        /// <param name="this">The this.</param>
        /// <returns>
        ///     <c>true</c> if [is null or empty] [the specified this]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this IEnumerable @this)
        {
            if (@this != null) return !@this.GetEnumerator().MoveNext();

            return true;
        }
    }
}