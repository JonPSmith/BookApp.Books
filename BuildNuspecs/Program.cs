using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BuildNuspecs.Helpers;
using BuildNuspecs.NuspecBuilder;
using BuildNuspecs.ParseProjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BuildNuspecs
{
    //Once you have build the NuSpecs you run then with the following commands 
    // DEBUG:   dotnet pack -p:NuspecFile=CreateNuGetDebug.nuspec
    // RELEASE: dotnet pack -c Release -p:NuspecFile=CreateNuGetRelease.nuspec

    class Program
    {
        private static IConfigurationRoot _configurationRoot;

        static void Main(string[] args)
        {
            //see https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration#configure-console-apps
            using var host = CreateHostBuilder(args).Build();

            var thisProjPath = ProjectHelpers.GetExecutingAssemblyPath();
            var solutionDir = thisProjPath.GetSolutionPathFromProjectPath();

            var debugMode = CalcDebugMode(args);

            var settings = new Settings(_configurationRoot, debugMode, solutionDir);

            var rootName = solutionDir.GetSolutionFilename();
            var appStructure = solutionDir.ParseModularMonolithApp(rootName);
            settings.BuildNuspecFile(appStructure);
        }
        //see https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#json-configuration-provider
        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //thanks to https://wakeupandcode.com/generic-host-builder-in-asp-net-core-3-1/
                .ConfigureHostConfiguration(configuration =>
                {
                    configuration
                        .AddJsonFile("hostsettings.json", optional: true);

                    _configurationRoot = configuration.Build();
                });

        private static bool CalcDebugMode(string[] args)
        {
            var debugMode = false;
#if DEBUG
            debugMode = true;
#endif
            //If there is a argument, then this overrides
            if (args.Length > 0)
            {
                if (args.Length != 1 || !(args[0] == "R" || args[0] == "D"))
                    throw new Exception("If you provide an argument it must be R (for Release) or D (for Debug)");
                debugMode = args[0] == "D";
            }
            return debugMode;
        }

    }
}
