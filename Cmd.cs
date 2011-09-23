namespace keystoreBrute
{
    using System;
    using System.Diagnostics;

    public class Cmd
    {
        public static Process Execute(String command, String arguments = "")
        {
            Process proc = new Process();
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

            return proc;
        }
    }
}