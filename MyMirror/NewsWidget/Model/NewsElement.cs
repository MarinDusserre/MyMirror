// -----------------------------------------------------------------------
// <copyright file="NewsElement.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class NewsElement</summary>
// -----------------------------------------------------------------------

namespace NewsWidget.Model
{
    using Common.ViewModel;

    /// <summary>
    /// News Element
    /// </summary>
    public class NewsElement : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Gets title of the news
        /// </summary>
        public string Title { get => _title; private set => Set(ref _title, value); }

        /// <summary>
        /// Gets text of the news
        /// </summary>
        public string Text { get => _text; private set => Set(ref _text, value); }

        #endregion

        #region Private members

        /// <summary>
        /// Title of the information
        /// </summary>
        private string _title;

        /// <summary>
        /// Text of the information
        /// </summary>
        private string _text;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct news with title and test
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public NewsElement(string title, string text)
        {
            Title = title;
            Text = text;
        }

        #endregion
    }
}
