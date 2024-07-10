using CMOScenToolProject;

namespace ConsoleAppTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            string content = File.ReadAllText("Middleware_local_test_remote_entity.scen");
            Console.WriteLine(content);

            var scenContainer = CMOScenTool.LoadScenContainer(content);

            Console.WriteLine(scenContainer);
            Console.WriteLine(scenContainer.GetScenarioObject_AsXML());
        }
    }
}
