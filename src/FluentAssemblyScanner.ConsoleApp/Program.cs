using System;
using System.Collections.Generic;

using FluentAssemblyScanner.ConsoleApp.Animals;

namespace FluentAssemblyScanner.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IEnumerable<Type> types = FluentAssemblyScanner.FromAssemblyInDirectory(AssemblyFilterFactory.All())
                                                           .IncludeNonPublicTypes()
                                                           .BasedOn<IAnimal>()
                                                           .InSameNamespaceOf(typeof(IAnimal))
                                                           .HasAttribute<VoiceAttribute>()
                                                           .OrBasedOn<Human>()
                                                           .Filter()
                                                           .Classes()
                                                           .NonStatic()
                                                           .MethodName("Run")
                                                           .MethodNameContains("n")
                                                           .MethodHasAttribute<VoiceAttribute>()
                                                           .Scan();
        }
    }
}