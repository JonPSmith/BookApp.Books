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

            var diffFrameworks = appInfo.AllProjects.Select(x => x.TargetFramework).Distinct();
            if (diffFrameworks.Count() > 1)
                consoleOut.LogMessage($"The projects use multiple frameworks ({(string.Join(", ", diffFrameworks))}.\n" +
                                      $"That usually has problems unless the project that uses this uses multiple frameworks too.", LogLevel.Warning);

            package.files = appInfo.AllProjects.SelectMany(x =>
            {

                var pathToDir = Path.GetDirectoryName(x.ProjectPath)
                    .GetCorrectAssemblyPath(settings.DebugOrRelease, x.TargetFramework);

                var dllPath = pathToDir + $"{x.ProjectName}.dll";

                if (!File.Exists(dllPath))
                    consoleOut.LogMessage($"The project {x.ProjectName} doesn't have a .dll file", LogLevel.Error);

                var result = new List<packageFile>
                {
                    new packageFile
                    {
                        src = dllPath,
                        target = $"lib\\{x.TargetFramework}"
                    }
                };
                consoleOut.LogMessage($"Added {x.ProjectName}.dll file to NuGet files", LogLevel.Debug);
                var xmlPath = pathToDir + $"{x.ProjectName}.xml";
                if (File.Exists(xmlPath))
                {
                    result.Add(new packageFile
                    {
                        src = xmlPath,
                        target = $"lib\\{x.TargetFramework}"
                    });
                    consoleOut.LogMessage($"Added {x.ProjectName}.xml file to NuGet files", LogLevel.Debug);
                }

                if (settings.IncludeSymbols)
                {
                    var pdbPath = pathToDir + $"{x.ProjectName}.pdb";
                    if (!File.Exists(pdbPath))
                        consoleOut.LogMessage($"You asked for symbols by {x.ProjectName} doesn't have a .pdb file", LogLevel.Warning);
                    else
                    {
                        result.Add(new packageFile
                        {
                            src = pathToDir + $"{x.ProjectName}.pdb",
                            target = $"lib\\{x.TargetFramework}"
                        });
                    }
                    consoleOut.LogMessage($"The project {x.ProjectName} symbols (.pdb) file", LogLevel.Debug);
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