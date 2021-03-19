// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
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
        private readonly ConsoleOutput _consoleOut;

        //fields filled in by BuildNuGet method
        private Settings _settings;
        private AppStructureInfo _appInfo;

        public MainCode(IConfiguration configuration)
        {
            _configuration = configuration;
            _consoleOut = new ConsoleOutput();
        }


        public void BuildNuGet(string[] args)
        {
            //args = new[] {"U"}; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            var thisProjPath = ProjectHelpers.GetExecutingAssemblyPath();
            var solutionDir = thisProjPath.GetSolutionPathFromProjectPath();

            _settings = new Settings(_configuration, solutionDir);
            args.DecodeArgsAndUpdateSettings(_consoleOut, _settings);
            _consoleOut.DefaultLogLevel = _settings.LogLevel;

            var rootName = solutionDir.GetSolutionFilename();
            _appInfo = solutionDir.ParseModularMonolithApp(rootName, _consoleOut);
            _consoleOut.LogMessage(_appInfo.ToString(), LogLevel.Information);
            _settings.BuildNuspecFile(_appInfo, _consoleOut);

            if (_settings.AutoPack)
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
            _consoleOut.LogMessage($"Running \"dotnet {startInfo.Arguments}\"", LogLevel.Information);
            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                _consoleOut.LogMessage("dotnet pack failed", LogLevel.Error);
            }
            _consoleOut.LogMessage("Finished dotnet pack...", LogLevel.Information);
            if (!string.IsNullOrEmpty(_settings.CopyNuGetTo))
            {
                var nuGetFromPath = thisProjPath.GetCorrectAssemblyPath(_settings.DebugOrRelease, null) +
                                    _settings.NuGetFileName() + ".nupkg";
                var nuGetToPath = Path.Combine( _settings.CopyNuGetTo , _settings.NuGetFileName() + ".nupkg");
                var fileIsOverwritten = File.Exists(nuGetToPath);

                File.Copy(nuGetFromPath, nuGetToPath, true);
                _consoleOut.LogMessage($"Copied created NuGet package to {_settings.CopyNuGetTo}", LogLevel.Information);
                if (fileIsOverwritten && !_settings.OverwriteCachedVersion)
                    _consoleOut.LogMessage("Copy overwrites existing NuGet package. If package is already installed you can't update it.", LogLevel.Warning);
            }

            if (_settings.OverwriteCachedVersion)
            {
                //Replace the over all the dlls to the 

                var pathToNuGetFolderInCache = Path.Combine(_settings.NuGetCachePath, _settings.NuGetId.ToLower(), _settings.Version);
                //Check that the NuGet is there 
                if (!Directory.Exists(pathToNuGetFolderInCache))
                    _consoleOut.LogMessage("Could not update NuGet as not in the cache. Have you added it yet?.", LogLevel.Warning);
                else
                {
                    foreach (var projectInfo in _appInfo.AllProjects)
                    {
                        var dllFilename = projectInfo.ProjectName + ".dll";
                        var pathFromDir = Path.GetDirectoryName(projectInfo.ProjectPath)
                            .GetCorrectAssemblyPath(_settings.DebugOrRelease, projectInfo.TargetFramework);
                        var pathToDir = Path.Combine(pathToNuGetFolderInCache, "lib", projectInfo.TargetFramework);
                        File.Copy(Path.Combine(pathFromDir, dllFilename), 
                            Path.Combine(pathToDir, dllFilename), true);
                        _consoleOut.LogMessage($"Updated {dllFilename} in nuget cache.", LogLevel.Debug);

                        var xmlFilename = projectInfo.ProjectName + ".xml";
                        if (File.Exists(Path.Combine(pathFromDir, xmlFilename)))
                        {
                            File.Copy(Path.Combine(pathFromDir, xmlFilename),
                                Path.Combine(pathToDir, xmlFilename), true);
                            _consoleOut.LogMessage($"Updated {xmlFilename} in nuget cache.", LogLevel.Debug);
                        }
                    }
                    _consoleOut.LogMessage("Have updated .dll files in NugGet cache. Use Rebuild Solution to update.", LogLevel.Information);
                }
            }
        }

        private string FormPackCommand(string thisProjPath)
        {
            var command = _settings.DebugMode
                ? $"pack -p:NuspecFile=CreateNuGetDebug.nuspec"
                : $"pack -c Release -p:NuspecFile=CreateNuGetRelease.nuspec";

            command += " --no-build";
            if (_settings.IncludeSymbols)
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

            var message = debugMode
                ? "Building a NuGet package using Debug code"
                : "Building a NuGet package using Release code";

            consoleOut.LogMessage(message, LogLevel.Information);
            return debugMode;
        }
    }
}