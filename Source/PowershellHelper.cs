using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalControl
{
    internal class PowershellHelper
    {
        public static void Run(string script)
        {
            using (var ps = new Process())
            {
                ps.StartInfo.FileName = "powershell.exe";
                ps.StartInfo.Arguments = $"-NoProfile -ExecutionPolicy Bypass -WindowStyle Hidden -Command \"{script}\"";
                ps.StartInfo.Verb = "runas"; // ensure admin
                ps.StartInfo.CreateNoWindow = true;
                ps.StartInfo.UseShellExecute = true;
                ps.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                ps.Start();
                ps.WaitForExit();
            }
        }
    

    public static List<string> RunWithOutput(string script)
        {
            var output = new List<string>();

            using (var ps = new Process())
            {
                ps.StartInfo.FileName = "powershell.exe";
                ps.StartInfo.Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{script}\"";
                ps.StartInfo.Verb = "runas"; // Run as admin
                ps.StartInfo.CreateNoWindow = true;
                ps.StartInfo.UseShellExecute = false; // Required to redirect output
                ps.StartInfo.RedirectStandardOutput = true;
                ps.StartInfo.RedirectStandardError = true;
                ps.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                ps.Start();

                // Read output line by line
                while (!ps.StandardOutput.EndOfStream)
                {
                    var line = ps.StandardOutput.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                        output.Add(line);
                }

                // Optionally capture errors for debugging
                var errors = ps.StandardError.ReadToEnd();
                if (!string.IsNullOrWhiteSpace(errors))
                {
                    // You can log or throw here if needed
                    Console.WriteLine("PowerShell Errors: " + errors);
                }

                ps.WaitForExit();
            }

            return output;
        }
    }
    
}
