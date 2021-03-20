// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using BuildNuspecs.Helpers;
using BuildNuspecs.ParseProjects;
using Microsoft.Extensions.Logging;

namespace BuildNuspecs.NuspecBuilder
{
    public static class NuspecBuilder
    {
        public static void BuildNuspecFile(this Settings settings, AppStructureInfo appInfo, ConsoleOutput consoleOut)
        {
            var package = new package
            {
                metadata = new packageMetadata
                {
                    id = settings.RootName,
                    version = settings.Version,
                    releaseNotes = settings.ReleaseNotes,
                    owners = settings.Owners,
                    authors = settings.Authors,
                    description = settings.Description,
                    projectUrl = settings.ProjectUrl
                }
            };

            package.metadata.dependencies = appInfo.NuGetInfosDistinctByFramework.Keys.Select(key =>
                new packageMetadataGroup
                {
                    targetFramework = key,
                    dependency = appInfo.NuGetInfosDistinctByFramework[key].Select(nuGetInfo =>
                        new packageMetadataGroupDependency
                        {
                            id = nuGetInfo.NuGetId,
                            version = nuGetInfo.Version
                        }).ToArray()
                }).ToArray();

            package.files = appInfo.AllProjects.SelectMany(x =>
            {
                var pathToDir = Path.GetDirectoryName(x.ProjectPath)
                    .GetCorrectAssemblyPath(settings.DebugOrRelease, x.TargetFramework);
                var projectFileTypes = Directory.GetFiles(pathToDir, $"{x.ProjectName}.*")
                    .ToDictionary(x => Path.GetExtension(x));

                if (!projectFileTypes.ContainsKey(".dll"))
                    consoleOut.LogMessage($"The project {x.ProjectName} doesn't have a .dll file", LogLevel.Error);
                consoleOut.LogMessage($"The project {x.ProjectName} doesn't have a symbols (.pdb) file", LogLevel.Debug);

                consoleOut.LogMessage($"Added {x.ProjectName}.dll file to NuGet files", LogLevel.Debug);
                var result = new List<packageFile>
                {
                    new packageFile
                    {
                        src = pathToDir + $"{x.ProjectName}.dll",
                        target = $"lib\\{x.TargetFramework}"
                    }
                };
                if (projectFileTypes.ContainsKey(".xml"))
                {
                    consoleOut.LogMessage($"Added {x.ProjectName}.xml file to NuGet files", LogLevel.Debug);
                    result.Add(new packageFile
                    {
                        src = pathToDir + $"{x.ProjectName}.xml",
                        target = $"lib\\{x.TargetFramework}"
                    });
                }
                if (settings.IncludeSymbols)
                {
                    if (!projectFileTypes.ContainsKey(".dll"))
                        consoleOut.LogMessage($"You asked for symbols by {x.ProjectName} doesn't have a symbols (.pdb) file", LogLevel.Warning);
                    else
                    {
                        consoleOut.LogMessage($"Added {x.ProjectName}.pdb file to NuGet files", LogLevel.Debug);
                        result.Add(new packageFile
                        {
                            src = pathToDir + $"{x.ProjectName}.pdb",
                            target = $"lib\\{x.TargetFramework}"
                        });
                    }
                }
                return result;

            }).ToArray();


            //Create/update Nuspec file
            var filename = $"CreateNuGet{settings.DebugOrRelease}.nuspec";
            var dir = ProjectHelpers.GetExecutingAssemblyPath();

            //see https://www.jonasjohn.de/snippets/csharp/xmlserializer-example.htm
            XmlSerializer serializerObj = new XmlSerializer(typeof(package));

            // Create a new file stream to write the serialized object to a file
            TextWriter writeFileStream = new StreamWriter(Path.Combine(dir, filename));
            serializerObj.Serialize(writeFileStream, package);
            writeFileStream.Close();

            consoleOut.LogMessage($"Updated {settings.DebugOrRelease} Nuspec file: NuGetId: '{settings.RootName}', Version: {settings.Version} containing {appInfo.AllProjects.Count} projects.", LogLevel.Information);
        }


    }
}