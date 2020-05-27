// -----------------------------------------------------------------------
// <copyright file="IntToTimeString.cs">
//
// </copyright>
// <summary>Convert float progress to size</summary>
// -----------------------------------------------------------------------

namespace SpotifyWidget.View.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Convert int time to string
    /// </summary>
    internal class FloatToSizeProgress : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double ret = 0;
            float coef = (float)value;

            if(int.TryParse((string)parameter, out int maxSize))
            {
                ret = Application.Current.MainWindow.ActualHeight * coef * maxSize / 100f;
            }

            return ret;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}