// -----------------------------------------------------------------------
// <copyright file="WidgetTime.cs">
//
// </copyright>
// <summary>Implements widget interface for Time widget</summary>
// -----------------------------------------------------------------------

namespace TimeWidget
{
    using TimeWidget.View;
    using TimeWidget.ViewModel;
    using System;
    using System.Windows.Controls;
    using WingetContract;
    using WingetContract.Enum;
    using System.Collections.Generic;
    using Common.Settings;
    using Common.Enums;

    /// <summary>
    /// Implements widget interface for Time widget
    /// </summary>
    public class WidgetTime : IWidget
    {
        #region Properties

        /// <inheritdoc />
        public string Name => "Time";

        /// <inheritdoc />
        public List<WidgetPositionEnum> WingetPossiblePosition => new List<WidgetPositionEnum>()
        {
            WidgetPositionEnum.TopLeft,
            WidgetPositionEnum.TopRight
        };

        /// <inheritdoc />
        public ISettingsBase Settings => _dataContext.TimeModel.SettingsManager.Settings;

        /// <inheritdoc />
        public UserControl RightOrLeftWidget => _rightLeftWidget;

        /// <inheritdoc />
        public UserControl TopOrBotWidget => _topBotWidget;

        /// <inheritdoc />
        public UserControl FullWidget => _fullWidget;

        /// <inheritdoc />
        public bool CanShowOnSleep => true;

        #endregion

        #region Private members

        /// <summary>
        /// Full widgets
        /// </summary>
        private UserControl _fullWidget;

        /// <summary>
        /// Reduce widgets
        /// </summary>
        private UserControl _rightLeftWidget;

        /// <summary>
        /// Reduce widgets
        /// </summary>
        private UserControl _topBotWidget;

        /// <summary>
        /// Current widget
        /// </summary>
        private TimeVM _dataContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public WidgetTime()
        {
            _fullWidget = new TimeWidgetFull();
            _rightLeftWidget = new TimeWidgetReduce();
            _topBotWidget = new TimeWidgetReduce();

            _dataContext = (TimeVM)_fullWidget.DataContext;

            _topBotWidget.DataContext = _dataContext;
            _rightLeftWidget.DataContext = _dataContext;
        }

        #endregion

        #region Public methodes

        /// <inheritdoc />
        public void Initialize()
        {
            _dataContext.TimeModel.Initialize();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void InputEvent(int xPos, int yPos, InputGestureEnum gesture)
        {
        }

        #endregion
    }
}
