// -----------------------------------------------------------------------
// <copyright file="TimeSettings.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class TimeSettings</summary>
// -----------------------------------------------------------------------

namespace TimeWidget.Model
{
    using System;
    using Common.Settings;
    using TimeWidget.Properties;

    /// <summary>
    /// Manage application main settings
    /// </summary>
    public class TimeSettings : SettingBase
    {
        #region Public methodes

        /// <inheritdoc />
        protected override Type GetResources()
        {
            return typeof(Resources);
        }

        #endregion
    }
}