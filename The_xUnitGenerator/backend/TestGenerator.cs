using System;
using System.Net;

namespace The_xUnitGenerator.backend
{
    class TestGenerator
    {
        private string Filter { set; get; } = "*.cs";

        public TestGenerator() { }

        /// <summary>
        /// Calls the logger tolog an error and return null as error result.
        /// </summary>
        /// <param name="line">given line to log</param>
        /// <returns>null</returns>
        private string AsError(string line)
        {
            Console.WriteLine($"FindRepositoryURL - {line}");
            return null;
        }

        /// <summary>
        /// Finds git url to Github repository.
        /// </summary>
        /// <param name="name">name of repository</param>
        /// <returns>url, if found, null toherwise</returns>
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

                FastLine fastLine = new FastLine();
                string projPath = fastLine.GetCurrentProjectsDirectory();

                if (projPath != null && flag_createAFile)
                {
                    // CHANGE TO LOCAL PROJECT'S TMP DIRECTORY
                    //string projPath = System.IO.Directory.GetCurrentDirectory();

                    // TODO CHECK, IF TMP DIRECTORY EXISTS


                    // TODO CREATE DIRECTORY, 
                    fastLine.Cmd($"cd %{projPath}% && mkdir tmp");

                    string filePath = $"{projPath}\\localfile.git";
                    client.DownloadFile(tmpURL, filePath);
                    Console.WriteLine($"Created file: {filePath}");
                }

                // Or you can get the file content without saving it;
                try
                {
                    htmlCode = client.DownloadString(tmpURL);
                }
                catch (Exception exc_1)
                {
                    //The remote server returned an error: (404) Not Found.'
                    // TODO CHANGE TO NULL AND PASSING EXCEPTINO EXTERNALLY
                    return exc_1.Message;
                }
            }
            if (string.IsNullOrWhiteSpace(htmlCode)) return AsError("Obtained github webpage has no content.");

            string url = null;
            // Obtain url to clone
            if (htmlCode.Contains("https://github.com") || htmlCode.Contains(".git"))
            {
                htmlCode = htmlCode.Substring(0, htmlCode.LastIndexOf(".git\""));
                htmlCode = htmlCode + ".git";
                htmlCode = htmlCode.Substring(htmlCode.LastIndexOf("\"https://github.com") + 1);
                url = htmlCode;
                // Checks, If the outcome is a single valid url
                return FastValidators.CheckURLValid(url) ? url : AsError("Obtained url is wrong: " + url);
            }
            return AsError("It doesn't contain a link to GitHub.");
        }

        /// <summary>
        /// Lists methods of pointed class.
        /// </summary>
        /// <param name="className">pointed class</param>
        /// <returns>lsit of methods, if found, null otherwise</returns>
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
