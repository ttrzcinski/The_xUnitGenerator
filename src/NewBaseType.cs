using System;
using The_xUnitGenerator.backend;

namespace The_xUnitGenerator
{
    internal class NewBaseType
    {
        public static void Obsolete()
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("  The_xUnitGenerator 0.0.1.");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("For more information add -?");

            FastLine fastLine = new FastLine();
            TestGenerator testGenerator = new TestGenerator();

            bool given2nd = false;
            // Check, what user wanted

            // TODO CHECK, IF IN ARGUMENTS WAS ALREADY PROVIDED FULL COMMAND WITH PARAMS

            // TODO CHECK, IF GIT IS SET
            if (!fastLine.GetIsGitRunning())
            {
                Console.WriteLine("GIT IS NOT HERE! - Please, install git first.");
                return;
            }

            // TODO CHECK, IF GITHUB RESPONDS MEANING NET CONNECTION WORKS

            // Compare with known arguments
            var looping = true;
            // Start looping, until user stops with exit
            do
            {
                // Obtain command from user with params
                Console.WriteLine($"{Environment.NewLine}What has to be done?");
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
                        string response = testGenerator.FindRepositoryURL(lineParts[1]);
                        if (string.IsNullOrWhiteSpace(response))
                        {
                            Console.WriteLine("Obtained git url is empty.");
                            continue;
                        }
                        response = $"git clone {response}";

                        string result = fastLine.Cmd("git status");
                        Console.WriteLine(result);
                        break;

                    case "co":
                        if (!given2nd)
                        {
                            Console.WriteLine("Checking out what?");
                            continue;
                        }

                        lineParts[1] = lineParts[1].Trim().ToLower();
                        if (!FastValidators.CheckURLValid(lineParts[1]))
                        {
                            Console.WriteLine("It is not a valid git repository url.");
                            continue;
                        }
                        Console.WriteLine($"Calling GitHub with {lineParts[1]}.");

                        //

                        break;

                    case "tst":
                        Console.WriteLine($"solution: {fastLine.GetCurrentSolutionDirectory()}");
                        Console.WriteLine($"project: {fastLine.GetCurrentProjectsDirectory()}");

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

                    case "?":
                        Console.WriteLine("Maybe something, like:");
                        Console.WriteLine("gen ttrzcinski/Clint");
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
