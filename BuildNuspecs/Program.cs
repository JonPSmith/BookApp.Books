using System;

namespace BuildNuspecs
{
    //Once you have build the NuSpecs you run then with the following commands 
    // DEBUG:   dotnet pack -p:NuspecFile=CreateNuGetDebug.nuspec
    // RELEASE: dotnet pack -c Release -p:NuspecFile=CreateNuGetRelease.nuspec

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
