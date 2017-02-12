using System;

namespace FluentAssemblyScanner
{
    internal static class ComponentExtensions
    {
        /// <summary>
        ///     Determines whether the specified type has attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     <c>true</c> if the specified type has attribute; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            return HasAttribute(type, typeof(TAttribute));
        }

        /// <summary>
        ///     Determines whether the specified type has attribute.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns>
        ///     <c>true</c> if the specified type has attribute; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttribute(Type type, Type attributeType)
        {
            return Attribute.IsDefined(type, attributeType);
        }

        /// <summary>
        ///     Determines whether [is in namespace] [the specified namespace].
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <returns></returns>
        public static Predicate<Type> IsInNamespace(string @namespace)
        {
            return IsInNamespace(@namespace, false);
        }

        /// <summary>
        ///     Determines whether [is in namespace] [the specified namespace].
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="includeSubnamespaces">if set to <c>true</c> [include subnamespaces].</param>
        /// <returns></returns>
        public static Predicate<Type> IsInNamespace(string @namespace, bool includeSubnamespaces)
        {
            if (includeSubnamespaces)
            {
                return type => type.Namespace == @namespace ||
                               type.Namespace != null &&
                               type.Namespace.StartsWith(@namespace + ".");
            }

            return type => type.Namespace == @namespace;
        }

        /// <summary>
        ///     Determines whether [is in same namespace of] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Predicate<Type> IsInSameNamespaceOf(Type type)
        {
            return IsInNamespace(type.Namespace);
        }

        /// <summary>
        ///     Determines whether [is in same namespace of] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="includeSubnamespaces">if set to <c>true</c> [include subnamespaces].</param>
        /// <returns></returns>
        public static Predicate<Type> IsInSameNamespaceOf(Type type, bool includeSubnamespaces)
        {
            return IsInNamespace(type.Namespace, includeSubnamespaces);
        }

        /// <summary>
        ///     Determines whether [is in same namespace of].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Predicate<Type> IsInSameNamespaceOf<T>()
        {
            return IsInSameNamespaceOf(typeof(T));
        }

        /// <summary>
        ///     Determines whether [is in same namespace of] [the specified include subnamespaces].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeSubnamespaces">if set to <c>true</c> [include subnamespaces].</param>
        /// <returns></returns>
        public static Predicate<Type> IsInSameNamespaceOf<T>(bool includeSubnamespaces)
        {
            return IsInSameNamespaceOf(typeof(T), includeSubnamespaces);
        }
    }
}
