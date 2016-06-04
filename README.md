# FluentAssemblyScanner

### Assembly and type scanner for .NET

[![Build status](https://ci.appveyor.com/api/projects/status/4ap8tbdpfivfeysc?svg=true)](https://ci.appveyor.com/project/osoykan/fluentassemblyscanner) [![NuGet version](https://badge.fury.io/nu/fluentassemblyscanner.svg)](https://badge.fury.io/nu/fluentassemblyscanner)
### Nuget Packages

```csharp
 // TODO Nuget
 
```

### Examples

```csharp
public static void Program()
{
  // Based on Interface
  IEnumerable<Type> types = AssemblyScanner.FromAssemblyInDirectory(AssemblyFilterFactory.All())
                                           .IncludeNonPublicTypes()
                                           .BasedOn<IAnyInterface>()
                                           .NonStatic()
                                           .Scan();
  // With method name                                         
  IEnumerable<Type> types = AssemblyScanner.FromAssemblyInDirectory(AssemblyFilterFactory.All())
                                           .IncludeNonPublicTypes()
                                           .BasedOn<IDummy>()
                                           .NonStatic()
                                           .UseDefaultfilter()
                                           .WithMethodNamed("DummyMethod")
                                           .Scan();
}
```
