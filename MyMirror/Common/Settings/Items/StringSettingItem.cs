// -----------------------------------------------------------------------
// <copyright file="StringSettingItem.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class StringSettingItem</summary>
// -----------------------------------------------------------------------
using System;

namespace Common.Settings.Items
{
    /// <summary>
    /// Setting item of time String
    /// </summary>
    public class StringSettingItem : SettingItemBase<string>
    {
        #region Properties

        /// <inheritdoc />
        public override string StringValue
        {
            get => Value;
            set
            {
                Value = value;
                NotifyPropertyChanged(nameof(Value));
            }
        }

        /// <inheritdoc />
        public override PamameterValueType DisplayType => PamameterValueType.Field;

        #endregion

        #region Public methode

        /// <inheritdoc />
        public override void InitializeFields(Type Resources)
        {
        }

        #endregion
    }
}
