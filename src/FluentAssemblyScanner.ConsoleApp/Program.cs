using FluentAssemblyScanner.ConsoleApp.Animals;

namespace FluentAssemblyScanner.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var assemblyDescriptor = AssemblyScanner
                .FromAssemblyInDirectory(AssemblyFilterFactory.All())
                .IncludeNonPublicTypes()
                .BasedOn<IAnimal>()
                .NonStatic()
                .UseDefaultfilter()
                .WithMethodNamed("VoiceType")
                .Scan();
        }
    }
}