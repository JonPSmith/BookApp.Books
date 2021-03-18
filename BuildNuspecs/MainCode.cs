// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using BuildNuspecs.Helpers;
using BuildNuspecs.NuspecBuilder;
using BuildNuspecs.ParseProjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BuildNuspecs
{
    public class MainCode
    {
        private readonly IConfiguration _configuration;
        private readonly ConsoleOutput consoleOut;
        private Settings settings;

        public MainCode(IConfiguration configuration)
        {
            _configuration = configuration;
            consoleOut = new ConsoleOutput();
        }


        public void BuildNuGet(string[] args)
        {
            var thisProjPath = ProjectHelpers.GetExecutingAssemblyPath();
            var solutionDir = thisProjPath.GetSolutionPathFromProjectPath();

            var debugMode = CalcDebugMode(consoleOut, args);
            settings = new Settings(_configuration, debugMode, solutionDir);
            consoleOut.DefaultLogLevel = settings.LogLevel;

            var rootName = solutionDir.GetSolutionFilename();
            var appStructure = solutionDir.ParseModularMonolithApp(rootName, consoleOut);
            consoleOut.LogMessage(appStructure.ToString(), LogLevel.Information);
            settings.BuildNuspecFile(appStructure, consoleOut);

            if (settings.AutoPack)
            {
                RunPackEct(thisProjPath);
            }
        }

        //Once you have build the NuSpecs you run then with the following commands 
        // DEBUG:   dotnet pack -p:NuspecFile=CreateNuGetDebug.nuspec
        // RELEASE: dotnet pack -c Release -p:NuspecFile=CreateNuGetRelease.nuspec
        //
        // With new symbols: dotnet pack -p:NuspecFile=CreateNuGetDebug.nuspec --include-symbols

        private const string VersionPrefix = "Version=v";

        private void RunPackEct(string thisProjPath)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet.exe",
                Arguments = FormPackCommand(thisProjPath),
                WorkingDirectory = thisProjPath
            };
            process.StartInfo = startInfo;
            consoleOut.LogMessage($"Running \"dotnet {startInfo.Arguments}", LogLevel.Information);
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                consoleOut.LogMessage("dotnet pack failed", LogLevel.Error);
            }
            consoleOut.LogMessage("Finished dotnet pack...", LogLevel.Information);
            if (!string.IsNullOrEmpty(settings.CopyNuGetTo))
            {
                var nuGetFromPath = thisProjPath.GetCorrectAssemblyPath(settings.DebugOrRelease, null) +
                                    settings.NuGetFileName() + ".nupkg";
                var nuGetToPath = settings.CopyNuGetTo + "\\" + settings.NuGetFileName() + ".nupkg";
                File.Copy(nuGetFromPath, nuGetToPath, true);

                consoleOut.LogMessage($"Copied created NuGet package to {settings.CopyNuGetTo}", LogLevel.Information);
            }
        }

        private string FormPackCommand(string thisProjPath)
        {
            var command = settings.DebugMode
                ? $"pack -p:NuspecFile=CreateNuGetDebug.nuspec"
                : $"pack -c Release -p:NuspecFile=CreateNuGetRelease.nuspec";

            command += " --no-build";
            if (settings.IncludeSymbols)
                command += " --include-symbols";

            return command;
        }

        private static bool CalcDebugMode(ConsoleOutput consoleOut, string[] args)
        {
            var debugMode = false;
#if DEBUG
            debugMode = true;
#endif
            //If there is a argument, then this overrides
            if (args.Length > 0)
            {
                if (args.Length != 1 || !(args[0] == "R" || args[0] == "D"))
                    consoleOut.LogMessage("If you provide an argument it must be R (for Release) or D (for Debug)", LogLevel.Error);
                debugMode = args[0] == "D";
            }
            return debugMode;
        }
    }
}