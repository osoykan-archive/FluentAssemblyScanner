﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssemblyScanner
{
    public class FromDescriptor
    {
        private readonly Predicate<Type> additionalFilters;
        private readonly IEnumerable<Assembly> assemblies;

        protected FromDescriptor(Predicate<Type> additionalFilters, IEnumerable<Assembly> assemblies)
        {
            this.additionalFilters = additionalFilters;
            this.assemblies = assemblies;
        }

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
            var descriptor = new BasedOnDescriptor(basedOn, additionalFilters, assemblies);

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

        public BasedOnDescriptor InSameNamespaceAs(Type type)
        {
            return Where(Component.IsInSameNamespaceAs(type));
        }

        public BasedOnDescriptor InSameNamespaceAs(Type type, bool includeSubnamespaces)
        {
            return Where(Component.IsInSameNamespaceAs(type, includeSubnamespaces));
        }

        public BasedOnDescriptor InSameNamespaceAs<T>()
        {
            return Where(Component.IsInSameNamespaceAs<T>());
        }

        public BasedOnDescriptor InSameNamespaceAs<T>(bool includeSubnamespaces) where T : class
        {
            return Where(Component.IsInSameNamespaceAs<T>(includeSubnamespaces));
        }

        public BasedOnDescriptor PickAny()
        {
            return BasedOn<object>();
        }

        public BasedOnDescriptor Where(Predicate<Type> filter)
        {
            var descriptor = new BasedOnDescriptor(new[] {typeof(object)}, additionalFilters, assemblies)
                .If(filter);

            return descriptor;
        }
    }
}