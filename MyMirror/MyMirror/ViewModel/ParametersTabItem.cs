// -----------------------------------------------------------------------
// <copyright file="ParametersTabItem.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class ParametersTabItem</summary>
// -----------------------------------------------------------------------

namespace MyMirror.ViewModel
{
    using Common.Settings;
    using Common.ViewModel;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters view model item in
    /// </summary>
    public class ParametersTabItem : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// Gets tab title
        /// </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        /// <summary>
        /// Gets tab items
        /// </summary>
        public List<ISettingItemBase> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Tab title
        /// </summary>
        private string _title;

        /// <summary>
        /// Tab items
        /// </summary>
        private List<ISettingItemBase> _items;

        #endregion
    }
}
