using FluentAssemblyScanner.ConsoleApp.Animals;

namespace FluentAssemblyScanner.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var types = AssemblyScanner.FromAssemblyInDirectory(AssemblyFilterFactory.All())
                                       .IncludeNonPublicTypes()
                                       .BasedOn<IAnimal>()
                                       .InNamespace("FluentAssemblyScanner.ConsoleApp.Animals")
                                       .Filter()
                                       .Then()
                                       .Scan();
        }
    }
}