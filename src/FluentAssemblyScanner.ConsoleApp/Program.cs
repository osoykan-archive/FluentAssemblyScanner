using FluentAssemblyScanner.ConsoleApp.Dummy;

namespace FluentAssemblyScanner.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var assemblyDescriptor = AssemblyScanner
                .FromAssemblyInDirectory(new AssemblyFilter(string.Empty))
                .BasedOn<IDummy>()
                .IncludeNonPublicTypes()
                .Scan();
        }
    }
}