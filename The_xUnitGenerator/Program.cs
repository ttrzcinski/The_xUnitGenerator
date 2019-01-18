using System;
using The_xUnitGenerator.backend;

namespace The_xUnitGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool given2nd = false;

            // Check, what user wanted
            Console.WriteLine("-----------------------------");
            Console.WriteLine("  The_xUnitGenerator 0.0.1.");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("For more information add -?");

            // Compare with known arguments
            // TODO ADD HERE WAIT FOR USER INPUT WITH WAANTED COMMAND NAD FIXED ARUGUMENTS
            var looping = true;
            // Start looping, until user stops with exit
            do
            {
                // Obtain command from user
                Console.WriteLine("What can be done?");
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] lineParts = line.Split(" ");
                lineParts[0] = lineParts[0].Trim().ToLower();
                if (lineParts.Length > 1) given2nd = !string.IsNullOrWhiteSpace(lineParts[1]);

                // Start comparing with known commands
                switch (lineParts[0])
                {
                    case "gen":
                        if (!given2nd)
                        {
                            Console.WriteLine("Generating for what?");
                            continue;
                        }

                        Console.WriteLine("Calling xUnit Generator..");
                        TestGenerator testGenerator = new TestGenerator();
                        string response = testGenerator.FindRepositoryURL(lineParts[1]);
                        if (string.IsNullOrWhiteSpace(response))
                        {
                            Console.WriteLine("Obtained git url is empty.");
                            continue;
                        }
                        response = $"git clone {response}";

                        FastLine fastLine = new FastLine();
                        if (!fastLine.GetIsGitRunning())
                        {
                            Console.WriteLine("GIT IS NOT HERE! - Please, install git.");
                            continue;
                        }
                        string result = fastLine.Cmd("git status");
                        Console.WriteLine(result);
                        break;

                    case "co":
                        if (!given2nd)
                        {
                            Console.WriteLine("Checking out what?");
                            continue;
                        }
                        else
                        {
                            Console.WriteLine($"Calling GitHub with {args[1]}.");
                        }
                        break;

                    case "tst":
                        if (!given2nd)
                        {
                            Console.WriteLine("Testing what?");
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Calling default test runner..");
                        }
                        break;

                    case "psh":
                        if (!given2nd)
                        {
                            Console.WriteLine("Pushing what?");
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Pushing changes..");
                        }
                        break;

                    case "exit":
                        looping = false;
                        break;

                    case "quit":
                        looping = false;
                        break;

                    default:
                        Console.WriteLine("I didn't understand that.");
                        break;
                }
            } while (looping);
            Console.WriteLine("Thanks and bye..");
        }
    }
}
