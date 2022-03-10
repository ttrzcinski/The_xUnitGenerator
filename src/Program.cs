using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace The_xUnitGenerator
{

    static class Program
    {
        static readonly List<string> AllFiles = new List<string>();
        static int ChangeCounter = 5;

        private static void ParsePath(string path)
        {
            string[] SubDirs = Directory.GetDirectories(path);
            //AllFiles.AddRange(SubDirs);
            var temp=Directory.GetFiles(path,"*.cs");
            AllFiles.AddRange(temp.Where(s=>!s.Contains("AssemblyInfo.cs")));
            foreach (string subdir in SubDirs)
                ParsePath(subdir);
        }

        private  static string projectLocation = @"";


        static async Task Main()
        {

            ParsePath(projectLocation);
            string TestProjectLocation = projectLocation.Replace("/src/", "/tests/");
            foreach (string file in AllFiles)
            {
                FileInfo fileInfo = new(file);
                Directory.CreateDirectory(TestProjectLocation);
                string TestFileLocation = $"{TestProjectLocation}/{fileInfo.Name.Replace(".cs", "")}Tests.cs";
                if (!File.Exists(TestFileLocation) && ChangeCounter > 0)
                {
                    string programText = File.ReadAllText(file);
                    CSCodeAnalyzer CSCodeAnalyzer = new();
                    var y = CSCodeAnalyzer.AnalyzeCode(programText);
                    foreach (var z in new PatternGenerator().GenerateCode(programText, y).ToList())
                    {
                        Console.WriteLine(z.Data);
                        File.WriteAllText(TestFileLocation, z.Data);
                    }
                    ChangeCounter--;
                }
            }
        }
    }
}
