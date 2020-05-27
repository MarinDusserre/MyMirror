// -----------------------------------------------------------------------
// <copyright file="IntToFigureSoundConverter.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains enum IntToFigureSoundConverter</summary>
// -----------------------------------------------------------------------

namespace MyMirror.View.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Converts int sound level to figure
    /// </summary>
    internal class IntToFigureSoundConverter : IValueConverter
    {
        #region Public functions

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int percent = (int)value;
            double screenX = Application.Current.MainWindow.ActualWidth;
            double screenY = Application.Current.MainWindow.ActualHeight;

            double width = screenY / 10 ;
            double outerRadius = screenY / 2;
            double innerRadius = outerRadius - width;

            percent = percent == 100 ? 99 : percent;
            percent = percent == 0 ? 1 : percent;

            float angle = (float)Math.PI * (50 - percent) / 50f;

            PathFigureCollection ret = new PathFigureCollection();

            PathFigure figure = new PathFigure
            {
                StartPoint = new Point((screenX / 2) - outerRadius, screenY / 2)
            };

            PathSegmentCollection pathSegment = new PathSegmentCollection
            {
                new ArcSegment(
                    new Point(screenX / 2 + Math.Cos(angle) * outerRadius, screenY / 2 - Math.Sin(angle) * outerRadius ),
                    new Size(outerRadius, outerRadius),
                    angle,
                    percent > 50, 
                    SweepDirection.Clockwise, 
                    false),
                new LineSegment(
                    new Point(screenX / 2 + Math.Cos(angle) * innerRadius, screenY / 2 - Math.Sin(angle) * innerRadius ), 
                    false),
                new ArcSegment(                    
                    new Point((screenX / 2) - innerRadius, screenY / 2),
                    new Size(innerRadius, innerRadius),
                    angle,
                    percent > 50,
                    SweepDirection.Counterclockwise,
                    false),
            };

            figure.Segments = pathSegment;

            ret.Add(figure);
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