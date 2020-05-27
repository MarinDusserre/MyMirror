// -----------------------------------------------------------------------
// <copyright file="SettingItemBase.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class SettingItemBase</summary>
// -----------------------------------------------------------------------

namespace Common.Settings
{
    using Common.ViewModel;
    using System;

    /// <summary>
    /// Contains settings item
    /// </summary>
    public abstract class SettingItemBase<T> : ObservableObject, ISettingItemBase
    {
        #region Properties

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string Translation { get; set; }

        /// <summary>
        /// Gets or set settings value
        /// </summary>
        public T Value { get; set; }

        /// <inheritdoc />
        public abstract string StringValue { get; set; }

        /// <inheritdoc />
        public abstract PamameterValueType DisplayType { get;}

        #endregion

        #region Public methodes

        /// <inheritdoc />
        public abstract void InitializeFields(Type Resources);

        #endregion
    }
}
