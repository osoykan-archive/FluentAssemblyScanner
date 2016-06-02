using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssemblyScanner.Util;

namespace FluentAssemblyScanner
{
    public class FromAssemblyDefiner : FromAssemblyDefinerBase
    {
        protected bool NonPublicTypes;

        protected internal FromAssemblyDefiner(Assembly assembly)
            : base(new Assembly[] {assembly}) {}

        protected internal FromAssemblyDefiner(IEnumerable<Assembly> assemblies)
            : base(assemblies) {}

        public FromAssemblyDefiner IncludeNonPublicTypes()
        {
            NonPublicTypes = true;
            return this;
        }

        public FromAssemblyDefiner ExcludeAssemblyNamed(string assemblyName)
        {
            var assembly = ReflectionUtil.GetAssemblyNamed(assemblyName);
            AssemblyFilter += assemblies => assemblies.Except(new[] {assembly});
            return this;
        }

        public FromAssemblyDefiner ExcludeAssemblyContaining<T>()
        {
            return ExcludeAssemblyNamed(typeof(T).Assembly.FullName);
        }

        public override IEnumerable<Type> SelectedTypes()
        {
            AssemblyFilter?.Invoke(Assemblies);

            return Assemblies.SelectMany(a => a.GetAvailableTypesOrdered(NonPublicTypes));
        }
    }
}