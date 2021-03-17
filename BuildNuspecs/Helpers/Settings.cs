// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Xml.Schema;
using Microsoft.Extensions.Configuration;

namespace BuildNuspecs.Helpers
{
    public class Settings
    {
        private const string JsonIncludeSymbolsName = "IncludeSymbols";
        public enum IncludeSymbolsOptions { None, Debug, Release, Both}

        public string RootName { get; private set; }
        public string MainProject { get; private set; }
        public string NuGetId { get; private set; }
        public IncludeSymbolsOptions IncludeSymbolsWhen { get; private set; }
        public string Version { get; private set; }
        public string ReleaseNotes { get; private set; }
        public string Owners { get; private set; }
        public string Authors { get; private set; }
        public string Description { get; private set; }
        public string ProjectUrl { get; private set; }

        //This isn't filled in from appsettings
        public string DebugOrRelease { get; private set; }
        //Only true if you should add symbols to the files
        public bool IncludeSymbols => IncludeSymbolsWhen == Settings.IncludeSymbolsOptions.Both ||
                                      IncludeSymbolsWhen.ToString() == DebugOrRelease;

        private static readonly string[] PropertiesToIgnore = new[]
        {
            nameof(IncludeSymbolsWhen), nameof(DebugOrRelease), nameof(IncludeSymbols)
        };

        public Settings(IConfiguration configuration, bool debugMode, string solutionDir)
        {
            var properties = GetType().GetProperties()
                .Where(x => !PropertiesToIgnore.Contains(x.Name));
            foreach (var propertyInfo in properties)
            {
                propertyInfo.SetValue(this, configuration[propertyInfo.Name]);
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
            DebugOrRelease = debugMode ? "Debug" : "Release";

            var includeSymbolsString = configuration[JsonIncludeSymbolsName];
            IncludeSymbolsWhen = string.IsNullOrEmpty(includeSymbolsString)
                ? IncludeSymbolsOptions.None
                : Enum.Parse<IncludeSymbolsOptions>(includeSymbolsString, true);
        }
    }

}