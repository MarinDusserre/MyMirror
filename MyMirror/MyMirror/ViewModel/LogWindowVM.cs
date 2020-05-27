// -----------------------------------------------------------------------
// <copyright file="LogWindowVM.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class LogWindowVM</summary>
// -----------------------------------------------------------------------

namespace MyMirror.ViewModel
{
    using Common.Log;
    using Common.ViewModel;
    using System.Windows;
    using System.Windows.Input;
    using System;

    /// <summary>
    /// View model for the log window
    /// </summary>
    internal class LogWindowVM : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// Gets log text
        /// </summary>
        public string LogText
        {
            get => _logText;
            private set => Set(ref _logText, value);
        }

        /// <summary>
        /// Gets exit button command
        /// </summary>
        public ICommand ExitButtonCommand { get; private set; }

        #endregion

        #region Private members

        /// <summary>
        /// Log text
        /// </summary>
        private string _logText;

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public LogWindowVM()
        {
            ExitButtonCommand = new RelayCommand(ExitButton);

            LogManager.LogFileUpdated += OnLogFileUpdate;
            LogText = LogManager.GetLogFileContent();
        }

        #endregion

        #region Private functions

        /// <summary>
        /// Handles exit command
        /// </summary>
        /// <param name="obj">Parameters</param>
        private void ExitButton(object obj)
        {
            ((Window)obj)?.Close();
        }

        /// <summary>
        /// Handles log file update
        /// </summary>
        private void OnLogFileUpdate(object sender, EventArgs e)
        {
            LogText = LogManager.GetLogFileContent();
        }

        #endregion
    }
}