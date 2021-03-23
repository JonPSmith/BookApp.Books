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

        public string RootName { get; set; }
        public AddSymbolsOptions AddSymbols { get; set; }
        public bool AutoPack { get; set; }
        public string CopyNuGetTo { get; set; }
        public LogLevel LogLevel { get; set; }
        public string NuGetCachePath { get; set; }

        //Information needed by NuGet package
        public string NuGetId { get; set; }
        public string Version { get; set; }
        public string ReleaseNotes { get; set; }
        public string Owners { get; set; }
        public string Authors { get; set; }
        public string Description { get; set; }
        public string ProjectUrl { get; set; }

        //This isn't filled in from appsettings
        public bool DebugMode { get; set;  }
        public bool OverwriteCachedVersion { get; set; }

        public string DebugOrRelease => DebugMode ? "Debug" : "Release";

        //Only true if you should add symbols to the files
        public bool IncludeSymbols => AddSymbols == Settings.AddSymbolsOptions.Both ||
                                      AddSymbols.ToString() == DebugOrRelease;

        private static readonly string[] PropertiesToIgnore = new[]
        {
            nameof(LogLevel), nameof(AddSymbols), //Let out because have to be done by hand
            nameof(DebugOrRelease), nameof(IncludeSymbols), //lambda properties
            nameof(DebugMode), nameof(OverwriteCachedVersion) //set from arguments
        };

        public Settings(IConfiguration configuration, ConsoleOutput consoleOutput, string solutionDir)
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
                RootName = solutionDir.GetSolutionFilename(consoleOutput);

            if (string.IsNullOrEmpty(NuGetId))
                NuGetId = RootName;

            if (string.IsNullOrEmpty(NuGetCachePath))
                NuGetCachePath = configuration["OS"].Contains("Windows")
                    ? $"{configuration["USERPROFILE"]}\\.nuget\\packages"
                    : "~/.nuget/packages";

            //setting that need a bit more work

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