using System;

namespace FluentAssemblyScanner.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var assemblyDescriptor = AssemblyScanner
                .FromAssemblyMatchingNamed("Fluent", AssemblyFilterFactory.All())
                .IncludeNonPublicTypes()
                .PickAny()
                .NonStatic()
                .HasAttribute<SerializableAttribute>()
                .Scan();
        }
    }
}