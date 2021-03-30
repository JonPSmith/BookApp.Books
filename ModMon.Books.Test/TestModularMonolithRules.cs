// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace ModMon.Books.Test
{
    public class TestModularMonolithRules
    {
        private const string ModuleName = "ModMon.Books.";

        private readonly string[] _layersPrefixInOrder = new[]
        {
            $"{ModuleName}ServiceLayer",
            $"{ModuleName}Infrastructure",
            $"{ModuleName}BizLogic",
            $"{ModuleName}Persistence",
            $"{ModuleName}Domain",
        };
        private static readonly List<Assembly> AllAppAssemblies = GetAppAssemblies(ModuleName).ToList();

        private readonly ITestOutputHelper _output;

        public TestModularMonolithRules(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestLowerLayersDoNotDependOnHigherLayers()
        {
            //SETUP
            var hasErrors = false;

            //ATTEMPT
            foreach (var namespacesPrefix in CheckNotAccessingOuterLayers())
            {
                _output.WriteLine($"Checking {namespacesPrefix.resideIn}.. does not rely on a {namespacesPrefix.notDependOn}");

                foreach (var assemblyToCheck in AllAppAssemblies.Where(x => x.GetName().Name.StartsWith(namespacesPrefix.resideIn)))
                {
                    var badLinks = assemblyToCheck.GetReferencedAssemblies()
                        .Where(x => x.Name.StartsWith(namespacesPrefix.resideIn)).ToList();
                    if (badLinks.Any())
                    {
                        hasErrors = true;
                        foreach (var assemblyName in badLinks)
                        {
                            _output.WriteLine($"Assembly {assemblyToCheck.GetName().Name} should not link to project {assemblyName.Name}");
                        }
                    }
                }

                //VERIFY
                Assert.False(hasErrors);
            }
        }

        [Fact]
        public void TestOnlyAccessesProjectsInSameNameSpaceOtherThanCommon()
        {
            var hasErrors = false;
            foreach (var namespacePrefix in _layersPrefixInOrder)
            {
                _output.WriteLine($"Check {namespacePrefix}.. for linking to project in same layer that hasn't got \"Common\" in its name");
                foreach (var assemblyToCheck in AllAppAssemblies.Where(x => x.GetName().Name.StartsWith(namespacePrefix)))
                {
                    var badLinks = assemblyToCheck.GetReferencedAssemblies()
                        .Where(x => x.Name.StartsWith(namespacePrefix) && !x.Name.Contains("Common")).ToList();
                    if (badLinks.Any())
                    {
                        hasErrors = true;
                        foreach (var assemblyName in badLinks)
                        {
                            _output.WriteLine($"Assembly {assemblyToCheck.GetName().Name} should not link to project {assemblyName.Name}");
                        }
                    }
                }

                //VERIFY
                Assert.False(hasErrors);
            }
        }

        private IEnumerable<(string resideIn, string notDependOn)> CheckNotAccessingOuterLayers()
        {
            for (int i = 1; i < _layersPrefixInOrder.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    yield return (_layersPrefixInOrder[i], _layersPrefixInOrder[j]);
                }
            }
        }

        private static IEnumerable<Assembly> GetAppAssemblies(string nameSpacePrefix)
        {
            //see https://stackoverflow.com/a/55672480/1434764
            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            foreach (var path in Directory.GetFiles(assemblyFolder, $"{nameSpacePrefix}*.dll"))
            {
                yield return Assembly.LoadFrom(path);
            }
        }
    }
}
