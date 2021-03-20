# Release notes

## TODO

### BuildNuspecs

- Add features
  - Better args used: e.g. -v:1.0.0-preview001
- Safety checks
  - Check Debug/Release are up to date (against other?)
  - Check project for embedded which matches IncludeSymbols setting??

- Turn dotnet tool NuGet package (see [this link](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools-how-to-create) to do this
  - (watch out for GetExecutingAssembly!)


#### Best way to provide debug information 

It seems that embedding the source in the DLL is the best way.

#### Exit application

Call `Environment.Exit(0)`

#### Can we replace a existing NuGet without changing the version?

You can, but you have to update all the dlls in the C:\Users\JonPSmith\.nuget\packages\trynugetmm\1.0.0-preview011\lib\netstandard2.1 folder. 

*See [this link](https://stackoverflow.com/questions/40902578/wheres-the-nuget-package-location-in-asp-net-core) for the name directory.*
