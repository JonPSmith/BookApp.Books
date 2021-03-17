// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using BuildNuspecs.Helpers;
using BuildNuspecs.ParseProjects;

namespace BuildNuspecs.NuspecBuilder
{
    public static class NuspecBuilder
    {
        public static void BuildNuspecFile(this Settings settings, AppStructureInfo appInfo)
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

            package.metadata.dependencies = new packageMetadataDependencies
            {
                group = new packageMetadataDependenciesGroup
                {
                    targetFramework = appInfo.RootProjects.First().TargetFramework,
                    dependency = appInfo.AllNuGetInfosDistinct.Select(x =>
                        new packageMetadataDependenciesGroupDependency
                        {
                            id = x.NuGetId,
                            version = x.Version
                        }).ToArray()
                }
            };

            package.files = appInfo.AllProjects.SelectMany(x =>
            {
                var pathToDir =
                    $"{Path.GetDirectoryName(x.ProjectPath)}\\bin\\{settings.DebugOrRelease}\\{x.TargetFramework}\\";
                var result = new List<packageFile>
                {
                    new packageFile
                    {
                        src = pathToDir + $"{x.ProjectName}.dll",
                        target = $"lib\\{x.TargetFramework}"
                    }
                };
                if (settings.IncludeSymbols)
                {
                    result.Add(new packageFile
                    {
                        src = pathToDir + $"{x.ProjectName}.pdb",
                        target = $"lib\\{x.TargetFramework}"
                    });
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
        }


    }
}