# Release notes

## TODO

### BuildNuspecs

- Add features
  - Call `dotnet pack` from app - see [link](https://stackoverflow.com/a/63341926/1434764) 
  - Copy NuGet package (and symbols) to given dir
  - Z mode - replaces the DLLs in the given named NuGet cache
  - second param for notes

- Safety checks
  - Check Debug/Release are up to date (against other?)
  - Check project for embedded which matches IncludeSymbols setting

### Best way to provide debug information 

It seems that embedding the source in the DLL is the best way.

### Can we replace a existing NuGet without changing the version?

You can, but you have to update all the dlls in the C:\Users\JonPSmith\.nuget\packages\trynugetmm\1.0.0-preview011\lib\netstandard2.1 folder. 

*See [this link](https://stackoverflow.com/questions/40902578/wheres-the-nuget-package-location-in-asp-net-core) for the name directory.*
