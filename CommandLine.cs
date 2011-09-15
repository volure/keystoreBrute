// ***************************************
//
// BaileySoft Solutions - CommandLine.cs
// Copyright (c) 2010 - All rights reserved.
// nealbailey@hotmail.com 
//
// ***************************************
//
//
// -- Sample Usage -- 
//
// CommandLine oCommandLine = new CommandLine();
// oCommandLine.ExecutionContext = ExecutionType.SHELL_EXECUTE_VISIBLE;
// oCommandLine.Path = "c:\\programToRun.exe";
// oCommandLine.Parameters = "-switches";
// oCommandLine.BeginExecute();     //asyncronous 
//
//
// -- If you want to know when the process is done executing add below to your caller --
//
// oCommandLine.HasExited += new HasExitedEventHandler(oCommandLine_HasExited);
//
// private void oCommandLine_HasExited(object sender, HasExitedEventArgs e)
// {
//    MessageBox.Show("Process is finally completed!");
// }
//
// ****************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;

namespace BaileySoft.Utility
{
    public enum ExecutionType
    {
        WINDOWS_EXECUTE_VISIBLE,
        WINDOWS_EXECUTE_HIDDEN,
        SHELL_EXECUTE_HIDDEN,
        SHELL_EXECUTE_VISIBLE
    }

    public delegate void HasExitedEventHandler(object sender, HasExitedEventArgs e);

    public class CommandLine
    {
        #region fields

        private string m_shellOutput;
        private Exception m_Error;
        private ExecutionType m_ExType;
        private DateTime m_startTime;
        private TimeSpan m_Runtime;

        private string m_ApplicationPath;
        private string m_ApplicationParameters;
        private int? m_TimeOutThreshold;

        private bool m_timeoutExpired;
        private bool m_isCompleted;
        private System.Timers.Timer pTimer;

        // Define event for determining if a shell process has exited
        public event HasExitedEventHandler HasExited;

        // Process handle for shell execution 
        Process m_pHandle = null;

        #endregion

        #region properties

        /// <summary>
        /// The path to the program to run.
        /// </summary>
        public string Path
        {
            get { return m_ApplicationPath; }
            set { m_ApplicationPath = value; }
        }

        /// <summary>
        /// The parameter string to send to the program.
        /// </summary>
        public string Parameters
        {
            get { return m_ApplicationParameters; }
            set { m_ApplicationParameters = value; }
        }

        /// <summary>
        /// Execution Type.
        /// </summary>
        public ExecutionType ExecutionContext
        {
            get { return m_ExType; }
            set { m_ExType = value; }
        }

        /// <summary>
        /// How long to wait for the process to complete.
        /// </summary>
        public int? TimeOut
        {
            get { return m_TimeOutThreshold; }
            set { m_TimeOutThreshold = value; }
        }

        /// <summary>
        /// How long the process ran.
        /// </summary>
        public TimeSpan Runtime
        {
            get { return m_Runtime; }
        }

        /// <summary>
        /// The StdOutput stream (Windows mode only).
        /// </summary>
        public String ShellOutput
        {
            get
            { return m_shellOutput ?? "Unable to redirect stdout stream."; }
        }

        /// <summary>
        /// The error thrown by the object.
        /// </summary>
        public Exception Error
        {
            get
            { return m_Error ?? new SystemException(); }
        }

        #endregion
        
        /// <summary>
        /// Constructor (+1 overloads.)
        /// </summary>
        public CommandLine() : this(ExecutionType.SHELL_EXECUTE_VISIBLE) { }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="exType">ExecutionType for the process</param>
        public CommandLine(ExecutionType exType)
        {
            this.m_ExType = exType;
        }

        /// <summary>
        /// Executes the process (+3 overloads).
        /// </summary>
        public void Execute()
        {
            Execute(m_ApplicationPath); 
        }

        /// <summary>
        /// Executes the process (+2 overloads).
        /// </summary>
        /// <param name="applicationPath">The path to the program to execute.</param>
        public void Execute(string applicationPath)
        {
            Execute(m_ApplicationPath, m_ApplicationParameters); 
        }

        /// <summary>
        /// Executes the process (+1 overloads).
        /// </summary>
        /// <param name="applicationPath">The path to the program to execute.</param>
        /// <param name="parameters">Parameters to send to the program.</param>
        public void Execute(string applicationPath, string parameters)
        {
            Execute(applicationPath, parameters, m_TimeOutThreshold);
        }

