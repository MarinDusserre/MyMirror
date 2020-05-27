// -----------------------------------------------------------------------
// <copyright file="WeatherModel.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class WeatherModel</summary>
// -----------------------------------------------------------------------

namespace WeatherWidget.Model
{
    using Newtonsoft.Json;
    using WeatherWidget.Properties;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Timers;
    using System.Xml;
    using Common.ViewModel;
    using Common.Log;
    using Common.Settings;

    /// <summary>
    /// Contains weather widget model
    /// </summary>
    internal class WeatherModel : ObservableObject
    {
        #region Properties
        /// <summary>
        /// <summary>
        /// Gets windget settings
        /// </summary>
        public SettingsManager<WeatherSettings> SettingsManager { get; internal set; }

        /// <summary>
        /// Gets weather forecast
        /// </summary>
        public List<WeatherElement> WeatherForcast { get => _weatherForcast; private set => Set(ref _weatherForcast, value); }

        #endregion

        #region Private members

        /// <summary>
        /// Refresh timer
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Api link
        /// </summary>
        private string _link;

        /// <summary>
        /// Associates wheater name to weather enum value
        /// </summary>
        private Dictionary<string, WeathersEnum> _weatherConverter = new Dictionary<string, WeathersEnum>
        {
            {"Thunderstorm",  WeathersEnum.Thunderstorm},
            {"Drizzle",  WeathersEnum.Rain},
            {"Rain",  WeathersEnum.Rain},
            {"Snow",  WeathersEnum.Snow},
            {"Atmosphere",  WeathersEnum.Mist},
            {"Clouds", WeathersEnum.Clouds},
            {"Clear",  WeathersEnum.Sunny}
        };


        /// <summary>
        /// Weather forecast
        /// </summary>
        private List<WeatherElement> _weatherForcast;

        #endregion

        #region Contructors

        /// <summary>
        /// Default constructeur
        /// </summary>
        public WeatherModel()
        {
            SettingsManager = new SettingsManager<WeatherSettings>();
            SettingsManager.Initialize(Resources.SettingsFileName);

            string linkAddress = SettingsManager.Settings.LinkAddress.Value;
            string cityId = SettingsManager.Settings.CityId.Value;
            string apiId = SettingsManager.Settings.ApiId.Value;

            _link = string.Format(linkAddress, cityId, apiId);

            _timer = new Timer(SettingsManager.Settings.PullPeriode.Value * 1000)
            {
                AutoReset = false
            };
            _timer.Elapsed += Refresh;
        }

        #endregion

        #region Public methodes

        /// <summary>
        /// Performs a first update
        /// </summary>
        public void Initialize()
        {
            WeatherForcast = new List<WeatherElement>() { new WeatherElement() { Weather = WeathersEnum.Unknown } };
            Refresh(null, null);
        }

        #endregion

        #region Private methodes

        /// <summary>
        /// Updates tram timers
        /// </summary>
        /// <param name="s">Sender</param>
        /// <param name="e">Arguments</param>
        private void Refresh(object s, ElapsedEventArgs e)
        {
            new Task(() =>
            {
                try
                {
                    XmlNode dataNode = GetData();

                    if (dataNode != null)
                    {
                        StringBuilder rssContent = new StringBuilder();

                        XmlNodeList nodes = dataNode.SelectNodes("list/list");
                        List<WeatherElement> forcast = new List<WeatherElement>();

                        double delta = 0;
                        int num = 0;
                        DateTime dateTime = DateTime.Now;

                        foreach (XmlNode node in nodes)
                        {
                            WeatherElement weather = new WeatherElement();
                            
                            if (nodes[0].SelectSingleNode("main/temp") != null)
                            {
                                // Get day
                                string[] fullTime = node.SelectSingleNode("dt_txt").InnerXml.Split(' ');
                                string[] date = fullTime[0].Split('-');
                                string[] time = fullTime[1].Split(':');

                                dateTime = new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]), int.Parse(time[0]), int.Parse(time[1]), int.Parse(time[2]));
                                weather.Day = typeof(Resources).GetProperty(dateTime.DayOfWeek.ToString())?.GetValue(null) as string ?? dateTime.DayOfWeek.ToString();
  
                                // Get time
                                weather.Hour = time[0].ToString() + ":" + time[1].ToString();

                                // Get temperature
                                double kelvin = XmlConvert.ToDouble(node.SelectSingleNode("main/temp").InnerXml);
                                weather.Temperature = KelToCel(kelvin).ToString("0.#") + "°C";

                                // Get weather
                                string currentWeather = node.SelectSingleNode("weather/main").InnerText;
                                weather.Weather = _weatherConverter[currentWeather];
                            }
                            else
                            {
                                weather.Weather = WeathersEnum.Unknown;
                                weather.Temperature = Resources.DefautValue;
                                weather.Hour = Resources.DefautValue;
                                weather.Day = Resources.DefautValue;
                            }
                            
                            delta = (dateTime - DateTime.Now).TotalHours;
                            if (delta >= num * SettingsManager.Settings.WheatherFrequency.Value)
                            {
                                num++;
                                delta = 0;
                                forcast.Add(weather);
                            }
                        }

                        WeatherForcast = forcast;
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogLine(ex.Message);
                }
                _timer.Start();
            }
            ).Start();
        }

        /// <summary>
        /// Gets data on web
        /// </summary>
        /// <returns>XML data read</returns>
        private XmlNode GetData()
        {
            XmlNode xmlDoc = new XmlDocument();
            try
            {
                WebClient wc = new WebClient();
                var json = wc.DownloadString(_link);
                wc.Dispose();
                xmlDoc = JsonConvert.DeserializeXmlNode(json, "list");
            }
            catch (Exception ex)
            {
                LogManager.LogLine(ex.Message);
                xmlDoc = null;
            }

            return xmlDoc;
        }

        /// <summary>
        /// Convert Kelvin to celcius
        /// </summary>
        /// <param name="kel">Kelvin value</param>
        /// <returns>Celcius value</returns>
        private double KelToCel(double kel)
        {
            return (kel - 273.15);
        }

        #endregion
    }
}