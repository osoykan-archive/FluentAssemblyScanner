using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssemblyScanner.Util;

namespace FluentAssemblyScanner
{
    public class FromAssemblyDefiner : FromAssemblyDefinerBase
    {
        protected internal FromAssemblyDefiner(Assembly assembly)
            : base(new Assembly[] {assembly}) {}

        protected internal FromAssemblyDefiner(IEnumerable<Assembly> assemblies)
            : base(assemblies) {}

        protected bool NonPublicTypes;

        public override IEnumerable<Type> AllTypes()
        {
            AssemblyFilter?.Invoke(Assemblies);

            return Assemblies.SelectMany(a => a.GetAvailableTypesOrdered(NonPublicTypes));
        }

        public FromAssemblyDefiner ExcludeAssemblyContaining<T>()
        {
            return ExcludeAssemblyNamed(typeof(T).Assembly.FullName);
        }

        public FromAssemblyDefiner ExcludeAssemblyNamed(string assemblyName)
        {
            var assembly = ReflectionUtil.GetAssemblyNamed(assemblyName);
            AssemblyFilter += assemblies => assemblies.Except(new[] {assembly});
            return this;
        }

        public FromAssemblyDefiner IncludeNonPublicTypes()
        {
            NonPublicTypes = true;
            return this;
        }
    }
}