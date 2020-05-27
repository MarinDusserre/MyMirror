// -----------------------------------------------------------------------
// <copyright file="SpotifySettings.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class SpotifySettings</summary>
// -----------------------------------------------------------------------


namespace SpotifyWidget.Model
{
    using System;
    using Common.Settings;
    using Common.Settings.Items;
    using SpotifyWidget.Properties;

    /// <summary>
    /// Manage application main settings
    /// </summary>
    public class SpotifySettings : SettingBase
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the cient Id
        /// </summary>
        public StringSettingItem ClientId { get; set; }

        /// <summary>
        /// Gets or Sets the user Id
        /// </summary>
        public StringSettingItem UserId { get; set; }

        /// <summary>
        /// Gets or Sets the user Id
        /// </summary>
        public StringSettingItem OtherPlayLists { get; set; }

        #endregion

        #region Public methode

        /// <inheritdoc />
        protected override Type GetResources()
        {
            return typeof(Resources);
        }

        #endregion
    }
}