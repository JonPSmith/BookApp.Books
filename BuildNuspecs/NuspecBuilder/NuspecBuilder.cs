// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

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

            package.files = appInfo.RootProjects.Select(x =>
            {
                var pathToDll =
                    $"{Path.GetDirectoryName(x.ProjectPath)}\\bin\\{settings.DebugOrRelease}\\{x.TargetFramework}\\{x.ProjectName}.dll";
                return new packageFile
                {
                    src = pathToDll,
                    target = $"lib\\{x.TargetFramework}"
                };
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