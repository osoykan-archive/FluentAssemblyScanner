# FluentAssemblyScanner

### Assembly and type scanner for .NET

[![Build status](https://ci.appveyor.com/api/projects/status/4ap8tbdpfivfeysc?svg=true)](https://ci.appveyor.com/project/osoykan/fluentassemblyscanner) [![NuGet version](https://badge.fury.io/nu/fluentassemblyscanner.svg)](https://badge.fury.io/nu/fluentassemblyscanner)

### Examples

```csharp
public static void Program()
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
```
