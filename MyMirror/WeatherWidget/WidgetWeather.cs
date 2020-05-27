// -----------------------------------------------------------------------
// <copyright file="WidgetWeather.cs">
//
// </copyright>
// <summary>Contains Tram widget interface implementation</summary>
// -----------------------------------------------------------------------

namespace WeatherWidget
{
    using WeatherWidget.View;
    using WeatherWidget.ViewModel;
    using System;
    using System.Windows.Controls;
    using WingetContract;
    using WingetContract.Enum;
    using System.Collections.Generic;
    using Common.Settings;
    using System.Windows;
    using Common.Enums;

    /// <summary>
    /// Contains Tram widget interface implementation
    /// </summary>
    public class WidgetWeather : IWidget
    {
        #region Properties

        /// <inheritdoc />
        public List<WidgetPositionEnum> WingetPossiblePosition => new List<WidgetPositionEnum>()
        {
            WidgetPositionEnum.Right,
            WidgetPositionEnum.Left
        };

        /// <inheritdoc />
        public string Name => "Wheather";

        /// <inheritdoc />
        public ISettingsBase Settings => _dataContext.WeatherModel.SettingsManager.Settings;

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
        private WeatherVM _dataContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public WidgetWeather()
        {
            _fullWidget = new WeatherWidgetFull();
            _rightLeftWidget = new WeatherWidgetReduce();
            _topBotWidget = new WeatherWidgetReduce();

            _dataContext = (WeatherVM)_fullWidget.DataContext;

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
            if (gesture == InputGestureEnum.Click)
            {
                _dataContext.InputClick(xPos, yPos, ((WeatherWidgetFull)FullWidget));
            }
        }
        #endregion
    }
}