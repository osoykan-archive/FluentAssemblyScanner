# FluentAssemblyScanner

### Assembly and type scanner for .NET

[![Build status](https://ci.appveyor.com/api/projects/status/4ap8tbdpfivfeysc?svg=true)](https://ci.appveyor.com/project/osoykan/fluentassemblyscanner) [![NuGet version](https://badge.fury.io/nu/fluentassemblyscanner.svg)](https://badge.fury.io/nu/fluentassemblyscanner)
### Nuget Packages

### Examples

```csharp
public static void Program()
{
  IEnumerable<Type> types = AssemblyScanner.FromAssemblyInDirectory(AssemblyFilterFactory.All())
                                           .IncludeNonPublicTypes()
                                           .BasedOn<IAnimal>()
                                           .Filter()
                                           .And.MethodHasAttribute<VoiceAttribute>()
                                           .And.MethodName("Sleep")
                                           .And.Classes()
                                           .And.NonStatic()
                                           .Then()
                                           .Scan();
}
```
