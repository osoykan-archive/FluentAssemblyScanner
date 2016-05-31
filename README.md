# FluentAssemblyScanner

### Assembly scanner for .NET

##### How to ?

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
