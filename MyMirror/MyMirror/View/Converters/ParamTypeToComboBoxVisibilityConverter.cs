// -----------------------------------------------------------------------
// <copyright file="ParamTypeToComboBoxVisibilityConverter.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains enum ParamTypeToComboBoxVisibilityConverter</summary>
// -----------------------------------------------------------------------

namespace MyMirror.View.Converters
{
    using Common.Settings;
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts bool to visibility
    /// </summary>
    internal class ParamTypeToComboBoxVisibilityConverter : IValueConverter
    {
        #region Public functions

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PamameterValueType val = (PamameterValueType)value;
            return val == PamameterValueType.List ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}