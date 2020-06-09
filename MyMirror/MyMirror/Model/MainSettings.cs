// -----------------------------------------------------------------------
// <copyright file="MainSettings.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class MainSettings</summary>
// -----------------------------------------------------------------------

namespace MyMirror.Model
{
    using Common.Settings;
    using Common.Settings.Items;
    using MyMirror.Properties;
    using System;

    /// <summary>
    /// Manage application main settings
    /// </summary>
    public class MainSettings : SettingBase
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the user name
        /// </summary>
        public StringSettingItem UserName { get; set; }

        /// <summary>
        /// Gets or Sets top left widgets
        /// </summary>
        public ListSettingItem TopLeftWidget { get; set; }

        /// <summary>
        /// Gets or Sets top widgets
        /// </summary>
        public ListSettingItem TopWidget { get; set; }

        /// <summary>
        /// Gets or Sets top right widgets
        /// </summary>
        public ListSettingItem TopRightWidget { get; set; }

        /// <summary>
        /// Gets or Sets top widgets
        /// </summary>
        public ListSettingItem LeftWidget { get; set; }

        /// <summary>
        /// Gets or Sets top widgets
        /// </summary>
        public ListSettingItem RightWidget { get; set; }

        /// <summary>
        /// Gets or Sets top widgets
        /// </summary>
        public ListSettingItem BotWidget { get; set; }

        /// <summary>
        /// Gets or Sets sleep widgets
        /// </summary>
        public ListSettingItem SleepWidget { get; set; }

        /// <summary>
        /// Gets or Sets sleep timer
        /// </summary>
        public IntegerSettingItem SleepTimer { get; set; }

        /// <summary>
        /// Gets or Sets leds mode
        /// </summary>
        public StringSettingItem LedsMode { get; set; }

        /// <summary>
        /// Gets or Sets leds port
        /// </summary>
        public StringSettingItem LedsPort { get; set; }

        #endregion

        #region Public methodes

        /// <inheritdoc />
        protected override Type GetResources()
        {
            return typeof(Resources);
        }

        #endregion
    }
}
