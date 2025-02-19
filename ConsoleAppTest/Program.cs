using CMOScenToolProject;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Newtonsoft.Json;

namespace ConsoleAppTest
{
    internal class Program
    {
        static void ScanCMOFolder(string searchDirectory, string outputDirectory)
        {
            Matcher matcher = new();
            matcher.AddInclude("**/*.scen");

            PatternMatchingResult result = matcher.Execute(
                new DirectoryInfoWrapper(
                    new DirectoryInfo(searchDirectory)));

            // Dictionary<string, string> pathToXML = new();
            foreach(var path in matcher.GetResultsInFullPath(searchDirectory))
            {
                Console.WriteLine(path);

                string content = File.ReadAllText(path);
                var scenContainer = CMOScenTool.LoadScenContainer(content);
                var xml = scenContainer.GetScenarioObject_AsXML();

                var relPath = System.IO.Path.GetRelativePath(searchDirectory, path);
                var outputPath = Path.ChangeExtension(Path.Join(outputDirectory, relPath), ".xml");
                var outputDirectDirectory = Path.GetDirectoryName(outputPath);
                if (!Path.Exists(outputDirectDirectory))
                {
                    Directory.CreateDirectory(outputDirectDirectory);
                }
                File.WriteAllText(outputPath, xml);

                // pathToXML[path] = xml;

                // break;
            }

            //var json = JsonConvert.SerializeObject(pathToXML);
            //File.WriteAllText(outputPath, json);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, CMO!");

            //ScanCMOFolder(
            //    @"D:\SteamLibrary\steamapps\common\Command - Modern Operations\Scenarios",
            //    "cmo_scenarios_xml"
            //);

            string content = File.ReadAllText("Middleware_local_test_remote_entity.scen");
            // string content = File.ReadAllText(@"D:\SteamLibrary\steamapps\common\Command - Modern Operations\Scenarios\Standalone Scenarios\Wooden Leg, 1985.scen");
            // string content = File.ReadAllText(@"D:\SteamLibrary\steamapps\common\Command - Modern Operations\Scenarios\Chains Of War\1. Blue Dawn.scen");
            Console.WriteLine(content);

            var scenContainer = CMOScenTool.LoadScenContainer(content);

            Console.WriteLine(scenContainer);
            var xml = scenContainer.GetScenarioObject_AsXML();
            Console.WriteLine(xml);
            Console.WriteLine(CMOScenTool.GetDecryptedDescription(xml));
        }
    }
}
