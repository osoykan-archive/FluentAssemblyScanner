using FluentAssemblyScanner.ConsoleApp.Dummy;

namespace FluentAssemblyScanner.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var assemblyDescriptor = AssemblyScanner
                .FromAssemblyInDirectory(new AssemblyFilter(string.Empty))
                .IncludeNonPublicTypes()
                .ExcludeAssemblyContaining<IDummy>()
                .BasedOn<IDummy>()
                .Scan();
        }
    }
}