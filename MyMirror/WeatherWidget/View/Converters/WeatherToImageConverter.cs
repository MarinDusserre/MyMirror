// -----------------------------------------------------------------------
// <copyright file="WeatherToImageConverter.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class WeatherToImageConverter</summary>
// -----------------------------------------------------------------------

namespace WeatherWidget.View.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Detects null or empty string and covert them to "Charging"
    /// </summary>
    internal class WeatherToImageConverter : IValueConverter
    {
        #region Public methodes

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WeathersEnum? weather = (WeathersEnum)value;

            BitmapImage ret = null;

            if (weather == null)
            {
                ret = new BitmapImage(new Uri($"pack://application:,,,/WeatherWidget;component/Images/Unknown.png"));
            }
            else
            {
                ret = new BitmapImage(new Uri($"pack://application:,,,/WeatherWidget;component/Images/{weather.ToString()}.png"));
            }

            return ret;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}