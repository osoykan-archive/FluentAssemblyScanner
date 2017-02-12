using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace FluentAssemblyScanner
{
    internal static class ReflectionUtil
    {
        private static readonly ConcurrentDictionary<ConstructorInfo, Func<object[], object>> Factories =
            new ConcurrentDictionary<ConstructorInfo, Func<object[], object>>();

        public static readonly Type[] OpenGenericArrayInterfaces = typeof(object[]).GetInterfaces()
                                                                                   .Where(i => i.IsGenericType)
                                                                                   .Select(i => i.GetGenericTypeDefinition())
                                                                                   .ToArray();

        public static TBase CreateInstance<TBase>(this Type subtypeofTBase, params object[] ctorArgs)
        {
            EnsureIsAssignable<TBase>(subtypeofTBase);

            return Instantiate<TBase>(subtypeofTBase, ctorArgs ?? new object[0]);
        }

        public static IEnumerable<Assembly> GetApplicationAssemblies(Assembly rootAssembly)
        {
            var index = rootAssembly.FullName.IndexOfAny(new[] {'.', ','});
            if (index < 0)
                throw new ArgumentException(
                    $"Could not determine application name for assembly \"{rootAssembly.FullName}\". Please use a different method for obtaining assemblies.");

            var applicationName = rootAssembly.FullName.Substring(0, index);
            var assemblies = new HashSet<Assembly>();
            AddApplicationAssemblies(rootAssembly, assemblies, applicationName);
            return assemblies;
        }

        public static IEnumerable<Assembly> GetAssemblies(IAssemblyDiscoverer assemblyDiscoverer)
        {
            return assemblyDiscoverer.GetAssemblies();
        }

        public static IEnumerable<Assembly> GetAssembliesContains(string assemblyPrefix, IAssemblyDiscoverer assemblyDiscoverer)
        {
            return assemblyDiscoverer.GetAssemblies().Where(assembly => assembly.FullName.Contains(assemblyPrefix));
        }

        public static Assembly GetAssemblyNamed(string assemblyName)
        {
            Debug.Assert(string.IsNullOrEmpty(assemblyName) == false);

            try
            {
                Assembly assembly;
                if (IsAssemblyFile(assemblyName))
                    if (Path.GetDirectoryName(assemblyName) == AppDomain.CurrentDomain.BaseDirectory)
                        assembly = Assembly.Load(Path.GetFileNameWithoutExtension(assemblyName));
                    else
                        assembly = Assembly.LoadFile(assemblyName);
                else
                    assembly = Assembly.Load(assemblyName);
                return assembly;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (FileLoadException)
            {
                throw;
            }
            catch (BadImageFormatException)
            {
                throw;
            }
            catch (Exception e)
            {
                // in theory there should be no other exception kind
                throw new Exception($"Could not load assembly {assemblyName}", e);
            }
        }

        public static Assembly GetAssemblyNamed(string filePath, Predicate<AssemblyName> nameFilter, Predicate<Assembly> assemblyFilter)
        {
            var assemblyName = GetAssemblyName(filePath);
            if (nameFilter != null)
                foreach (Predicate<AssemblyName> predicate in nameFilter.GetInvocationList())
                    if (predicate(assemblyName) == false)
                        return null;

            var assembly = LoadAssembly(assemblyName);
            if (assemblyFilter != null)
                foreach (Predicate<Assembly> predicate in assemblyFilter.GetInvocationList())
                    if (predicate(assembly) == false)
                        return null;

            return assembly;
        }

        public static TAttribute[] GetAttributes<TAttribute>(this MemberInfo item) where TAttribute : Attribute
        {
            return (TAttribute[])Attribute.GetCustomAttributes(item, typeof(TAttribute), true);
        }

        public static Type[] GetAvailableTypes(this Assembly assembly, bool includeNonExported = false)
        {
            try
            {
                if (includeNonExported)
                    return assembly.GetTypes();

                return assembly.GetExportedTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.FindAll(t => t != null);

                // NOTE: perhaps we should not ignore the exceptions here, and log them?
            }
        }

        public static Type[] GetAvailableTypesOrdered(this Assembly assembly, bool includeNonExported = false)
        {
            return assembly.GetAvailableTypes(includeNonExported).OrderBy(t => t.FullName).ToArray();
        }

        /// <summary>
        ///     If the extended type is a Foo[] or IEnumerable{Foo} which is assignable from Foo[] this method will return
        ///     typeof(Foo)
        ///     otherwise <c>null</c>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetCompatibleArrayItemType(this Type type)
        {
            if (type == null)
                return null;
            if (type.IsArray)
                return type.GetElementType();
            if (type.IsGenericType == false || type.IsGenericTypeDefinition)
                return null;

            var openGeneric = type.GetGenericTypeDefinition();
            if (OpenGenericArrayInterfaces.Contains(openGeneric))
                return type.GetGenericArguments()[0];

            return null;
        }

        public static Assembly[] GetLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public static bool HasDefaultValue(this ParameterInfo item)
        {
            return (item.Attributes & ParameterAttributes.HasDefault) != 0;
        }

        public static object Instantiate(this ConstructorInfo ctor, object[] ctorArgs)
        {
            Func<object[], object> factory;

            factory = Factories.GetOrAdd(ctor, BuildFactory);

            return factory.Invoke(ctorArgs);
        }

        public static bool Is<TType>(this Type type)
        {
            return typeof(TType).IsAssignableFrom(type);
        }

        public static bool IsAssemblyFile(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            string extension;
            try
            {
                extension = Path.GetExtension(filePath);
            }
            catch (ArgumentException)
            {
                // path contains invalid characters...
                return false;
            }

            return IsDll(extension) || IsExe(extension);
        }

        private static void AddApplicationAssemblies(Assembly assembly, HashSet<Assembly> assemblies, string applicationName)
        {
            if (assemblies.Add(assembly) == false)
                return;

            foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
                if (IsApplicationAssembly(applicationName, referencedAssembly.FullName))
                    AddApplicationAssemblies(LoadAssembly(referencedAssembly), assemblies, applicationName);
        }

        private static Func<object[], object> BuildFactory(ConstructorInfo ctor)
        {
            var parameterInfos = ctor.GetParameters();
            var parameterExpressions = new Expression[parameterInfos.Length];
            var argument = Expression.Parameter(typeof(object[]), "parameters");
            for (var i = 0; i < parameterExpressions.Length; i++)
                parameterExpressions[i] = Expression.Convert(
                    Expression.ArrayIndex(argument, Expression.Constant(i, typeof(int))),
                    parameterInfos[i].ParameterType.IsByRef ? parameterInfos[i].ParameterType.GetElementType() : parameterInfos[i].ParameterType);

            return Expression.Lambda<Func<object[], object>>(
                Expression.New(ctor, parameterExpressions),
                argument).Compile();
        }

        private static void EnsureIsAssignable<TBase>(Type subtypeofTBase)
        {
            if (subtypeofTBase.Is<TBase>())
                return;

            string message;
            if (typeof(TBase).IsInterface)
                message = $"Type {subtypeofTBase.FullName} does not implement the interface {typeof(TBase).FullName}.";
            else
                message = $"Type {subtypeofTBase.FullName} does not inherit from {typeof(TBase).FullName}.";
            throw new InvalidCastException(message);
        }

        private static AssemblyName GetAssemblyName(string filePath)
        {
            AssemblyName assemblyName;
            try
            {
                assemblyName = AssemblyName.GetAssemblyName(filePath);
            }
            catch (ArgumentException)
            {
                assemblyName = new AssemblyName {CodeBase = filePath};
            }
            return assemblyName;
        }

        private static TBase Instantiate<TBase>(Type subtypeofTBase, object[] ctorArgs)
        {
            ctorArgs = ctorArgs ?? new object[0];
            var types = ctorArgs.ConvertAll(a => a == null ? typeof(object) : a.GetType());
            var constructor = subtypeofTBase.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, types, null);
            if (constructor != null)
                return (TBase)Instantiate(constructor, ctorArgs);

            try
            {
                return (TBase)Activator.CreateInstance(subtypeofTBase, ctorArgs);
            }
            catch (MissingMethodException ex)
            {
                string message;
                if (ctorArgs.Length == 0)
                    message = $"Type {subtypeofTBase.FullName} does not have a public default constructor and could not be instantiated.";
                else
                {
                    var messageBuilder = new StringBuilder();
                    messageBuilder.AppendLine(
                        $"Type {subtypeofTBase.FullName} does not have a public constructor matching arguments of the following types:");
                    foreach (var type in ctorArgs.Select(o => o.GetType()))
                        messageBuilder.AppendLine(type.FullName);

                    message = messageBuilder.ToString();
                }

                throw new ArgumentException(message, ex);
            }
            catch (Exception ex)
            {
                var message = $"Could not instantiate {subtypeofTBase.FullName}.";
                throw new Exception(message, ex);
            }
        }

        private static bool IsApplicationAssembly(string applicationName, string assemblyName)
        {
            return assemblyName.StartsWith(applicationName);
        }

        private static bool IsDll(string extension)
        {
            return ".dll".Equals(extension, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsExe(string extension)
        {
            return ".exe".Equals(extension, StringComparison.OrdinalIgnoreCase);
        }

        private static Assembly LoadAssembly(AssemblyName assemblyName)
        {
            return Assembly.Load(assemblyName);
        }
    }
}