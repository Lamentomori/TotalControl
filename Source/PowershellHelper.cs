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
    }
}
