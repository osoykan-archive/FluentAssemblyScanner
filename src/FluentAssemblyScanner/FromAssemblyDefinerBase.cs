using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssemblyScanner
{
    public abstract class FromAssemblyDefinerBase
    {
        protected IEnumerable<Assembly> Assemblies;
        protected Action<IEnumerable<Assembly>> AssemblyFilter;

        protected internal FromAssemblyDefinerBase(IEnumerable<Assembly> assemblies)
        {
            Assemblies = assemblies;
        }

        public abstract IEnumerable<Type> SelectedTypes();

        public BasedOnDefiner BasedOn<T>()
        {
            return BasedOn(typeof(T));
        }

        public BasedOnDefiner BasedOn(Type basedOn)
        {
            return BasedOn((IEnumerable<Type>)new[] {basedOn});
        }

        public BasedOnDefiner BasedOn(params Type[] basedOn)
        {
            return BasedOn((IEnumerable<Type>)basedOn);
        }

        public BasedOnDefiner BasedOn(IEnumerable<Type> basedOn)
        {
            var descriptor = new BasedOnDefiner(basedOn, this);

            return descriptor;
        }

        public BasedOnDefiner InNamespace(string @namespace)
        {
            return Where(Component.IsInNamespace(@namespace, false));
        }

        public BasedOnDefiner InNamespace(string @namespace, bool includeSubnamespaces)
        {
            return Where(Component.IsInNamespace(@namespace, includeSubnamespaces));
        }

        public BasedOnDefiner InSameNamespaceOf(Type type)
        {
            return Where(Component.IsInSameNamespaceOf(type));
        }

        public BasedOnDefiner InSameNamespaceOf(Type type, bool includeSubnamespaces)
        {
            return Where(Component.IsInSameNamespaceOf(type, includeSubnamespaces));
        }

        public BasedOnDefiner InSameNamespaceOf<T>()
        {
            return Where(Component.IsInSameNamespaceOf<T>());
        }

        public BasedOnDefiner InSameNamespaceOf<T>(bool includeSubnamespaces) where T : class
        {
            return Where(Component.IsInSameNamespaceOf<T>(includeSubnamespaces));
        }

        public BasedOnDefiner PickAny()
        {
            return BasedOn<object>();
        }

        public BasedOnDefiner Where(Predicate<Type> filter)
        {
            var descriptor = new BasedOnDefiner(new[] {typeof(object)}, this).If(filter);

            return (BasedOnDefiner)descriptor;
        }
    }
}