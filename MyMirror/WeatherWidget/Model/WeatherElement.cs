// -----------------------------------------------------------------------
// <copyright file="WeatherElement.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class WeatherElement</summary>
// -----------------------------------------------------------------------

namespace WeatherWidget.Model
{
    using Common.ViewModel;

    /// <summary>
    /// Weather element object
    /// </summary>
    internal class WeatherElement : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Gets or sets temperature
        /// </summary>
        public string Temperature { get => _temperature; set => Set(ref _temperature, value); }

        /// <summary>
        /// Gets or sets hour
        /// </summary>
        public string Hour { get => _hour; set => Set(ref _hour, value); }

        /// <summary>
        /// Gets or sets day
        /// </summary>
        public string Day { get => _day; set => Set(ref _day, value); }

        /// <summary>
        /// Gets or sets weather
        /// </summary>
        public WeathersEnum Weather { get => _weather; set => Set(ref _weather, value); }

        #endregion

        #region Private members

        /// <summary>
        /// Temperature
        /// </summary>
        private string _temperature;

        /// <summary>
        /// Hour
        /// </summary>
        private string _hour;

        /// <summary>
        /// Day
        /// </summary>
        private string _day;

        /// <summary>
        /// Weather
        /// </summary>
        private WeathersEnum _weather;

        #endregion
    }
}