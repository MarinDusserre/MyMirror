// -----------------------------------------------------------------------
// <copyright file="MainModel.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class MainModel</summary>
// -----------------------------------------------------------------------

namespace MyMirror.Model
{
    using InputContract;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using WingetContract;
    using Common.ViewModel;
    using Common.Log;
    using Common.Settings;
    using MyMirror.Properties;
    using WingetContract.Enum;
    using Common.Annimations;
    using MyMirror.Model.Led;

    /// <summary>
    /// Contains application main model
    /// </summary>
    internal class MainModel : ObservableObject
    {
        #region Singleton declaration

        /// <summary>
        /// Singleton declaration
        /// </summary>
        private static readonly Lazy<MainModel> Lazy = new Lazy<MainModel>(() => new MainModel());

        /// <summary>
        /// Singleton propertie
        /// </summary>
        public static MainModel Instance => Lazy.Value;

        #endregion Singleton declaration

        #region Properties

        /// <summary>
        /// Gets widget list
        /// </summary>
        internal ICollection<IWidget> WidgetList { get; private set; }

        /// <summary>
        /// Gets screen input
        /// </summary>
        internal ICollection<IScreenInput> ScreenInputs { get; private set; }

        /// <summary>
        /// Gets sound level from 0 to 100
        /// </summary>
        public int SoundLevel
        {
            get => _soundLevel;
            private set => Set(ref _soundLevel, value);
        }

        /// <summary>
        /// Gets current main message
        /// </summary>
        public string MainMessage
        {
            get => _mainMessage;
            private set => Set(ref _mainMessage, value);
        }

        /// <summary>
        /// Gets current main message opacity
        /// </summary>
        public double MainMessageOpacity
        {
            get => _mainMessageOpacity;
            private set => Set(ref _mainMessageOpacity, value);
        }

        /// <summary>
        /// Gets main settings
        /// </summary>
        public SettingsManager<MainSettings> MainSettings { get; private set; }

        /// <summary>
        /// Getsl ed manager
        /// </summary>
        public ILedManager LedManager { get; private set; }

        #endregion

        #region Private members

        /// <summary>
        /// Current audio level
        /// </summary>
        private int _soundLevel;

        /// <summary>
        /// Current main messgae
        /// </summary>
        private string _mainMessage;

        /// <summary>
        /// Current main message opacity
        /// </summary>
        private double _mainMessageOpacity;

        /// <summary>
        /// Main message opacity annimation
        /// </summary>
        private FadeAnnimation _mainMessageAnnimation;


        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainModel()
        {
            LogManager.InitializeSessionLog();
            _soundLevel = 30;

            MainSettings = new SettingsManager<MainSettings>();
            MainSettings.Initialize(Resources.SettingsFileName);

            _mainMessageAnnimation = new FadeAnnimation(value => MainMessageOpacity = value);
            LedManager = LedManagerFactory.GetLedManager(MainSettings.Settings.LedsMode.Value, MainSettings.Settings.LedsPort.Value);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Load widgets
        /// </summary>
        internal void LoadWinget()
        {
            WidgetList = PluginLoader<IWidget>.LoadPlugins(Resources.WidgetFolder, Resources.WidgetFilter);
            AddVolume(0);
        }

        /// <summary>
        /// Load input
        /// </summary>
        internal void LoadInput()
        {
            ScreenInputs = PluginLoader<IScreenInput>.LoadPlugins(Resources.InputFolder, Resources.InputFilter);
        }

        /// <summary>
        /// Initilaize settings
        /// </summary>
        internal void InitializeMainSettings()
        {
            MainSettings.Settings.TopLeftWidget.PossibleValues = PossibleWidgets(WidgetPositionEnum.TopLeft);
            MainSettings.Settings.TopWidget.PossibleValues = PossibleWidgets(WidgetPositionEnum.Top);
            MainSettings.Settings.TopRightWidget.PossibleValues = PossibleWidgets(WidgetPositionEnum.TopRight);
            MainSettings.Settings.LeftWidget.PossibleValues = PossibleWidgets(WidgetPositionEnum.Left);
            MainSettings.Settings.RightWidget.PossibleValues = PossibleWidgets(WidgetPositionEnum.Right);
            MainSettings.Settings.BotWidget.PossibleValues = PossibleWidgets(WidgetPositionEnum.Bot);
            MainSettings.Settings.SleepWidget.PossibleValues = PossibleSleepWidgets();
        }

        /// <summary>
        /// Set main message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="duration">Display duration</param>
        internal void SetMainMessage(string message, int duration)
        {
            MainMessageOpacity = 0f;
            MainMessage = message;
            _mainMessageAnnimation.FadeInAndOut(2000, duration);
        }

        /// <summary>
        /// Add volume
        /// </summary>
        /// <param name="delta">Volume to add, in percent</param>
        internal void AddVolume(int delta)
        {
            int lvl = _soundLevel + delta;
            lvl = lvl > 100 ? 100 : lvl;
            lvl = lvl < 0 ? 0 : lvl;
            SoundLevel = lvl;

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = AppDomain.CurrentDomain.BaseDirectory + "/Tools/nircmd.exe",
                Arguments = "setsysvolume " + ((int)(0xFFFF * (float)_soundLevel / 100f)).ToString(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process processTemp = new Process
            {
                StartInfo = startInfo
            };

            try
            {
                processTemp.Start();
            }
            catch (Exception ex)
            {
                LogManager.LogLine(ex.Message);
            }

            foreach (IWidget widget in WidgetList)
            {
                (widget.FullWidget.DataContext as ISoundManageable)?.SetSoundVolume(_soundLevel);
            }
        }

        #endregion

        #region Private methodes

        /// <summary>
        /// Lists all possible widgets for a position
        /// </summary>
        /// <param name="pos">Widgets osition</param>
        /// <returns>Possible widgets</returns>
        private List<string> PossibleWidgets(WidgetPositionEnum pos)
        {
            List<string> ret = new List<string>() { "None" };

            foreach (IWidget widget in WidgetList)
            {
               if(widget.WingetPossiblePosition.Contains(pos))
               {
                    ret.Add(widget.Name);
               }
            }

            return ret;
        }

        /// <summary>
        /// List all possible widgets for sleep
        /// </summary>
        /// <returns></returns>
        private List<string> PossibleSleepWidgets()
        {
            List<string> ret = new List<string>() { "None" };

            foreach (IWidget widget in WidgetList)
            {
                if (widget.CanShowOnSleep)
                {
                    ret.Add(widget.Name);
                }
            }

            return ret;
        }

        #endregion
    }
}