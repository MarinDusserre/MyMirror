// -----------------------------------------------------------------------
// <copyright file="ISettingsBase.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains interface ISettingsBase</summary>
// -----------------------------------------------------------------------

namespace Common.Settings
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base class for settings
    /// </summary>
    public interface ISettingsBase
    {
        #region Events

        /// <summary>
        /// Event raised when settings are updated
        /// </summary>
        event EventHandler<EventArgs> SettingsUpdated;


        #endregion

        #region Methodes

        /// <summary>
        /// Gets all settings
        /// </summary>
        /// <returns>Return all settings</returns>
        List<ISettingItemBase> GetSettingsList();

        /// <summary>
        /// Sets all settings
        /// </summary>
        /// <param name="settings"></param>
        void SetSettingsList(List<ISettingItemBase> settings);

        /// <summary>
        /// Set settings to defaut values
        /// </summary>
        void GenerateDefaultSettings();

        #endregion
    }
}
