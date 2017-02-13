using System;
using System.Collections.Generic;
using System.Reflection;

using JetBrains.Annotations;

namespace FluentAssemblyScanner
{
    public abstract class FilterDefinerBase
    {
        /// <summary>
        ///     The and filter
        /// </summary>
        protected Predicate<Type> AndFilter;

        /// <summary>
        ///     The method filters
        /// </summary>
        protected Predicate<MethodInfo> MethodFilters;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterDefinerBase" /> class.
        /// </summary>
        /// <param name="types">The types.</param>
        protected FilterDefinerBase([NotNull] List<Type> types)
        {
            AndFilter = type => true;
            MethodFilters = info => true;
        }

        /// <summary>
        ///     Scans this instance.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public abstract List<Type> Scan();

        /// <summary>
        ///     Wheres the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [NotNull]
        public FilterDefinerBase Where([NotNull] Predicate<Type> filter)
        {
            AndFilter += filter;
            return this;
        }
    }
}
