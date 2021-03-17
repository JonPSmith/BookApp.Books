# Release notes

## TODO

### BuildNuspecs

- Learn
  - Try building a simple project with symbols
  - How to use symbols for debugging
  - Can we replace a existing NuGet without changing the version?
- Add symbols/debugging 
  - work out how to debug NuGet
- Add features
  - Add xml if available
  - Add console output to say what is happening
  - Create optional unique suffix to version (control from json), e.g.  
`DateTime.UtcNow.ToString("-yy-mm-ddThh-mm-ssZ")`
  - second param for notes
  - Call `dotnet pack` from app - see [link](https://stackoverflow.com/a/63341926/1434764) 
  - Copy NuGet package (and symbols) to given dir
- Safety checks
  - Check dll exists
  - Check Debug/Release are up to date (against other?)
  - Check xml?
  - Check project for embedded 

### Best way to provide debug information 

It seems that embedding the source in the DLL is the best way.

### Can we replace a existing NuGet without changing the version?

You can, but you have to update all the dlls in the C:\Users\JonPSmith\.nuget\packages\trynugetmm\1.0.0-preview011\lib\netstandard2.1 folder. 

*See [this link](https://stackoverflow.com/questions/40902578/wheres-the-nuget-package-location-in-asp-net-core) for the name directory.*
