using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssemblyScanner
{
    public abstract class FilterDefinerBase
    {
        protected Predicate<Type> AndFilter;
        protected Predicate<MethodInfo> MethodFilter;

        protected FilterDefinerBase(List<Type> types)
        {
            AndFilter = type => true;
            MethodFilter = info => true;
        }

        public abstract List<Type> Scan();

        public FilterDefinerBase Where(Predicate<Type> filter)
        {
            AndFilter += filter;
            return this;
        }
    }
}