// -----------------------------------------------------------------------
// <copyright file="NewsWidgetFull.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class NewsWidgetFull</summary>
// -----------------------------------------------------------------------

namespace NewsWidget.View
{
    using Common.Annimations;
    using System.Windows.Controls;

    /// <summary>
    /// News window view full
    /// </summary>
    public partial class NewsWidgetFull : UserControl
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
        public NewsWidgetFull()
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