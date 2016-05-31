using FluentAssemblyScanner.ConsoleApp.Dummy;

namespace FluentAssemblyScanner.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var assemblyDescriptor = Classes
                .FromAssemblyInDirectory(new AssemblyFilter(string.Empty))
                .IncludeNonPublicTypes()
                .BasedOn<IDummy>()
                .Scan();
        }
    }
}