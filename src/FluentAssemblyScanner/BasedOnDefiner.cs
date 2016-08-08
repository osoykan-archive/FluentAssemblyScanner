using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssemblyScanner.Extensions;

namespace FluentAssemblyScanner
{
    public class BasedOnDefiner : BasedOnDefinerBase
    {
        private readonly FromAssemblyDefinerBase fromAssemblyDefinerBase;

        internal BasedOnDefiner(IEnumerable<Type> basedOns, FromAssemblyDefinerBase fromAssemblyDefinerBase) : base(basedOns)
        {
            this.fromAssemblyDefinerBase = fromAssemblyDefinerBase;
        }

        public FilterDefiner Filter()
        {
            return new FilterDefiner(
                fromAssemblyDefinerBase.AllTypes().ToList(),
                new List<Func<Type, bool>>
                {
                    type => BasedOns.Any(t => t.IsAssignableFrom(type)),
                    type => TypeFilter.ApplyTo(type)
                });
        }

        public BasedOnDefiner HasAttribute<TAttribute>() where TAttribute : Attribute
        {
            Where(ComponentExtensions.HasAttribute<TAttribute>);
            return this;
        }

        public BasedOnDefiner HasAttribute(Type attributeType)
        {
            Where(type => ComponentExtensions.HasAttribute(type, attributeType));
            return this;
        }

        public BasedOnDefiner InNamespace(string @namespace)
        {
            return Where(ComponentExtensions.IsInNamespace(@namespace, false));
        }

        public BasedOnDefiner InNamespace(string @namespace, bool includeSubnamespaces)
        {
            return Where(ComponentExtensions.IsInNamespace(@namespace, includeSubnamespaces));
        }

        public BasedOnDefiner InSameNamespaceOf(Type type)
        {
            return Where(ComponentExtensions.IsInSameNamespaceOf(type));
        }

        public BasedOnDefiner InSameNamespaceOf(Type type, bool includeSubnamespaces)
        {
            return Where(ComponentExtensions.IsInSameNamespaceOf(type, includeSubnamespaces));
        }

        public BasedOnDefiner InSameNamespaceOf<T>()
        {
            return Where(ComponentExtensions.IsInSameNamespaceOf<T>());
        }

        public BasedOnDefiner InSameNamespaceOf<T>(bool includeSubnamespaces) where T : class
        {
            return Where(ComponentExtensions.IsInSameNamespaceOf<T>(includeSubnamespaces));
        }

        public BasedOnDefiner OrBasedOn(Type basedOn)
        {
            BasedOns.Add(basedOn);
            return this;
        }

        public BasedOnDefiner OrBasedOn<T>()
        {
            BasedOns.Add(typeof(T));
            return this;
        }

        protected BasedOnDefiner Where(Predicate<Type> filter)
        {
            return If(filter).As<BasedOnDefiner>();
        }
    }
}