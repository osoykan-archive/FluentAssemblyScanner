using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace FluentAssemblyScanner
{
    /// <seealso cref="FluentAssemblyScanner.FromAssemblyDefinerBase" />
    public class FromAssemblyDefiner : FromAssemblyDefinerBase
    {
        /// <summary>
        ///     Include non public types.
        /// </summary>
        private bool _nonPublicTypes;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FromAssemblyDefiner" /> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        protected internal FromAssemblyDefiner([NotNull] Assembly assembly)
            : base(new Assembly[] { assembly })
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FromAssemblyDefiner" /> class.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        protected internal FromAssemblyDefiner([NotNull] IEnumerable<Assembly> assemblies)
            : base(assemblies)
        {
        }

        /// <summary>
        ///     Gets all types from searched assemlies according to given criteria.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Type> GetAllTypes()
        {
            IEnumerable<Assembly> filteredAssemblies = Assemblies.Where(AssemblyFilter.ApplyTo);
            return filteredAssemblies.SelectMany(a => a.GetAvailableTypesOrdered(_nonPublicTypes));
        }

        /// <summary>
        ///     Excludes the assembly containing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [NotNull]
        public FromAssemblyDefiner ExcludeAssemblyContaining<T>()
        {
            return ExcludeAssemblyFullNamed(typeof(T).Assembly.FullName);
        }

        /// <summary>
        ///     Excludes the assembly named.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns></returns>
        [NotNull]
        public FromAssemblyDefiner ExcludeAssemblyFullNamed([NotNull] string assemblyName)
        {
            AssemblyFilter += assembly => assembly.FullName != assemblyName;
            return this;
        }

        /// <summary>
        ///     Excludes the assembly named.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns></returns>
        [NotNull]
        public FromAssemblyDefiner ExcludeAssemblyNamed([NotNull] string assemblyName)
        {
            AssemblyFilter += assembly => assembly.GetName().Name != assemblyName;
            return this;
        }

        /// <summary>
        ///     Excludes the assembly name starts with.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        [NotNull]
        public FromAssemblyDefiner ExcludeAssemblyNameStartsWith([NotNull] string text)
        {
            AssemblyFilter += assembly => !assembly.GetName().Name.StartsWith(text, StringComparison.OrdinalIgnoreCase);
            return this;
        }

        /// <summary>
        ///     Excludes the assembly name ends with.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        [NotNull]
        public FromAssemblyDefiner ExcludeAssemblyNameEndsWith([NotNull] string text)
        {
            AssemblyFilter += assembly => !assembly.GetName().Name.EndsWith(text, StringComparison.OrdinalIgnoreCase);
            return this;
        }

        /// <summary>
        ///     Excludes the assembly name contains.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        [NotNull]
        public FromAssemblyDefiner ExcludeAssemblyNameContains([NotNull] string text)
        {
            AssemblyFilter += assembly => !assembly.GetName().Name.Contains(text);
            return this;
        }

        /// <summary>
        ///     Includes the non public types.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public FromAssemblyDefiner IncludeNonPublicTypes()
        {
            _nonPublicTypes = true;
            return this;
        }

        /// <summary>
        ///     Ignores the dynamic assemblies.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public FromAssemblyDefiner IgnoreDynamicAssemblies()
        {
            AssemblyFilter += assembly => assembly.IsDynamic == false;
            return this;
        }
    }
}
