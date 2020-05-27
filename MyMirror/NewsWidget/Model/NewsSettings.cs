// -----------------------------------------------------------------------
// <copyright file="NewsSettings.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class NewsSettings</summary>
// -----------------------------------------------------------------------


namespace NewsWidget.Model
{
    using System;
    using Common.Settings;
    using Common.Settings.Items;
    using NewsWidget.Properties;

    /// <summary>
    /// Manage application main settings
    /// </summary>
    public class NewsSettings : SettingBase
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the news pull frequency
        /// </summary>
        public IntegerSettingItem NewsPullFrequency { get; set; }

        /// <summary>
        /// Gets or Sets the news refresh frequency
        /// </summary>
        public IntegerSettingItem NewsRefreshFrequency { get; set; }

        /// <summary>
        /// Gets or Sets the news feed url
        /// </summary>
        public StringSettingItem NewsFeedUrl { get; set; }

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