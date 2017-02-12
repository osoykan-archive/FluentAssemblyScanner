using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace FluentAssemblyScanner
{
    public class FilterDefiner : FilterDefinerBase
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
        ///     Initializes a new instance of the <see cref="FilterDefiner" /> class.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="filterActions">The filter actions.</param>
        public FilterDefiner([NotNull] List<Type> types, [NotNull] List<Func<Type, bool>> filterActions) 
            : base(types)
        {
            _types = types;
            _filterActions = filterActions;

            filterActions.Add(type => AndFilter(type));
            filterActions.Add(type => type.GetMethods().Any(method => MethodFilters.ApplyTo(method)));
        }

        /// <summary>
        ///     Scans this instance.
        /// </summary>
        /// <returns></returns>
        public override List<Type> Scan()
        {
            return _types.Whereify(_filterActions)
                         .ToList();
        }

        /// <summary>
        ///     Classeses this instance.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public FilterDefiner Classes()
        {
            Where(type => type.IsClass && type.IsAbstract == false);
            return this;
        }

        /// <summary>
        ///     Methods the has attribute.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns></returns>
        [NotNull]
        public FilterDefiner MethodHasAttribute<TAttribute>() where TAttribute : Attribute
        {
            return MethodHasAttribute(typeof(TAttribute));
        }

        /// <summary>
        ///     Methods the has attribute.
        /// </summary>
        /// <param name="attributeType">Type of the attribute.</param>
        /// <returns></returns>
        [NotNull]
        public FilterDefiner MethodHasAttribute(Type attributeType)
        {
            MethodFilters += method => method.GetCustomAttributes(attributeType).Any();
            return this;
        }

        /// <summary>
        ///     Methods the name.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        [NotNull]
        public FilterDefiner MethodName(string methodName)
        {
            MethodFilters += method => method.Name == methodName;
            return this;
        }

        /// <summary>
        ///     Methods the name contains.
        /// </summary>
        /// <param name="methodText">The method text.</param>
        /// <returns></returns>
        [NotNull]
        public FilterDefiner MethodNameContains(string methodText)
        {
            MethodFilters += method => method.Name.Contains(methodText);
            return this;
        }

        /// <summary>
        ///     Nons the static.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public FilterDefiner NonStatic()
        {
            Where(type => type.IsAbstract == false && type.IsSealed == false);
            return this;
        }
    }
}
