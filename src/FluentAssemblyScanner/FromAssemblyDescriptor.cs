using System;
using System.Collections.Generic;
using System.Reflection;

namespace FluentAssemblyScanner
{
    public class FromAssemblyDescriptor : FromDescriptor
    {
        protected readonly IEnumerable<Assembly> Assemblies;

        protected internal FromAssemblyDescriptor(Assembly assembly, Predicate<Type> additionalFilters) :
            base(additionalFilters, new Assembly[] {assembly})
        {
            Assemblies = new[] {assembly};
        }

        protected internal FromAssemblyDescriptor(IEnumerable<Assembly> assemblies, Predicate<Type> additionalFilters)
            : base(additionalFilters, assemblies)
        {
            Assemblies = assemblies;
        }
    }
}