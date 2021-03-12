using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace UploadGit.Server.Utilities
{
    public class GitCaller
    {
        public string ExecutablePath { get; }
        public string WorkingDirectory { get; }

        public GitCaller(string executablePath, string workingDirectory = null)
        {
            ExecutablePath = executablePath ?? throw new ArgumentNullException(nameof(executablePath));
            WorkingDirectory = workingDirectory ?? System.IO.Path.GetDirectoryName(executablePath);
        }

        public string Run(string arguments)
        {

            var info = new ProcessStartInfo(ExecutablePath, arguments)
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = WorkingDirectory
            };

            var process = new Process
            {
                StartInfo = info
            };

            try
            {
                process.Start();
                return process.StandardOutput.ReadToEnd();
            }

            catch (Exception e)
            {
                return $"Something went wrong when trying" +
                    $" to start the process. \n Error Message: {e.Message}";
            }
        }
    }
}
