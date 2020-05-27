// -----------------------------------------------------------------------
// <copyright file="TramSettings.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class TramSettings</summary>
// -----------------------------------------------------------------------

namespace TramWidget.Model
{
    using System;
    using Common.Settings;
    using Common.Settings.Items;
    using TramWidget.Properties;

    /// <summary>
    /// Manage application main settings
    /// </summary>
    public class TramSettings : SettingBase
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the tram time pull frequency
        /// </summary>
        public IntegerSettingItem TramPullFrequency { get; set; }

        /// <summary>
        /// Gets or Sets the tram feed URL
        /// </summary>
        public StringSettingItem LinkAddress { get; set; }

        /// <summary>
        /// Gets or Sets the line 1 name
        /// </summary>
        public StringSettingItem Line1 { get; set; }

        /// <summary>
        /// Gets or Sets the tram 1
        /// </summary>
        public StringSettingItem Tram1 { get; set; }

        /// <summary>
        /// Gets or Sets the tram 2
        /// </summary>
        public StringSettingItem Tram2 { get; set; }

        /// <summary>
        /// Gets or Sets the line 2 name
        /// </summary>
        public StringSettingItem Line2 { get; set; }

        /// <summary>
        /// Gets or Sets the tram 3
        /// </summary>
        public StringSettingItem Tram3 { get; set; }

        /// <summary>
        /// Gets or Sets the tram 4
        /// </summary>
        public StringSettingItem Tram4 { get; set; }

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