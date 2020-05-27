// -----------------------------------------------------------------------
// <copyright file="IntToTimeString.cs">
//
// </copyright>
// <summary>Convert int time to string</summary>
// -----------------------------------------------------------------------

namespace SpotifyWidget.View.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Convert int time to string
    /// </summary>
    internal class IntToTimeString : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int sec = (int)value;
            string ret;

            ret = (sec / 60).ToString() + ":";
            if (sec % 60 < 10)
            {
                ret += "0";
            }
            ret += (sec % 60).ToString();

            return ret;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
