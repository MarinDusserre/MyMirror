// -----------------------------------------------------------------------
// <copyright file="TramModel.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class TramModel</summary>
// -----------------------------------------------------------------------

namespace TramWidget.Model
{
    using TramWidget.Properties;
    using Newtonsoft.Json;
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
    /// Contains Tram widget model
    /// </summary>
    internal class TramModel : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Gets windget settings
        /// </summary>
        public SettingsManager<TramSettings> SettingsManager { get; internal set; }

        /// <summary>
        /// Gets line 1 name
        /// </summary>
        public string Line1 { get => _line1; private set => Set(ref _line1, value); } 
        
        /// <summary>
        /// Gets line 2 name
        /// </summary>
        public string Line2 { get => _line2; private set => Set(ref _line2, value); }

        /// <summary>
        /// Gets line 1 direction
        /// </summary>
        public List<string> DirectionLine1 { get => _directionLine1; private set => Set(ref _directionLine1, value); }

        /// <summary>
        /// Gets line 2 direction
        /// </summary>
        public List<string> DirectionLine2 { get => _directionLine2; private set => Set(ref _directionLine2, value); }

        /// <summary>
        /// Gets line 1 - 1
        /// </summary>
        public List<string> NextTram1 { get => _nextTram1; private set => Set(ref _nextTram1, value); }

        /// <summary>
        /// Gets line 1 - 2
        /// </summary>
        public List<string> NextTram2 { get => _nextTram2; private set => Set(ref _nextTram2, value); }

        /// <summary>
        /// Gets line 2 - 1
        /// </summary>
        public List<string> NextTram3 { get => _nextTram3; private set => Set(ref _nextTram3, value); }

        /// <summary>
        /// Gets line 2 - 2
        /// </summary>
        public List<string> NextTram4 { get => _nextTram4; private set => Set(ref _nextTram4, value); }

        #endregion

        #region Private members

        /// <summary>
        /// Refresh timer
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Line 1 name
        /// </summary>
        private string _line1; 
        
        /// <summary>
        /// Line 1 name
        /// </summary>
        private string _line2;

        /// <summary>
        /// Api link Line1 - 1
        /// </summary>
        private string _link1Line1;

        /// <summary>
        /// Api link Line1 - 2
        /// </summary>
        private string _link2Line1;

        /// <summary>
        /// Api link Line2 - 1
        /// </summary>
        private string _link1Line2;

        /// <summary>
        /// Api link Line2 - 2
        /// </summary>
        private string _link2Line2;

        /// <summary>
        /// Tram Line1 direction
        /// </summary>
        private List<string> _directionLine1;

        /// <summary>
        /// Tram Line2 direction
        /// </summary>
        private List<string> _directionLine2;

        /// <summary>
        /// Tram timers Line1 - 1
        /// </summary>
        private List<string> _nextTram1;

        /// <summary>
        /// Tram timers Line1 - 2
        /// </summary>
        private List<string> _nextTram2;

        /// <summary>
        /// Tram timers Line2 - 1
        /// </summary>
        private List<string> _nextTram3;

        /// <summary>
        /// Tram timers Line2 - 2
        /// </summary>
        private List<string> _nextTram4;

        #endregion

        #region Contructors

