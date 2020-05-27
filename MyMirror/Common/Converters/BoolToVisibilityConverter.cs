// -----------------------------------------------------------------------
// <copyright file="BoolToVisibilityConverter.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class BoolToVisibilityConverter</summary>
// -----------------------------------------------------------------------
namespace Common.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts bool to visibility
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        #region Public methode

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}