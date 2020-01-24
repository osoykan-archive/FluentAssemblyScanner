using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssemblyScanner
{
    public class BasedOnDefiner
    {
        private readonly List<Type> _basedOns;
        private readonly FromAssemblyDefiner _fromAssemblyDefiner;
        private Predicate<Type> _typeFilter;

        internal BasedOnDefiner(IEnumerable<Type> basedOns, FromAssemblyDefiner fromAssemblyDefiner)

        {
            _basedOns = basedOns.ToList();
            _typeFilter = type => true;
            _fromAssemblyDefiner = fromAssemblyDefiner;
        }

        /// <summary>
        ///     Ifs the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        internal BasedOnDefiner If(Predicate<Type> filter)
        {
            _typeFilter += filter;
            return this;
        }

        /// <summary>
        ///     Filters this instance.
        /// </summary>
        /// <returns></returns>
        public FilterDefiner Filter()
        {
            return new FilterDefiner(
                _fromAssemblyDefiner.GetAllTypes().ToList(),
                new List<Func<Type, bool>>
                {
                    type => _basedOns.Any(t => t.IsAssignableFrom(type)),
                    type => _typeFilter.ApplyTo(type)
                });
        }

        /// <summary>
        ///     Determines whether this instance has attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns></returns>
        public BasedOnDefiner HasAttribute<TAttribute>() where TAttribute : Attribute
        {
            Where(ComponentExtensions.HasAttribute<TAttribute>);
            return this;
        }

        /// <summary>
        ///     Determines whether the specified attribute type has attribute.
        /// </summary>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public BasedOnDefiner HasAttribute(Type attributeType)
        {
            Where(type => ComponentExtensions.HasAttribute(type, attributeType));
            return this;
        }

        /// <summary>
        ///     Ins the namespace.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <returns></returns>
        public BasedOnDefiner InNamespace(string @namespace)
        {
            return Where(ComponentExtensions.IsInNamespace(@namespace, false));
        }

        /// <summary>
        ///     Ins the namespace.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        /// <param name="includeSubnamespaces">if set to <c>true</c> [include subnamespaces].</param>
        /// <returns></returns>
        public BasedOnDefiner InNamespace(string @namespace, bool includeSubnamespaces)
        {
            return Where(ComponentExtensions.IsInNamespace(@namespace, includeSubnamespaces));
        }

        /// <summary>
        ///     Ins the same namespace of.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public BasedOnDefiner InSameNamespaceOf(Type type)
        {
            return Where(ComponentExtensions.IsInSameNamespaceOf(type));
        }

        /// <summary>
        ///     Ins the same namespace of.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="includeSubnamespaces">if set to <c>true</c> [include subnamespaces].</param>
        /// <returns></returns>
        public BasedOnDefiner InSameNamespaceOf(Type type, bool includeSubnamespaces)
        {
            return Where(ComponentExtensions.IsInSameNamespaceOf(type, includeSubnamespaces));
        }

        /// <summary>
        ///     Ins the same namespace of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public BasedOnDefiner InSameNamespaceOf<T>()
        {
            return Where(ComponentExtensions.IsInSameNamespaceOf<T>());
        }

        /// <summary>
        ///     Ins the same namespace of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeSubnamespaces">if set to <c>true</c> [include subnamespaces].</param>
        /// <returns></returns>
        public BasedOnDefiner InSameNamespaceOf<T>(bool includeSubnamespaces) where T : class
        {
            return Where(ComponentExtensions.IsInSameNamespaceOf<T>(includeSubnamespaces));
        }

        /// <summary>
        ///     Ors the based on.
        /// </summary>
        /// <param name="basedOn">The based on.</param>
        /// <returns></returns>
        public BasedOnDefiner OrBasedOn(Type basedOn)
        {
            _basedOns.Add(basedOn);
            return this;
        }

        /// <summary>
        ///     Ors the based on.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public BasedOnDefiner OrBasedOn<T>()
        {
            _basedOns.Add(typeof(T));
            return this;
        }

        /// <summary>
        ///     Wheres the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        protected BasedOnDefiner Where(Predicate<Type> filter)
        {
            return If(filter).As<BasedOnDefiner>();
        }
    }
}