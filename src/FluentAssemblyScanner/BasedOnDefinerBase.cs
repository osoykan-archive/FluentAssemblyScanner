using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAssemblyScanner
{
    public abstract class BasedOnDefinerBase
    {
        protected readonly List<Type> BasedOns;
        protected Predicate<Type> TypeFilter;

        protected BasedOnDefinerBase(IEnumerable<Type> basedOns)
        {
            BasedOns = basedOns.ToList();
            TypeFilter = type => true;
        }

        protected BasedOnDefinerBase If(Predicate<Type> filter)
        {
            TypeFilter += filter;
            return this;
        }
    }
}