// -----------------------------------------------------------------------
// <copyright file="ListSettingItem.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class ListSettingItem</summary>
// -----------------------------------------------------------------------

namespace Common.Settings.Items
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Setting item of type List<string>
    /// </summary>
    public class ListSettingItem : SettingItemBase<string>
    {
        #region Properties

        /// <inheritdoc />
        public override PamameterValueType DisplayType => PamameterValueType.List;

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

        /// </summary>
        /// Gets or set settings possible value
        /// </summary>
        public List<string> PossibleValues { get; set; }

        /// <summary>
        /// Gets or sets value index
        /// </summary>
        public int ValueIndex
        {
            get => PossibleValues?.IndexOf(Value) ?? -1;
            set
            {
                if (PossibleValues != null)
                {
                    if (value > -1 && value < PossibleValues.Count)
                    {
                        StringValue = PossibleValues[value];
                    }
                }
            }
        }

        #endregion

        #region Public methode

        /// <inheritdoc />
        public override void InitializeFields(Type Resources)
        {

        }

        #endregion
    }
}
