using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssemblyScanner
{
    public abstract class FromAssemblyDefinerBase
    {
        protected internal FromAssemblyDefinerBase(IEnumerable<Assembly> assemblies)
        {
            Assemblies = assemblies;
        }

        protected IEnumerable<Assembly> Assemblies;
        protected Action<IEnumerable<Assembly>> AssemblyFilter;

        public abstract IEnumerable<Type> AllTypes();

        public BasedOnDefiner BasedOn<T>()
        {
            return BasedOn(typeof(T));
        }

        public BasedOnDefiner BasedOn(Type basedOn)
        {
            return BasedOn((IEnumerable<Type>)new[] {basedOn});
        }

        public BasedOnDefiner BasedOn(params Type[] basedOn)
        {
            return BasedOn((IEnumerable<Type>)basedOn);
        }

        public BasedOnDefiner BasedOn(IEnumerable<Type> basedOn)
        {
            return new BasedOnDefiner(basedOn, this);
        }

        public BasedOnDefiner PickAny()
        {
            return BasedOn<object>();
        }
    }
}