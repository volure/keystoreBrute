namespace keystoreBrute
{
    using System;
    using System.Diagnostics;

    public class Shell
    {
        public static string Execute(String command, String arguments = "")
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.FileName = command;
                proc.StartInfo.Arguments = arguments;
                proc.StartInfo.RedirectStandardOutput=true;
                proc.StartInfo.RedirectStandardError=true;
                proc.StartInfo.CreateNoWindow = false;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                proc.Start();
                proc.WaitForExit();
                return proc.StandardOutput.ReadToEnd().Trim();
        }
    }
}