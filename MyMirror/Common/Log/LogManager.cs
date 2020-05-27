// -----------------------------------------------------------------------
// <copyright file="LogManager.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class LogManager</summary>
// -----------------------------------------------------------------------

namespace Common.Log
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;

    /// <summary>
    /// Manages debug log
    /// </summary>
    public static class LogManager
    {
        #region Events

        /// <summary>
        /// Event risen on log file updating
        /// </summary>
        public static event EventHandler LogFileUpdated;

        #endregion

        #region Private members

        /// <summary>
        /// File access mutex
        /// </summary>
        private static Mutex _accessMutex;

        /// <summary>
        /// Path to session log file
        /// </summary>
        /// <value>Path of session log</value>
        private static string _sessionLogFilePath;

        #endregion

        #region Public methode

        /// <summary>
        /// Return current log file content
        /// </summary>
        /// <returns>Content of the file</returns>
        public static string GetLogFileContent()
        {
            string ret = null;

            if (File.Exists(_sessionLogFilePath))
            {
                ret = File.ReadAllText(_sessionLogFilePath);
            }

            return ret;
        }

        /// <summary>
        /// Static method to initialize the Session Log with user name and session beginning date
        /// </summary>
        public static void InitializeSessionLog()
        {
            _accessMutex = new Mutex();
            string filename = "Log_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss", CultureInfo.InvariantCulture) + ".log";
            string friendlyName = AppDomain.CurrentDomain.FriendlyName;
            string assemblyName = friendlyName.Substring(0, friendlyName.Length - ".exe".Length);
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            _sessionLogFilePath = Path.Combine(folder, "MyMirror", "Log", filename);

            if (!Directory.Exists(Path.GetDirectoryName(_sessionLogFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_sessionLogFilePath));
            }

            LogLine("Start");

            // Open session log file in writing mode
            if (!File.Exists(_sessionLogFilePath))
            {
                FileStream file = File.Create(_sessionLogFilePath);
                file.Close();
            }
        }

        /// <summary>
        /// Static method to add a line to log file
        /// </summary>
        /// <param name="text">Text to show</param>
        public static void LogLine(string text, [CallerFilePath] string propertyName = null)
        {
            if (_accessMutex.WaitOne(1000))
            {
                // Open session log file in writing mode
                StreamWriter sessionLogWriter = File.AppendText(_sessionLogFilePath);
                CultureInfo culture = CultureInfo.CurrentUICulture;

                sessionLogWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", culture) + " => " + text + "(" + propertyName + ")");
                sessionLogWriter.Close();

                LogFileUpdated?.Invoke(null, null);
            }
        }

        #endregion
    }
}
