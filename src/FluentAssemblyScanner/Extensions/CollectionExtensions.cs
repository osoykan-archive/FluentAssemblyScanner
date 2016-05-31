using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentAssemblyScanner.Extensions
{
    public static class CollectionExtensions
    {
        public static TResult[] ConvertAll<T, TResult>(this T[] items, Converter<T, TResult> transformation)
        {
            return Array.ConvertAll(items, transformation);
        }

        public static T Find<T>(this T[] items, Predicate<T> predicate)
        {
            return Array.Find(items, predicate);
        }

        public static T[] FindAll<T>(this T[] items, Predicate<T> predicate)
        {
            return Array.FindAll(items, predicate);
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null)
                return;
            foreach (var obj in items)
                action(obj);
        }

        /// <summary>
        ///     Checks whether or not collection is null or empty. Assumes colleciton can be safely enumerated multiple times.
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this IEnumerable @this)
        {
            if (@this != null)
                return !@this.GetEnumerator().MoveNext();
            return true;
        }
    }
}