        /// <summary>
        /// Default constructeur
        /// </summary>
        public TramModel()
        {
            SettingsManager = new SettingsManager<TramSettings>();
            SettingsManager.Initialize(Resources.SettingsFileName);

            string address = SettingsManager.Settings.LinkAddress.Value;
            string tram1 = SettingsManager.Settings.Tram1.Value;
            string tram2 = SettingsManager.Settings.Tram2.Value;
            string tram3 = SettingsManager.Settings.Tram3.Value;
            string tram4 = SettingsManager.Settings.Tram4.Value;

            Line1 = SettingsManager.Settings.Line1.Value;
            Line2 = SettingsManager.Settings.Line2.Value;

            _link1Line1 = string.Format(address, tram1);
            _link2Line1 = string.Format(address, tram2);
            _link1Line2 = string.Format(address, tram3);
            _link2Line2 = string.Format(address, tram4);

            DirectionLine1 = DirectionLine2 = new List<string>() { string.Empty, string.Empty };

            NextTram1 = NextTram2 = NextTram3 = NextTram4 = new List <string> { Resources.DefaultTramText, Resources.DefaultTramText };

            _timer = new Timer(SettingsManager.Settings.TramPullFrequency.Value)
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
                    XmlNode dataNode;

                    // Get tram line 1 direction 1
                    string direction1 = null;
                    List<string> nextTram = null;
                    dataNode = GetData(_link1Line1);
                    ParseData(dataNode, ref direction1, ref nextTram);
                    NextTram1 = nextTram;

                    // Get tram line 1 direction 2
                    string direction2 = null;
                    dataNode = GetData(_link2Line1);
                    ParseData(dataNode, ref direction2, ref nextTram);
                    NextTram2 = nextTram;
                    DirectionLine1 = new List<string> { direction1, direction2 };

                    // Get tram line 2 direction 1
                    dataNode = GetData(_link1Line2);
                    ParseData(dataNode, ref direction1, ref nextTram);
                    NextTram3 = nextTram;

                    // Get tram line 2 direction 2
                    dataNode = GetData(_link2Line2);
                    ParseData(dataNode, ref direction2, ref nextTram);
                    NextTram4 = nextTram;
                    DirectionLine2 = new List<string> { direction1, direction2 };
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
        /// Extract data from XML
        /// </summary>
        /// <param name="dataNode">XML node</param>
        /// <param name="direction">Read direction</param>
        /// <param name="nextTram">Read tram times</param>
        private void ParseData(XmlNode dataNode, ref string direction, ref List<string> nextTram)
        {
            direction = Resources.DefaultTramText;
            nextTram = new List<string> { Resources.DefaultTramText, Resources.DefaultTramText };

            if (dataNode != null)
            {
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                Encoding utf8 = Encoding.UTF8;

                // Next 
                XmlNodeList nodes = dataNode.SelectNodes("root/Row");

                bool firstNode = true;


                List<int> firsts = new List<int> { -1, -1 };
                foreach (XmlNode node in nodes)
                {
                    XmlNodeList nds = node.SelectNodes("times");

                    if (nds.Count > 1)
                    {
                        if (firstNode)
                        {
                            byte[] utfBytes = utf8.GetBytes(node.SelectSingleNode("pattern/desc").InnerXml);
                            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                            direction = utf8.GetString(isoBytes);
                            firsts = new List<int> { XmlConvert.ToInt32(nds[0].SelectSingleNode("realtimeArrival").InnerText),
                                                            XmlConvert.ToInt32(nds[1].SelectSingleNode("realtimeArrival").InnerText)};

                            firstNode = false;
                        }
                        else if (XmlConvert.ToInt32(nds[0].SelectSingleNode("realtimeArrival").InnerText) < firsts[0])
                        {
                            byte[] utfBytes = utf8.GetBytes(node.SelectSingleNode("pattern/desc").InnerXml);
                            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                            direction = utf8.GetString(isoBytes);

                            if (XmlConvert.ToInt32(nds[1].SelectSingleNode("realtimeArrival").InnerText) < firsts[0])
                            {
                                firsts[1] = XmlConvert.ToInt32(nds[1].SelectSingleNode("realtimeArrival").InnerText);
                            }
                            else
                            {
                                firsts[1] = firsts[0];
                            }
                            firsts[0] = XmlConvert.ToInt32(nds[0].SelectSingleNode("realtimeArrival").InnerText);
                        }
                        else if (XmlConvert.ToInt32(nds[0].SelectSingleNode("realtimeArrival").InnerText) < firsts[1])
                        {
                            firsts[1] = XmlConvert.ToInt32(nds[0].SelectSingleNode("realtimeArrival").InnerText);
                        }
                    }
                }

                nextTram = new List<string>
                        {
                            (firsts[0] == -1) ? Resources.DefaultTramText : SecToString(firsts[0]),
                            (firsts[1] == -1) ? Resources.DefaultTramText : SecToString(firsts[1])
                        };
            }
        }

        /// <summary>
        /// Gets XML data on web
        /// </summary>
        /// <param name="link">Data adress</param>
        /// <returns>XML data</returns>
        private XmlNode GetData(string link)
        {
            XmlNode xmlDoc = new XmlDocument();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WebClient wc = new WebClient();
                string json = wc.DownloadString(link);

                wc.Dispose();
                xmlDoc = JsonConvert.DeserializeXmlNode("{\"Row\":" + json + "}", "root");
            }
            catch (Exception ex)
            {
                LogManager.LogLine(ex.Message);
                xmlDoc = null;
            }

            return xmlDoc;
        }

        /// <summary>
        /// Converts integer sec in time string
        /// </summary>
        /// <param name="sec">Seconds</param>
        /// <returns>Returned string</returns>
        private string SecToString(int sec)
        {
            int delay = (sec - (DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second)) / 60;

            delay = delay == 0 ? 1 : delay;

            string delayString = delay.ToString() + " min";

            return delayString;
        }

        #endregion
    }
}
