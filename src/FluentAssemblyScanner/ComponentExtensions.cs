using System;

namespace FluentAssemblyScanner
{
    internal static class ComponentExtensions
    {
        public static bool HasAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            return HasAttribute(type, typeof(TAttribute));
        }

        public static bool HasAttribute(Type type, Type attributeType)
        {
            return Attribute.IsDefined(type, attributeType);
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