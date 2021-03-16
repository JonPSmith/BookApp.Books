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
   - Create optional unique suffix to version (control from json), e.g.   
`DateTime.UtcNow.ToString("-yy-mm-ddThh-mm-ssZ")`
   - second param for notes
   - Call `dotnet pack` from app - see [link](https://stackoverflow.com/a/63341926/1434764) 
   - Copy NuGet package (and symbols) to given dir
- Safety checks
   - Check dll exists
   - Check Debug/Release are up to date (against other?)



### ModMon.Books.Persistence

- Add code to AppStart to 
   - Register services
   - Setup database on start
   - Provide assemblies for GenericServices
- Add DesignTimeContext + Migrate db