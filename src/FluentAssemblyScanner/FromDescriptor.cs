using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssemblyScanner
{
    public abstract class FromDescriptor
    {
        protected IEnumerable<Assembly> Assemblies;
        protected Func<IEnumerable<Assembly>, IEnumerable<Assembly>> assemblyFilter;

        protected internal FromDescriptor(IEnumerable<Assembly> assemblies)
        {
            Assemblies = assemblies;
        }

        public abstract IEnumerable<Type> SelectedTypes();

        public BasedOnDescriptor BasedOn<T>()
        {
            return BasedOn(typeof(T));
        }

        public BasedOnDescriptor BasedOn(Type basedOn)
        {
            return BasedOn((IEnumerable<Type>)new[] {basedOn});
        }

        public BasedOnDescriptor BasedOn(params Type[] basedOn)
        {
            return BasedOn((IEnumerable<Type>)basedOn);
        }

        public BasedOnDescriptor BasedOn(IEnumerable<Type> basedOn)
        {
            var descriptor = new BasedOnDescriptor(basedOn, this);

            return descriptor;
        }

        public BasedOnDescriptor InNamespace(string @namespace)
        {
            return Where(Component.IsInNamespace(@namespace, false));
        }

        public BasedOnDescriptor InNamespace(string @namespace, bool includeSubnamespaces)
        {
            return Where(Component.IsInNamespace(@namespace, includeSubnamespaces));
        }

        public BasedOnDescriptor InSameNamespaceOf(Type type)
        {
            return Where(Component.IsInSameNamespaceOf(type));
        }

        public BasedOnDescriptor InSameNamespaceOf(Type type, bool includeSubnamespaces)
        {
            return Where(Component.IsInSameNamespaceOf(type, includeSubnamespaces));
        }

        public BasedOnDescriptor InSameNamespaceOf<T>()
        {
            return Where(Component.IsInSameNamespaceOf<T>());
        }

        public BasedOnDescriptor InSameNamespaceOf<T>(bool includeSubnamespaces) where T : class
        {
            return Where(Component.IsInSameNamespaceOf<T>(includeSubnamespaces));
        }

        public BasedOnDescriptor PickAny()
        {
            return BasedOn<object>();
        }

        public BasedOnDescriptor Where(Predicate<Type> filter)
        {
            var descriptor = new BasedOnDescriptor(new[] {typeof(object)}, this).If(filter);

            return descriptor;
        }
    }
}