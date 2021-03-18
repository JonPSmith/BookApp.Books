// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Xml.Schema;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BuildNuspecs.Helpers
{
    public class Settings
    {
        public enum AddSymbolsOptions { None, Debug, Release, Both}

        public string RootName { get; private set; }
        public string MainProject { get; private set; }
        public AddSymbolsOptions AddSymbols { get; private set; }
        public bool AutoPack { get; private set; }
        public string CopyNuGetTo { get; private set; }
        public LogLevel LogLevel { get; private set; }

        //Information needed by NuGet package
        public string NuGetId { get; private set; }
        public string Version { get; private set; }
        public string ReleaseNotes { get; private set; }
        public string Owners { get; private set; }
        public string Authors { get; private set; }
        public string Description { get; private set; }
        public string ProjectUrl { get; private set; }

        //This isn't filled in from appsettings
        public bool DebugMode { get; }
        public string DebugOrRelease { get; }
        //Only true if you should add symbols to the files
        public bool IncludeSymbols => AddSymbols == Settings.AddSymbolsOptions.Both ||
                                      AddSymbols.ToString() == DebugOrRelease;

        private static readonly string[] PropertiesToIgnore = new[]
        {
            nameof(LogLevel), nameof(AddSymbols), nameof(DebugMode), nameof(DebugOrRelease), nameof(IncludeSymbols)
        };

        public Settings(IConfiguration configuration, bool debugMode, string solutionDir)
        {
            var properties = GetType().GetProperties()
                .Where(x => !PropertiesToIgnore.Contains(x.Name));
            foreach (var propertyInfo in properties)
            {
                var value = configuration[propertyInfo.Name];
                if (propertyInfo.PropertyType == typeof(string))
                    propertyInfo.SetValue(this, value);
                else if (propertyInfo.PropertyType == typeof(bool))
                    propertyInfo.SetValue(this, bool.Parse(value));
                else
                    throw new Exception($"Setting property {propertyInfo.Name} cannot be converted");
            }

            if (string.IsNullOrEmpty(RootName))
                RootName = solutionDir.GetSolutionFilename();

            if (string.IsNullOrEmpty(MainProject))
                MainProject = $"{RootName}.AppSetup";
            else if (MainProject[0] == '.') 
                MainProject = $"{RootName}{MainProject}";

            if (string.IsNullOrEmpty(NuGetId))
                NuGetId = RootName;

            //setting that need a bit more work
            DebugMode = debugMode;
            DebugOrRelease = debugMode ? "Debug" : "Release";

            var logLevelString = configuration[nameof(LogLevel)];
            LogLevel = string.IsNullOrEmpty(logLevelString)
                ? LogLevel.Information
                : Enum.Parse<LogLevel>(logLevelString, true);

            var includeSymbolsString = configuration[nameof(AddSymbols)];
            AddSymbols = string.IsNullOrEmpty(includeSymbolsString)
                ? AddSymbolsOptions.None
                : Enum.Parse<AddSymbolsOptions>(includeSymbolsString, true);
        }

        public string NuGetFileName()
        {
            return $"{NuGetId}.{Version}";
        }
    }

}