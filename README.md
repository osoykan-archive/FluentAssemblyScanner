# FluentAssemblyScanner

### Assembly scanner for .NET

##### How to ?

```csharp
private static void Program()
{
  AssemblyScanner.FromAssemblyInDirectory(AssemblyFilterFactory.All())
                 .IncludeNonPublicTypes()
                 .BasedOn<JobModuleBase>()
                 .NonStatic()
                 .Scan();
}

 
```
