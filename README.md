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
  AssemblyScanner.FromAssemblyInDirectory(AssemblyFilterFactory.All())
                 .IncludeNonPublicTypes()
                 .BasedOn<IAnyInterface>()
                 .NonStatic()
                 .Scan();
}
```
