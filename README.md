# FluentAssemblyScanner

### Assembly scanner for .NET

##### How to ?

```csharp
AssemblyScanner.FromAssemblyInDirectory(AssemblyFilterFactory.All())
                 .IncludeNonPublicTypes()
                 .BasedOn<JobModuleBase>()
                 .NonStatic()
                 .Scan();
```
