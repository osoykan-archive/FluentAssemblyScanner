# FluentAssemblyScanner

### Assembly scanner for .NET

##### How to ?

```C#
AssemblyScanner.FromAssemblyInDirectory(AssemblyFilterFactory.All())
                 .IncludeNonPublicTypes()
                 .BasedOn<JobModuleBase>()
                 .NonStatic()
                 .Scan();
```
