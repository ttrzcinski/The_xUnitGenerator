using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace The_xUnitGenerator.backend
{
    class FastLine
    {
        /// <summary>
        /// Runs given lin as cmd call.
        /// </summary>
        /// <param name="line">given line</param>
        /// <param name="echoOutput">shows output of user's console 0 by default turend off</param>
        /// <returns>console output, if there was some, null otherwise</returns>
        public string Cmd(string line, bool echoOutput = false)
        {
            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            // Run this cmd command
            cmd.StandardInput.WriteLine(line);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            var output = cmd.StandardOutput.ReadToEnd();
            if (echoOutput) Console.WriteLine(output);
            return string.IsNullOrWhiteSpace(output) ? null : output;
        }

        /// <summary>
        /// Checks, if git is set on the console.
        /// </summary>
        /// <returns>true means it is set, false otherwise</returns>
        public bool GetIsGitRunning()
        {
            var value = Cmd("git --version");
            return value != null ? value.Contains("git version ") : false;
        }

        /// <summary>
        /// Returns current directory's path.
        /// </summary>
        /// <returns>current directory's path</returns>
        public string WhereAmI() => Cmd("echo %cd%");

        public bool IsCurrentDirectoryEmpty()
        {
            List<string> result = ListFiles();
            return result != null && result.Count == 0;
        }

        /// <summary>
        /// Lsts of the files in current directory.
        /// </summary>
        /// <returns>list of files from current directory, null means error</returns>
        public List<string> ListFiles()
        {
            // Checked obtained list of files
            string response = Cmd("dir /w");
            if (string.IsNullOrWhiteSpace(response)) return null;
            // Prepare results
            List<string> results = new List<string>();
            // Chop obtained list -- 22 characters per column
            string[] lines = response.Split(Environment.NewLine);
            // Check, if actually directory is empty.
            if (lines.Length < 5) return results;
            // Start parsing those meaningful lines - with files and subdirectories
            for (int i = 5; i < lines.Length; i++)
            {
                results.Add("files in line " + i + " : " + (lines[i].Length % 22));
            }

            return results;
        }
    }
}
