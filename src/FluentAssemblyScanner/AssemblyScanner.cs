﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentAssemblyScanner
{
    public class AssemblyScanner
    {
        /// <summary>
        ///     Froms the assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static FromAssemblyDefiner FromAssembly(Assembly assembly)
        {
            Check.NotNull(assembly, nameof(assembly));

            return new FromAssemblyDefiner(assembly);
        }

        /// <summary>
        ///     Froms the assembly containing.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static FromAssemblyDefiner FromAssemblyContaining(Type type)
        {
            Check.NotNull(type, nameof(type));

            return new FromAssemblyDefiner(type.Assembly);
        }

        /// <summary>
        ///     Froms the assembly containing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static FromAssemblyDefiner FromAssemblyContaining<T>()
        {
            return FromAssemblyContaining(typeof(T));
        }

        /// <summary>
        ///     Froms the assembly in directory.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public static FromAssemblyDefiner FromAssemblyInDirectory(AssemblyFilter filter)
        {
            Check.NotNull(filter, nameof(filter));

            var assemblies = ReflectionUtil.GetAssemblies(filter);
            return new FromAssemblyDefiner(assemblies);
        }

        /// <summary>
        ///     Scans current assembly and all refernced assemblies with the same first part of the name.
        /// </summary>
        /// <returns></returns>
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
            var assemblies =
                new HashSet<Assembly>(ReflectionUtil.GetApplicationAssemblies(Assembly.GetCallingAssembly()));
            return new FromAssemblyDefiner(assemblies);
        }

        /// <summary>
        ///     Froms the assembly in this application directory.
        /// </summary>
        /// <returns></returns>
        public static FromAssemblyDefiner FromAssemblyInThisApplicationDirectory()
        {
            var assemblies =
                ReflectionUtil.GetAssemblies(new AssemblyFilter(AppDomain.CurrentDomain.GetActualDomainPath()));
            return new FromAssemblyDefiner(assemblies);
        }

        /// <summary>
        ///     Froms the assembly matching named.
        /// </summary>
        /// <param name="assemblyPrefix">The assembly prefix.</param>
        /// <param name="assemblyFilter">The assembly filter.</param>
        /// <returns></returns>
        public static FromAssemblyDefiner FromAssemblyMatchingNamed(string assemblyPrefix,
            AssemblyFilter assemblyFilter)
        {
            var assemblies = ReflectionUtil.GetAssembliesContains(assemblyPrefix, assemblyFilter);
            return new FromAssemblyDefiner(assemblies);
        }

        /// <summary>
        ///     Froms the assembly named.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns></returns>
        public static FromAssemblyDefiner FromAssemblyNamed(string assemblyName)
        {
            var assembly = ReflectionUtil.GetAssemblyNamed(assemblyName);
            return FromAssembly(assembly);
        }

        /// <summary>
        ///     Froms the assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns></returns>
        public static FromAssemblyDefiner FromAssemblies(IEnumerable<Assembly> assemblies)
        {
            Check.NotNull(assemblies, nameof(assemblies));

            return new FromAssemblyDefiner(assemblies);
        }

        /// <summary>
        ///     Froms the this assembly.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static FromAssemblyDefiner FromThisAssembly()
        {
            return FromAssembly(Assembly.GetCallingAssembly());
        }
    }
}