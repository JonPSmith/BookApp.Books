# Release notes

## TODO

### BuildNuspecs

- Add features
  - Better args used: e.g. -v:1.0.0-preview001
- Safety checks
  - Check Debug/Release are up to date (against other?)

- Turn dotnet tool NuGet package (see [this link](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools-how-to-create) to do this
  - (watch out for GetExecutingAssembly!)


#### Best way to provide debug information 

It seems that embedding the source in the DLL is the best way.

#### Exit application

Call `Environment.Exit(0)`

