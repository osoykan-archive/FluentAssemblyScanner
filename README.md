# FluentAssemblyScanner

## This repository is not actively being maintained.

### Assembly and type scanner for .NET

[![Build status](https://ci.appveyor.com/api/projects/status/4ap8tbdpfivfeysc?svg=true)](https://ci.appveyor.com/project/osoykan/fluentassemblyscanner) [![NuGet version](https://badge.fury.io/nu/fluentassemblyscanner.svg)](https://badge.fury.io/nu/fluentassemblyscanner) [![Coverage Status](https://coveralls.io/repos/github/osoykan/FluentAssemblyScanner/badge.svg?branch=dev)](https://coveralls.io/github/osoykan/FluentAssemblyScanner?branch=dev)

### Examples

```c#
public static void Program()
{
  IEnumerable<Type> types = AssemblyScanner.FromAssemblyInDirectory(new AssemblyFilter("bin"))
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
