using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssemblyScanner
{
    public class FilterDefiner
    {
        /// <summary>
        ///     The filter actions
        /// </summary>
        private readonly List<Func<Type, bool>> _filterActions;

        /// <summary>
        ///     The types
        /// </summary>
        private readonly List<Type> _types;

        /// <summary>
        ///     The and filter
        /// </summary>
        private Predicate<Type> _andFilter;

        /// <summary>
        ///     The method filters
        /// </summary>
        private Predicate<MethodInfo> _methodFilters;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterDefiner" /> class.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="filterActions">The filter actions.</param>
        public FilterDefiner(List<Type> types, List<Func<Type, bool>> filterActions)
        {
            _andFilter = type => true;
            _methodFilters = info => true;

            _types = types;
            _filterActions = filterActions;

            filterActions.Add(type => _andFilter(type));
            filterActions.Add(type => type.GetMethods().Any(method => _methodFilters.ApplyTo(method)));
        }

        /// <summary>
        ///     Wheres the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public FilterDefiner Where(Predicate<Type> filter)
        {
            _andFilter += filter;
            return this;
        }

        /// <summary>
        ///     Scans this instance.
        /// </summary>
        /// <returns></returns>
        public List<Type> Scan()
        {
            return _types.Whereify(_filterActions)
                .ToList();
        }

        /// <summary>
        ///     Classeses this instance.
        /// </summary>
        /// <returns></returns>
        public FilterDefiner Classes()
        {
            Where(type => type.IsClass && !type.IsInterface);
            return this;
        }

        /// <summary>
        ///     Just interfaces.
        /// </summary>
        /// <returns></returns>
        public FilterDefiner Interfaces()
        {
            Where(type => !type.IsClass && type.IsInterface);
            return this;
        }

        /// <summary>
        ///     Eliminates all abstract classes.
        /// </summary>
        /// <returns></returns>
        public FilterDefiner NonAbstract()
        {
            Where(type => type.IsAbstract == false);
            return this;
        }

        /// <summary>
        ///     Nons the static.
        /// </summary>
        /// <returns></returns>
        public FilterDefiner NonStatic()
        {
            Where(type => type.IsAbstract == false && type.IsSealed == false);
            return this;
        }

        /// <summary>
        ///     Nons the attribute.
        /// </summary>
        /// <returns></returns>
        public FilterDefiner NonAttribute()
        {
            Where(type => !typeof(Attribute).IsAssignableFrom(type));
            return this;
        }

        /// <summary>
        ///     Methods the has attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns></returns>
        public FilterDefiner MethodHasAttribute<TAttribute>() where TAttribute : Attribute
        {
            return MethodHasAttribute(typeof(TAttribute));
        }

        /// <summary>
        ///     Methods the has attribute.
        /// </summary>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        public FilterDefiner MethodHasAttribute(Type attributeType)
        {
            _methodFilters += method => method.GetCustomAttributes(attributeType).Any();
            return this;
        }

        /// <summary>
        ///     Methods the name.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        public FilterDefiner MethodName(string methodName)
        {
            _methodFilters += method => method.Name == methodName;
            return this;
        }

        /// <summary>
        ///     Methods the name contains.
        /// </summary>
        /// <param name="methodText">The method text.</param>
        /// <returns></returns>
        public FilterDefiner MethodNameContains(string methodText)
        {
            _methodFilters += method => method.Name.Contains(methodText);
            return this;
        }
    }
}