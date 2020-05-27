// -----------------------------------------------------------------------
// <copyright file="TimeModel.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class TimeModel</summary>
// -----------------------------------------------------------------------

namespace TimeWidget.Model
{
    using System.Timers;
    using System;
    using Common.Settings;
    using Common.ViewModel;
    using TimeWidget.Properties;

    /// <summary>
    /// Contains Time widget model
    /// </summary>
    public class TimeModel : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Gets windget settings
        /// </summary>
        public SettingsManager<TimeSettings> SettingsManager { get; internal set; }

        /// <summary>
        /// Gets current time with hours, minutes and seconds
        /// </summary>
        public string CurrentTimeWithSecString
        {
            get => _currentTimeWithSecString;
            private set => Set(ref _currentTimeWithSecString, value);
        }

        /// <summary>
        /// Gets current time with hours and minutes
        /// </summary>
        public string CurrentTimeString
        {
            get => _currentTimeString;
            private set => Set(ref _currentTimeString, value);
        }

        /// <summary>
        /// Gets seconds angle 
        /// </summary>
        public double SecAngle
        {
            get => _secAngle;
            private set => Set(ref _secAngle, value);
        }

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructeur
        /// </summary>
        public TimeModel()
        {
            SettingsManager = new SettingsManager<TimeSettings>();
            SettingsManager.Initialize(Resources.SettingsFileName);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Refresh timer
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Current time with hours, minutes and seconds 
        /// </summary>
        private string _currentTimeWithSecString;

        /// <summary>
        /// Current time with hours and minutes
        /// </summary>
        private string _currentTimeString;

        /// <summary>
        /// Seconds angle
        /// </summary>
        private double _secAngle;

        #endregion

        #region Public methodes

        /// <summary>
        /// Initialize timer
        /// </summary>
        public void Initialize()
        {
            _timer = new Timer(1000)
            {
                AutoReset = true
            };
            _timer.Elapsed += UptadeTime;
            _timer.Start();
        }

        #endregion

        #region Private methodes

        /// <summary>
        /// Update time on timer
        /// </summary>
        /// <param name="s">Sender</param>
        /// <param name="e">Arguments</param>
        private void UptadeTime(object s, ElapsedEventArgs e)
        {
            CurrentTimeWithSecString = DateTime.Now.ToLongTimeString();
            CurrentTimeString = DateTime.Now.ToShortTimeString();
            SecAngle = DateTime.Now.Second * 6;
        }

        #endregion
    }
}
