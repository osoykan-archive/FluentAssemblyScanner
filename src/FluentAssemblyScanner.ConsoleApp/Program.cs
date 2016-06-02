using FluentAssemblyScanner.ConsoleApp.Dummy;

namespace FluentAssemblyScanner.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var assemblyDescriptor = AssemblyScanner
                .FromAssemblyInDirectory(AssemblyFilterFactory.All())
                .IncludeNonPublicTypes()
                .BasedOn<IDummy>()
                .NonStatic()
                .UseDefaultfilter()
                .WithMethodNamed("DummyMethod")
                .Scan();
        }
    }
}