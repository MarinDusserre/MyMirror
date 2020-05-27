// -----------------------------------------------------------------------
// <copyright file="IScreenInput.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Interface for mirror input</summary>
// -----------------------------------------------------------------------

namespace InputContract
{
    using System;
    using Common.Settings;

    /// <summary>
    /// Interface for mirror input
    /// </summary>
    public interface IScreenInput
    {
        #region Events

        /// <summary>
        /// Input event
        /// </summary>
        event EventHandler<ScreenInputEventArg> ScreenInputEvent;

        #endregion

        #region Properties

        /// <summary>
        /// Gets widget name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets input settings
        /// </summary>
        ISettingsBase Settings { get; }

        #endregion
    }
}