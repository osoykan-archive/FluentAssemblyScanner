using System;

namespace FluentAssemblyScanner
{
    public static class Component
    {
        public static bool HasAttribute<TAttribute>(Type type) where TAttribute : Attribute
        {
            return Attribute.IsDefined(type, typeof(TAttribute));
        }

        public static Predicate<Type> HasAttribute<TAttribute>(Predicate<TAttribute> filter) where TAttribute : Attribute
        {
            return type => HasAttribute<TAttribute>(type) && filter((TAttribute)Attribute.GetCustomAttribute(type, typeof(TAttribute)));
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

        public static Predicate<Type> IsInSameNamespaceAs(Type type)
        {
            return IsInNamespace(type.Namespace);
        }

        public static Predicate<Type> IsInSameNamespaceAs(Type type, bool includeSubnamespaces)
        {
            return IsInNamespace(type.Namespace, includeSubnamespaces);
        }

        public static Predicate<Type> IsInSameNamespaceAs<T>()
        {
            return IsInSameNamespaceAs(typeof(T));
        }

        public static Predicate<Type> IsInSameNamespaceAs<T>(bool includeSubnamespaces)
        {
            return IsInSameNamespaceAs(typeof(T), includeSubnamespaces);
        }
    }
}