// -----------------------------------------------------------------------
// <copyright file="WeatherWidgetFull.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class WeatherWidgetFull</summary>
// -----------------------------------------------------------------------

namespace WeatherWidget.View
{
    using System.Windows.Controls;
    using Common.Annimations;

    /// <summary>
    /// Tram window view full
    /// </summary>
    public partial class WeatherWidgetFull : UserControl
    {
        #region Private members

        /// <summary>
        /// Scroll bar annimator
        /// </summary>
        ScrollBarAnimator _scrollBarAnimator;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WeatherWidgetFull()
        {
            InitializeComponent();
            _scrollBarAnimator = new ScrollBarAnimator(ScrollContainer, Dispatcher);
        }

        #endregion

        #region Public methodes

        /// <summary>
        /// Handles scrollbutton click
        /// </summary>
        /// <param name="xPos">X position of the click</param>
        public void OnScrollClick(int xPos)
        {
            _scrollBarAnimator.ScrollToHorizontalPosition(xPos);
        }

        #endregion
    }
}