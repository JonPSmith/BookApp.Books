// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BuildNuspecs.Helpers
{
    public class Settings
    {
        public string RootName { get; }
        public string MainProject { get; }
        public string NuGetId { get; }
        public string Version { get; }
        public string ReleaseNotes { get; }
        public string Owners { get; }
        public string Authors { get; }
        public string Description { get; }
        public string ProjectUrl { get; }

        //This isn't filled in from appsettings
        public string DebugOrRelease { get; }

        public Settings(IConfiguration configuration, bool debugMode, string solutionDir)
        {
            RootName = configuration[nameof(RootName)];
            MainProject = configuration[nameof(MainProject)];
            NuGetId = configuration[nameof(NuGetId)];
            Version = configuration[nameof(Version)];
            ReleaseNotes = configuration[nameof(ReleaseNotes)];
            Owners = configuration[nameof(Owners)];
            Authors = configuration[nameof(Authors)];
            Description = configuration[nameof(Description)];
            ProjectUrl = configuration[nameof(ProjectUrl)];

            configuration.Bind(this);
            if (string.IsNullOrEmpty(RootName))
                RootName = solutionDir.GetSolutionFilename();

            if (string.IsNullOrEmpty(MainProject))
                MainProject = $"{RootName}.AppSetup";
            else if (MainProject[0] == '.') 
                MainProject = $"{RootName}{MainProject}";

            if (string.IsNullOrEmpty(NuGetId))
                NuGetId = RootName;

            DebugOrRelease = debugMode ? "Debug" : "Release";
        }
    }

}