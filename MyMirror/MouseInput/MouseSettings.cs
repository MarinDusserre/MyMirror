// -----------------------------------------------------------------------
// <copyright file="MouseSettings.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class MouseSettings</summary>
// -----------------------------------------------------------------------

namespace MouseInput
{
    using System;
    using Common.Settings;
    using Common.Settings.Items;
    using global::MouseInput.Properties;

    /// <summary>
    /// Mouse input settings
    /// </summary>
    public class MouseSettings : SettingBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the mouse refresh periode
        /// </summary>
        public IntegerSettingItem RefreshPeriode { get; set; }

        #endregion

        #region Protected functions

        /// <inheritdoc />
        protected override Type GetResources()
        {
            return typeof(Resources);
        }

        #endregion
    }
}