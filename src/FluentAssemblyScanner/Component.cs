using System;

namespace FluentAssemblyScanner
{
    internal static class Component
    {
        public static bool HasAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            return Attribute.IsDefined(type, typeof(TAttribute));
        }

        public static Predicate<Type> IsInNamespace(string @namespace)
        {
            return IsInNamespace(@namespace, false);
        }

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

        public static Predicate<Type> IsInSameNamespaceOf(Type type)
        {
            return IsInNamespace(type.Namespace);
        }

        public static Predicate<Type> IsInSameNamespaceOf(Type type, bool includeSubnamespaces)
        {
            return IsInNamespace(type.Namespace, includeSubnamespaces);
        }

        public static Predicate<Type> IsInSameNamespaceOf<T>()
        {
            return IsInSameNamespaceOf(typeof(T));
        }

        public static Predicate<Type> IsInSameNamespaceOf<T>(bool includeSubnamespaces)
        {
            return IsInSameNamespaceOf(typeof(T), includeSubnamespaces);
        }
    }
}