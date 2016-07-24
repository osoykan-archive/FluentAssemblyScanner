using FluentAssemblyScanner.ConsoleApp.Animals;

namespace FluentAssemblyScanner.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var types = FluentAssemblyScanner.FromAssemblyInDirectory(AssemblyFilterFactory.All())
                                             .IncludeNonPublicTypes()
                                             .BasedOn<IAnimal>()
                                             .Filter()
                                             .Classes()
                                             .MethodHasAttribute<VoiceAttribute>()
                                             .Scan();
        }
    }
}