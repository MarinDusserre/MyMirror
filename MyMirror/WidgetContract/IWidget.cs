// -----------------------------------------------------------------------
// <copyright file="IWidget.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains interface IWidget</summary>
// -----------------------------------------------------------------------

namespace WingetContract
{
    using Common.Enums;
    using Common.Settings;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using WingetContract.Enum;

    /// <summary>
    /// Contains Interface for widget
    /// </summary>
    public interface IWidget
    {
        #region Properties

        /// <summary>
        /// Gets widget name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets widget possible positions
        /// </summary>
        List<WidgetPositionEnum> WingetPossiblePosition { get; }

        /// <summary>
        /// Gets widget view, right or left version
        /// </summary>
        UserControl RightOrLeftWidget { get; }

        /// <summary>
        /// Gets widget view, top or bot version
        /// </summary>
        UserControl TopOrBotWidget { get; }

        /// <summary>
        /// Gets widget view, full version
        /// </summary>
        UserControl FullWidget { get; }

        /// <summary>
        /// Gets a value indicating wheather the widget can be shown on sleep
        /// </summary>
        bool CanShowOnSleep { get; }

        /// <summary>
        /// Gets widget setting manager
        /// </summary>
        ISettingsBase Settings { get; }

        #endregion

        #region Public functions

        /// <summary>
        /// Initializes widget
        /// </summary>
        void Initialize();

        /// <summary>
        /// Disposes widget
        /// </summary>
        void Dispose();

        /// <summary>
        /// Send click input to widget
        /// </summary>
        /// <param name="xPos">Click X pos</param>
        /// <param name="yPos">Click Y pos</param>
        /// <param name="gesture">Gesture</param>
        void InputEvent(int xPos, int yPos, InputGestureEnum gesture);

        #endregion
    }
}
