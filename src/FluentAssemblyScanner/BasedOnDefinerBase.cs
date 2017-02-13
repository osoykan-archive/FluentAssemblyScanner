using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssemblyScanner
{
    public abstract class BasedOnDefinerBase
    {
        protected readonly List<Type> BasedOns;
        protected Predicate<Type> TypeFilter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BasedOnDefinerBase" /> class.
        /// </summary>
        /// <param name="basedOns">The based ons.</param>
        protected BasedOnDefinerBase(IEnumerable<Type> basedOns)
        {
            BasedOns = basedOns.ToList();
            TypeFilter = type => true;
        }

        /// <summary>
        ///     Ifs the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        protected BasedOnDefinerBase If(Predicate<Type> filter)
        {
            TypeFilter += filter;
            return this;
        }
    }
}