        /// <summary>
        /// Executes the process.
        /// </summary>
        /// <param name="applicationPath">The path to the process to start</param>
        /// <param name="parameters">The parameters to send to the process</param>
        /// <param name="timeout">Optional timeout value</param>
        public void Execute(string applicationPath, string parameters, int? timeout)
        {
            if (String.IsNullOrEmpty(applicationPath))// || !File.Exists(applicationPath))
                throw new ArgumentException("Application must exist and its path cannot be null.");

            m_ApplicationPath = applicationPath;
            m_ApplicationParameters = parameters;
            m_TimeOutThreshold = timeout;
            m_isCompleted = false; // for shell execution
            m_timeoutExpired = false;

            // Capture when the process began
            m_startTime = DateTime.Now;

            switch (ExecutionContext)
            {
                case ExecutionType.WINDOWS_EXECUTE_VISIBLE:
                    WindowsExecute();
                    break;
                case ExecutionType.WINDOWS_EXECUTE_HIDDEN:
                    WindowsExecute();
                    break;
                case ExecutionType.SHELL_EXECUTE_VISIBLE:
                    DoShellExecute();
                    break;
                case ExecutionType.SHELL_EXECUTE_HIDDEN:
                    DoShellExecute();
                    break;
            }
        }


        /// <summary>
        /// Executes the process asynchonously (+3 overloads).
        /// </summary>
        public void BeginExecute()
        {
            BeginExecute(m_ApplicationPath);
        }

        /// <summary>
        /// Executes the process asynchonously (+2 overloads).
        /// </summary>
        /// <param name="applicationPath">The path to the program to execute.</param>
        public void BeginExecute(string applicationPath)
        {
            BeginExecute(m_ApplicationPath, m_ApplicationParameters);
        }

        /// <summary>
        /// Executes the process asynchonously (+1 overloads).
        /// </summary>
        /// <param name="applicationPath">The path to the program to execute.</param>
        /// <param name="parameters">Parameters to send to the program.</param>
        public void BeginExecute(string applicationPath, string parameters)
        {
            BeginExecute(applicationPath, parameters, m_TimeOutThreshold);
        }

        /// <summary>
        /// Executes the process asynchonously.
        /// </summary>
        /// <param name="applicationPath">The path to the process to start</param>
        /// <param name="parameters">The parameters to send to the process</param>
        /// <param name="timeout">Optional timeout value</param>
        public void BeginExecute(string applicationPath, string parameters, int? timeout)
        {
            if (String.IsNullOrEmpty(applicationPath))
                throw new ArgumentException("Application path cannot be null.");

            m_ApplicationPath = applicationPath;
            m_ApplicationParameters = parameters;
            m_TimeOutThreshold = timeout;
            m_isCompleted = false; // for shell execution
            m_timeoutExpired = false;

            // Capture when the process began
            m_startTime = DateTime.Now;

            switch (ExecutionContext)
            {
                case ExecutionType.WINDOWS_EXECUTE_VISIBLE:
                    BeginWindowsExecute();
                    break;
                case ExecutionType.WINDOWS_EXECUTE_HIDDEN:
                    BeginWindowsExecute();
                    break;
                case ExecutionType.SHELL_EXECUTE_VISIBLE:
                    BeginDoShellExecute();
                    break;
                case ExecutionType.SHELL_EXECUTE_HIDDEN:
                    BeginDoShellExecute();
                    break;
            }
        }

        // Start process in new thread
        private void BeginWindowsExecute()
        {
            Thread thread = new Thread(new ThreadStart(WindowsExecute));
            thread.Priority = ThreadPriority.Lowest;
            thread.Name = "myProc";
            thread.Start();
        }

