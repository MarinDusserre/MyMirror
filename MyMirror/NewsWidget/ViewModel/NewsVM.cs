// -----------------------------------------------------------------------
// <copyright file="NewsVM.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class NewsVM</summary>
// -----------------------------------------------------------------------

namespace NewsWidget.ViewModel
{
    using NewsWidget.Model;
    using Common.ViewModel;
    using Common.Settings;
    using NewsWidget.View;
    using System.Windows;

    /// <summary>
    /// Contains Niews widget view model
    /// </summary>
    internal class NewsVM : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// Gets news Model
        /// </summary>
        public NewsModel NewsModel { get; private set; }

        /// <summary>
        /// News settings
        /// </summary>
        public ISettingsBase Settings { get; internal set; }

        /// <summary>
        /// Gets the visibility of next arow
        /// </summary>
        public bool ShowNextArrow
        {
            get => _showNextArrow;
            private set => Set(ref _showNextArrow, value);
        }

        /// <summary>
        /// Gets the visibility of previous arow
        /// </summary>
        public bool ShowPreviousArrow
        {
            get => _showPreviousArrow;
            private set => Set(ref _showPreviousArrow, value);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Gets the visibilit of next arow
        /// </summary>
        private bool _showNextArrow;

        /// <summary>
        /// Gets the visibilit of previous arow
        /// </summary>
        private bool _showPreviousArrow;

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public NewsVM()
        {
            NewsModel = new NewsModel();
            ShowNextArrow = true;
            ShowPreviousArrow = false;
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Initialize model
        /// </summary>
        public void Initialize()
        {
            NewsModel.Initialize();
        }

        /// <summary>
        /// Handles inout click
        /// </summary>
        /// <param name="xPos">Click X pos</param>
        /// <param name="yPos">Click Y pos</param>
        /// <param name="fullWidget">Widget ref</param>
        public void InputClick(int xPos, int yPos, NewsWidgetFull fullWidget)
        {
            double margin = 0.12;
            double targetPos = 0;
            int size = 142;

            if (xPos > Application.Current.MainWindow.ActualWidth * (1 - margin))
            {
                targetPos = SizeDict[size];
            }
            else if (xPos < Application.Current.MainWindow.ActualWidth * margin)
            {
                targetPos = - SizeDict[size];
            }

            if (targetPos != 0)
            {
                targetPos += fullWidget.ScrollContainer.HorizontalOffset;
                targetPos -= (int)targetPos % (int)SizeDict[size];
                fullWidget.OnScrollClick((int)targetPos);
                ShowPreviousArrow = targetPos > 0;
                ShowNextArrow = targetPos < fullWidget.ScrollContainer.ScrollableWidth;
            }
        }

        #endregion
    }
}
