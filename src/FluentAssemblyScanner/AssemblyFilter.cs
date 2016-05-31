using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

using FluentAssemblyScanner.Util;

namespace FluentAssemblyScanner
{
    public class AssemblyFilter : IAssemblyProvider
    {
        private readonly string directoryName;
        private readonly string mask;
        private Predicate<Assembly> assemblyFilter;
        private Predicate<AssemblyName> nameFilter;

        internal AssemblyFilter(string directoryName, string mask = null)
        {
            if (directoryName == null)
            {
                throw new ArgumentNullException(nameof(directoryName));
            }

            this.directoryName = GetFullPath(directoryName);
            this.mask = mask;
        }

        IEnumerable<Assembly> IAssemblyProvider.GetAssemblies()
        {
            foreach (var file in GetFiles())
            {
                if (!ReflectionUtil.IsAssemblyFile(file))
                {
                    continue;
                }

                var assembly = LoadAssemblyIgnoringErrors(file);
                if (assembly != null)
                {
                    yield return assembly;
                }
            }
        }

        public AssemblyFilter FilterByAssembly(Predicate<Assembly> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            assemblyFilter += filter;
            return this;
        }

        public AssemblyFilter FilterByName(Predicate<AssemblyName> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            nameFilter += filter;
            return this;
        }

        public AssemblyFilter WithKeyToken(string publicKeyToken)
        {
            return WithKeyToken(ExtractKeyToken(publicKeyToken));
        }

        public AssemblyFilter WithKeyToken(byte[] publicKeyToken)
        {
            if (publicKeyToken == null)
            {
                throw new ArgumentNullException(nameof(publicKeyToken));
            }

            return FilterByName(n => IsTokenEqual(n.GetPublicKeyToken(), publicKeyToken));
        }

        public AssemblyFilter WithKeyToken(Type typeFromAssemblySignedWithKey)
        {
            return WithKeyToken(typeFromAssemblySignedWithKey.Assembly);
        }

        public AssemblyFilter WithKeyToken<TTypeFromAssemblySignedWithKey>()
        {
            return WithKeyToken(typeof(TTypeFromAssemblySignedWithKey).Assembly);
        }

        public AssemblyFilter WithKeyToken(Assembly assembly)
        {
            return WithKeyToken(assembly.GetName().GetPublicKeyToken());
        }

        private byte[] ExtractKeyToken(string keyToken)
        {
            if (keyToken == null)
            {
                throw new ArgumentNullException(nameof(keyToken));
            }
            if (keyToken.Length != 16)
            {
                throw new ArgumentException(
                    $"The string '{keyToken}' does not appear to be a valid public key token. It should have 16 characters, has {keyToken.Length}.");
            }

            try
            {
                var tokenBytes = new byte[8];
                for (var i = 0; i < 8; i++)
                {
                    tokenBytes[i] = byte.Parse(keyToken.Substring(2 * i, 2), NumberStyles.HexNumber);
                }

                return tokenBytes;
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    $"The string '{keyToken}' does not appear to be a valid public key token. It could not be processed.",
                    e);
            }
        }

        private IEnumerable<string> GetFiles()
        {
            try
            {
                if (Directory.Exists(directoryName) == false)
                {
                    return Enumerable.Empty<string>();
                }
                if (string.IsNullOrEmpty(mask))
                {
                    return Directory.EnumerateFiles(directoryName);
                }

                return Directory.EnumerateFiles(directoryName, mask);
            }
            catch (IOException e)
            {
                throw new ArgumentException("Could not resolve assemblies.", e);
            }
        }

        private static string GetFullPath(string path)
        {
            if (Path.IsPathRooted(path) == false)
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            return Path.GetFullPath(path);
        }

        private static bool IsTokenEqual(byte[] actualToken, byte[] expectedToken)
        {
            if (actualToken == null)
            {
                return false;
            }
            if (actualToken.Length != expectedToken.Length)
            {
                return false;
            }

            for (var i = 0; i < actualToken.Length; i++)
            {
                if (actualToken[i] != expectedToken[i])
                {
                    return false;
                }
            }

            return true;
        }

        private Assembly LoadAssemblyIgnoringErrors(string file)
        {
            // based on MEF DirectoryCatalog
            try
            {
                return ReflectionUtil.GetAssemblyNamed(file, nameFilter, assemblyFilter);
            }
            catch (FileNotFoundException) {}
            catch (FileLoadException)
            {
                // File was found but could not be loaded
            }
            catch (BadImageFormatException)
            {
                // Dlls that contain native code or assemblies for wrong runtime (like .NET 4 asembly when we're in CLR2 process)
            }
            catch (ReflectionTypeLoadException)
            {
                // Dlls that have missing Managed dependencies are not loaded, but do not invalidate the Directory 
            }

            // TODO: log
            return null;
        }
    }
}