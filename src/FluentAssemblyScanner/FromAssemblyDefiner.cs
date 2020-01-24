using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssemblyScanner
{
    public class FromAssemblyDefiner
    {
        /// <summary>
        ///     The assemblies
        /// </summary>
        private readonly IEnumerable<Assembly> _assemblies;

        /// <summary>
        ///     The assembly filters
        /// </summary>
        private Predicate<Assembly> _assemblyFilter;

        /// <summary>
        ///     Include non public types.
        /// </summary>
        private bool _nonPublicTypes;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FromAssemblyDefiner" /> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        internal FromAssemblyDefiner(Assembly assembly)
            : this(new[] {assembly})
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FromAssemblyDefiner" /> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        internal FromAssemblyDefiner(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
            _assemblyFilter = assembly => true;
        }


        /// <summary>
        ///     Baseds the on.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public BasedOnDefiner BasedOn<T>()
        {
            return BasedOn(typeof(T));
        }

        /// <summary>
        ///     Baseds the on.
        /// </summary>
        /// <param name="basedOn">The based on.</param>
        /// <returns></returns>
        public BasedOnDefiner BasedOn(Type basedOn)
        {
            return BasedOn((IEnumerable<Type>) new[] {basedOn});
        }

        /// <summary>
        ///     Baseds the on.
        /// </summary>
        /// <param name="basedOn">The based on.</param>
        /// <returns></returns>
        public BasedOnDefiner BasedOn(params Type[] basedOn)
        {
            return BasedOn((IEnumerable<Type>) basedOn);
        }

        /// <summary>
        ///     Baseds the on.
        /// </summary>
        /// <param name="basedOn">The based on.</param>
        /// <returns></returns>
        public BasedOnDefiner BasedOn(IEnumerable<Type> basedOn)
        {
            return new BasedOnDefiner(basedOn, this);
        }

        /// <summary>
        ///     Picks any.
        /// </summary>
        /// <returns></returns>
        public BasedOnDefiner PickAny()
        {
            return BasedOn<object>();
        }

        /// <summary>
        ///     Gets all types from searched assemlies according to given criteria.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetAllTypes()
        {
            var filteredAssemblies = _assemblies.Where(_assemblyFilter.ApplyTo);
            return filteredAssemblies.SelectMany(a => a.GetAvailableTypesOrdered(_nonPublicTypes));
        }

        /// <summary>
        ///     Excludes the assembly containing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public FromAssemblyDefiner ExcludeAssemblyContaining<T>()
        {
            return ExcludeAssemblyFullNamed(typeof(T).Assembly.FullName);
        }

        /// <summary>
        ///     Excludes the assembly named.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns></returns>
        public FromAssemblyDefiner ExcludeAssemblyFullNamed(string assemblyName)
        {
            _assemblyFilter += assembly => assembly.FullName != assemblyName;
            return this;
        }

        /// <summary>
        ///     Excludes the assembly named.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns></returns>
        public FromAssemblyDefiner ExcludeAssemblyNamed(string assemblyName)
        {
            _assemblyFilter += assembly => assembly.GetName().Name != assemblyName;
            return this;
        }

        /// <summary>
        ///     Excludes the assembly name starts with.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public FromAssemblyDefiner ExcludeAssemblyNameStartsWith(string text)
        {
            _assemblyFilter += assembly =>
                !assembly.GetName().Name.StartsWith(text, StringComparison.OrdinalIgnoreCase);
            return this;
        }

        /// <summary>
        ///     Excludes the assembly name ends with.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public FromAssemblyDefiner ExcludeAssemblyNameEndsWith(string text)
        {
            _assemblyFilter += assembly => !assembly.GetName().Name.EndsWith(text, StringComparison.OrdinalIgnoreCase);
            return this;
        }

        /// <summary>
        ///     Excludes the assembly name contains.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public FromAssemblyDefiner ExcludeAssemblyNameContains(string text)
        {
            _assemblyFilter += assembly => !assembly.GetName().Name.Contains(text);
            return this;
        }

        /// <summary>
        ///     Includes the non public types.
        /// </summary>
        /// <returns></returns>
        public FromAssemblyDefiner IncludeNonPublicTypes()
        {
            _nonPublicTypes = true;
            return this;
        }

        /// <summary>
        ///     Ignores the dynamic assemblies.
        /// </summary>
        /// <returns></returns>
        public FromAssemblyDefiner IgnoreDynamicAssemblies()
        {
            _assemblyFilter += assembly => assembly.IsDynamic == false;
            return this;
        }
    }
}