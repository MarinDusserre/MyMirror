// -----------------------------------------------------------------------
// <copyright file="BoolToImagePlayButton.cs">
//
// </copyright>
// <summary>Converts bool to play button image</summary>
// -----------------------------------------------------------------------

namespace SpotifyWidget.View.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Converts bool to play button image
    /// </summary>
    internal class BoolToImagePlayButton : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? new BitmapImage(new Uri($"pack://application:,,,/SpotifyWidget;component/Images/Pause.png")) :
                new BitmapImage(new Uri($"pack://application:,,,/SpotifyWidget;component/Images/Play.png"));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}