        // Execute using Windows > Run
        private void WindowsExecute()
        {
            ProcessStartInfo psi = new ProcessStartInfo(m_ApplicationPath);
            psi.Arguments = m_ApplicationParameters;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;

            if (m_ExType == ExecutionType.WINDOWS_EXECUTE_VISIBLE)
            {
                psi.CreateNoWindow = false;
                psi.WindowStyle = ProcessWindowStyle.Normal;
            }

            m_pHandle = new Process();
            StreamReader p_output = null;

            if (pTimer == null)
                CreateTimeoutClock();

            try
            {
                m_pHandle = Process.Start(psi);
                p_output = m_pHandle.StandardOutput;
                m_shellOutput = p_output.ReadToEnd();

                if (m_TimeOutThreshold != null)
                {
                    if (m_TimeOutThreshold == 0)
                    {
                        m_pHandle.WaitForExit();
                    }
                    else
                    {
                        m_pHandle.WaitForExit((int)m_TimeOutThreshold);
                    }
                }
                else
                {
                    m_pHandle.WaitForExit();
                }
            }
            catch (Exception e)
            {
                m_Error = e;
            }
            finally
            {
                m_pHandle.Close();
                p_output.Close();
            }

            // Calculate how long the process took to complete
            DateTime finishTime = DateTime.Now;
            m_Runtime = finishTime.Subtract(m_startTime);

            // Raise our exiting Event
            HasExitedEventArgs args = new HasExitedEventArgs(m_isCompleted);
            OnHasExited(args);
        }

        // Start the ShellEx in a new thread.
        private void BeginDoShellExecute()
        {
            Thread thread = new Thread(new ThreadStart(DoShellExecute));
            thread.Priority = ThreadPriority.Lowest;
            thread.Name = "myProc";
            thread.Start();
        }

        // Threaded Shell Execution. 
        private void DoShellExecute()
        {
            ProcessStartInfo psi = new ProcessStartInfo(m_ApplicationPath);
            psi.Arguments = m_ApplicationParameters;
            psi.UseShellExecute = true;
            psi.RedirectStandardOutput = false;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Normal;

            if (ExecutionContext == ExecutionType.SHELL_EXECUTE_HIDDEN)
                psi.WindowStyle = ProcessWindowStyle.Hidden;

            m_pHandle = new Process();
            m_shellOutput = "Cannot redirect output streams in shell mode.";
            m_pHandle = Process.Start(psi);

            try
            {
                // Sadly there is no way to determine if a shell process has finished. We're going to 
                // endlessly loop until the process finishes, and throws an error. Then we know it's done. 
                Process procHandle = null;

                while (procHandle == null && !m_timeoutExpired)
                {
                    if (m_TimeOutThreshold != null && pTimer == null)
                    {
                        CreateTimeoutClock();
                    }
                    else
                    {
                        Process.GetProcessById(m_pHandle.Id);
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (ArgumentException) { }
            finally
            {
                m_pHandle.Close();
            }

            m_isCompleted = true;

            // Calculate how long the process took to complete
            DateTime finishTime = DateTime.Now;
            m_Runtime = finishTime.Subtract(m_startTime);

            // Raise our Exiting Event
            HasExitedEventArgs e = new HasExitedEventArgs(m_isCompleted);
            OnHasExited(e);
        }

        // Let's create a timer to allow for timeout (having to do this is so lame) :(
        private void CreateTimeoutClock()
        {
            pTimer = new System.Timers.Timer();
            pTimer.Elapsed += new ElapsedEventHandler(OnTimeoutExpired);
            pTimer.Interval = Convert.ToDouble(m_TimeOutThreshold * 1000);
            pTimer.Enabled = true;
        }

        // TimeoutExpired Event
        private void OnTimeoutExpired(object source, ElapsedEventArgs e)
        {
            // Timeout expired, assassinate the process!
            m_timeoutExpired = true;
            KillProcess();
            pTimer.Enabled = false;
            pTimer = null;
        }

        // Handle Event
        protected virtual void OnHasExited(HasExitedEventArgs e)
        {
            if (HasExited != null)
            {
                HasExited(this, e);
            }
        }

        // It's been nice, but now it's time for you to die. ;)
        private void KillProcess()
        {
            try
            {
                m_pHandle.Kill();
                m_pHandle.Close();
                m_pHandle.Dispose();
            }
            catch (Exception e) 
            { m_Error = e; }
        }
    }

    /// <summary>
    /// EventArgs for determining if the process is completed.
    /// </summary>
    public class HasExitedEventArgs : EventArgs
    {
        private bool _done = false;

        // Constructor
		public HasExitedEventArgs(bool done)
		{
			this._done = done;
		}

		public bool IsCompleted
		{
            get
            { return _done; }
		}
    }
}