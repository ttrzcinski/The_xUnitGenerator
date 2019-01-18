using System;
using System.Net;

namespace The_xUnitGenerator.backend
{
    class TestGenerator
    {
        private string Filter { set; get; } = "*.cs";
    
        public TestGenerator() {}

        private string AsError(string line)
        {
            Console.WriteLine($"FindRepositoryURL - {line}");
            return null;
        }

        public string FindRepositoryURL(string name)
        {
            bool flag_createAFile = false;

            if (string.IsNullOrWhiteSpace(name)) return AsError("Given name has no value.");
            name = name.Trim();

            // Parse user/name_of_repo into two vars
            if (!name.Contains("/")) return AsError("Given name has no slash.");
            string[] lineParts = name.Split("/");
            if (string.IsNullOrWhiteSpace(lineParts[0])) return AsError("Given name has no user name.");
            if (string.IsNullOrWhiteSpace(lineParts[1])) return AsError("Given name has no project name.");

            // Check, if there is a repository of pointed user
            string htmlCode = null;
            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                string tmpURL = $"https://github.com/{lineParts[0]}/{lineParts[1]}.git";
                // CHANGE TO LOCAL PROJECT'S TMP DIRECTORY
                if (flag_createAFile) {
                    string projPath = System.IO.Directory.GetCurrentDirectory();
                    Console.WriteLine("Created file: " + projPath);
                    client.DownloadFile(tmpURL, $"{projPath}\\localfile.git");
                }

                // Or you can get the file content without saving it;
                //Console.WriteLine("Created git: " + tmpURL);
                htmlCode = client.DownloadString(tmpURL);
            }
            if (string.IsNullOrWhiteSpace(htmlCode)) return AsError("Obtained github webpage has no content.");

            string url = null;
            // TODO OBTAIN URL TO CLONE
            if (htmlCode.Contains("https://github.com") || htmlCode.Contains(".git"))
            {
                htmlCode = htmlCode.Substring(0, htmlCode.LastIndexOf(".git\""));
                htmlCode = htmlCode + ".git";
                htmlCode = htmlCode.Substring(htmlCode.LastIndexOf("\"https://github.com")+1);
                url = htmlCode;
            }
            else
            {
                Console.WriteLine("It doesn't contain a link to GitHub.");
            }

            return url;
        }

        //
        public string[] ListMethods(string className)
        {
            if (string.IsNullOrWhiteSpace(className)) return null;

            string[] result = null;

            // TODO FIND A FILE WITH CODE

            // TODO LIST ALL THE CLASSES IN IT

            // TODO LIST ALL THE METHODS FOR EVERY CLASS

            // TODO GENERATE NEW TEST PROJECT NEAR SOURCE CODE PROJECT WITHING SOLUTION

            // TODO ADD REFERENCE TO SOURCE CODE TO COVER

            // TODO COPY DIRECTORIES STRUCTURE

            // TODO GENERATE TEST FILES WITH TESTS COVERING EVERY CLASS

            return result;
        }
    }
}
