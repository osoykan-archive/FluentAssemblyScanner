using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssemblyScanner.Util;

namespace FluentAssemblyScanner
{
    public class FromAssemblyDescriptor : FromDescriptor
    {
        protected bool NonPublicTypes;

        protected internal FromAssemblyDescriptor(Assembly assembly)
            : base(new Assembly[] {assembly}) {}

        protected internal FromAssemblyDescriptor(IEnumerable<Assembly> assemblies)
            : base(assemblies) {}

        public FromAssemblyDescriptor IncludeNonPublicTypes()
        {
            NonPublicTypes = true;
            return this;
        }

        public FromAssemblyDescriptor ExcludeAssemblyNamed(string assemblyName)
        {
            var assembly = ReflectionUtil.GetAssemblyNamed(assemblyName);
            assemblyFilter += assemblies => assemblies.Except(new[] {assembly});
            return this;
        }

        public FromAssemblyDescriptor ExcludeAssemblyContaining<T>()
        {
            return ExcludeAssemblyNamed(typeof(T).Assembly.FullName);
        }

        public override IEnumerable<Type> SelectedTypes()
        {
            if (assemblyFilter != null)
            {
                Assemblies = assemblyFilter(Assemblies);
            }

            return Assemblies.SelectMany(a => a.GetAvailableTypesOrdered(NonPublicTypes));
        }
    }
}