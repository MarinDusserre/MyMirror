// -----------------------------------------------------------------------
// <copyright file="TramWidget.cs">
//
// </copyright>
// <summary>Contains Tram widget interface implementation</summary>
// -----------------------------------------------------------------------

namespace TramWidget
{
    using TramWidget.View;
    using TramWidget.ViewModel;
    using System;
    using System.Windows.Controls;
    using WingetContract;
    using WingetContract.Enum;
    using System.Collections.Generic;
    using Common.Settings;
    using Common.Enums;

    /// <summary>
    /// Contains Tram widget interface implementation
    /// </summary>
    public class WidgetTram : IWidget
    {
        #region Properties

        /// <inheritdoc />
        public string Name => "Tram";

        /// <inheritdoc />
        public List<WidgetPositionEnum> WingetPossiblePosition => new List<WidgetPositionEnum>()
        {
            WidgetPositionEnum.Right,
            WidgetPositionEnum.Left
        };

        /// <inheritdoc />
        public ISettingsBase Settings => _dataContext.TramModel.SettingsManager.Settings;

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
        /// Widgets view model
        /// </summary>
        private TramVM _dataContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public WidgetTram()
        {
            _fullWidget = new TramWidgetFull();
            _rightLeftWidget = new TramWidgetReduce();
            _topBotWidget = new TramWidgetReduce();

            _dataContext = (TramVM)_fullWidget.DataContext;

            _topBotWidget.DataContext = _dataContext;
            _rightLeftWidget.DataContext = _dataContext;
        }

        #endregion

        #region Public methodes

        /// <inheritdoc />
        public void Initialize()
        {
            _dataContext.Initialize();
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
