// -----------------------------------------------------------------------
// <copyright file="LeapMotionSettings.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class LeapMotionSettings</summary>
// -----------------------------------------------------------------------

namespace LeapMotionInput
{
    using System;
    using Common.Settings;
    using Common.Settings.Items;
    using global::LeapMotionInput.Properties;

    /// <summary>
    /// LeapMotion input settings
    /// </summary>
    public class LeapMotionSettings : SettingBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the leamotion refresh periode
        /// </summary>
        public IntegerSettingItem RefreshPeriode { get; set; }

        /// <summary>
        /// Gets or sets the screen X size
        /// </summary>
        public IntegerSettingItem ScreenX { get; set; }

        /// <summary>
        /// Gets or sets the screen Y size
        /// </summary>
        public IntegerSettingItem ScreenY { get; set; }

        /// <summary>
        /// Gets or sets the screen height
        /// </summary>
        public IntegerSettingItem ScreenHeight { get; set; }

        /// <summary>
        /// Gets or sets the delay between actions
        /// </summary>
        public IntegerSettingItem DelayBetweenActions { get; set; }

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