# FluentAssemblyScanner

### Assembly scanner for .NET

### Nuget Packages

```csharp
 // TODO
```

### Example

```csharp
public static void Program()
{
  IEnumerable<Type> types = AssemblyScanner.FromAssemblyInDirectory(AssemblyFilterFactory.All())
                                           .IncludeNonPublicTypes()
                                           .BasedOn<IAnyInterface>()
                                           .NonStatic()
                                           .Scan();
}
```
