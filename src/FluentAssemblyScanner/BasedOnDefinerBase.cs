using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssemblyScanner
{
    public abstract class BasedOnDefinerBase
    {
        protected readonly List<Type> BasedOns;
        protected Predicate<MethodInfo> MethodFilter;
        protected Predicate<Type> TypeFilter;

        protected BasedOnDefinerBase(IEnumerable<Type> basedOns)
        {
            BasedOns = basedOns.ToList();
        }

        protected abstract bool ApplyBasedOnFilter(Type type);

        protected abstract bool ApplyIfFilter(Type type);

        protected abstract bool ApplyMethodFilter(MethodInfo method);

        public abstract List<Type> Scan();

        public BasedOnDefinerBase WithMethodNamed(string methodName)
        {
            MethodFilter += method => method.Name == methodName;
            return this;
        }

        public BasedOnDefinerBase If(Predicate<Type> filter)
        {
            TypeFilter += filter;
            return this;
        }

        public BasedOnDefinerBase OrBasedOn(Type basedOn)
        {
            BasedOns.Add(basedOn);
            return this;
        }

        public BasedOnDefinerBase OrBasedOn<T>()
        {
            BasedOns.Add(typeof(T));
            return this;
        }
    }
}