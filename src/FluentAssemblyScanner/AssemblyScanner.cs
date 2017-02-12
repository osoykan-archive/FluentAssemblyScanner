using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentAssemblyScanner
{
    public class AssemblyScanner
    {
        public static FromAssemblyDefiner FromAssembly(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            return new FromAssemblyDefiner(assembly);
        }

        public static FromAssemblyDefiner FromAssemblyContaining(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return new FromAssemblyDefiner(type.Assembly);
        }

        public static FromAssemblyDefiner FromAssemblyContaining<T>()
        {
            return FromAssemblyContaining(typeof(T));
        }

        public static FromAssemblyDefiner FromAssemblyInDirectory(AssemblyFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            IEnumerable<Assembly> assemblies = ReflectionUtil.GetAssemblies(filter);
            return new FromAssemblyDefiner(assemblies);
        }

        /// <summary>Scans current assembly and all refernced assemblies with the same first part of the name.</summary>
        /// <returns> </returns>
        /// <remarks>
        ///     Assemblies are considered to belong to the same application based on the first part of the name. For example if the
        ///     method is called fromAssembly within <c>MyApp.exe</c> and <c>MyApp.exe</c> references
        ///     <c>MyApp.SuperFeatures.dll</c>, <c>mscorlib.dll</c> and <c>ThirdPartyCompany.UberControls.dll</c> the
        ///     <c>MyApp.exe</c> and <c>MyApp.SuperFeatures.dll</c> will be scanned for components, and other
        ///     assemblies will be ignored.
        /// </remarks>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static FromAssemblyDefiner FromAssemblyInThisApplication()
        {
            var assemblies = new HashSet<Assembly>(ReflectionUtil.GetApplicationAssemblies(Assembly.GetCallingAssembly()));
            return new FromAssemblyDefiner(assemblies);
        }

        public static FromAssemblyDefiner FromAssemblyMatchingNamed(string assemblyPrefix, AssemblyFilter assemblyFilter)
        {
            IEnumerable<Assembly> assemblies = ReflectionUtil.GetAssembliesContains(assemblyPrefix, assemblyFilter);
            return new FromAssemblyDefiner(assemblies);
        }

        public static FromAssemblyDefiner FromAssemblyNamed(string assemblyName)
        {
            Assembly assembly = ReflectionUtil.GetAssemblyNamed(assemblyName);
            return FromAssembly(assembly);
        }

        public static FromAssemblyDefiner FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
            {
                throw new ArgumentNullException(nameof(assemblies));
            }

            return new FromAssemblyDefiner(assemblies);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static FromAssemblyDefiner FromThisAssembly()
        {
            return FromAssembly(Assembly.GetCallingAssembly());
        }
    }
}
