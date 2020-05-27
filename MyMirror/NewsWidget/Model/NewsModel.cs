// -----------------------------------------------------------------------
// <copyright file="NewsModel.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class NewsModel</summary>
// -----------------------------------------------------------------------

namespace NewsWidget.Model
{
    using System;
    using System.Threading.Tasks;
    using System.Timers;
    using System.Threading;
    using Common.ViewModel;
    using Common.Log;
    using System.Collections.Generic;
    using Common.Settings;
    using NewsWidget.Properties;
    using System.Xml;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Contains news widget model
    /// </summary>
    internal class NewsModel : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Gets windget settings
        /// </summary>
        public SettingsManager<NewsSettings> SettingsManager { get; internal set; }

        /// <summary>
        /// News elements
        /// </summary>
        public List<NewsElement> NewsList { get => _newsList; private set => Set(ref _newsList, value); }

        #endregion

        #region Private members

        /// <summary>
        /// Refresh news list timer
        /// </summary>
        private System.Timers.Timer _timer;

        /// <summary>
        /// Switch news timer
        /// </summary>
        private System.Timers.Timer _switchTimer;

        /// <summary>
        /// News list access mutex
        /// </summary>
        private readonly Mutex _accessMutex;

        /// <summary>
        /// List of the news
        /// </summary>
        private List<NewsElement> _newsList;

        /// <summary>
        /// Can rotate news
        /// </summary>
        private bool _rotateNews;

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructeur
        /// </summary>
        public NewsModel()
        {
            SettingsManager = new SettingsManager<NewsSettings>();
            SettingsManager.Initialize(Resources.SettingsFileName);
            _rotateNews = true;

            _timer = new System.Timers.Timer(SettingsManager.Settings.NewsPullFrequency.Value)
            {
                AutoReset = false
            };
            _timer.Elapsed += GetInfo;

            _switchTimer = new System.Timers.Timer(SettingsManager.Settings.NewsRefreshFrequency.Value)
            {
                AutoReset = false
            };
            _timer.Elapsed += Refresh;

            _accessMutex = new Mutex();
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Performs a first update
        /// </summary>
        public void Initialize()
        {
            try
            {
                LogManager.LogLine("News initialization OK");
                GetInfo(null, null);
            }
            catch (Exception ex)
            {
                LogManager.LogLine("News initialization Error");
                LogManager.LogLine(ex.Message);
            }
        }

        #endregion

        #region Private functions

        /// <summary>
        /// Get informations form API
        /// </summary>
        /// <param name="s">Sender</param>
        /// <param name="e">Arguments</param>
        private void GetInfo(object s, ElapsedEventArgs e)
        {
            new Task(() =>
            {
                try
                {
                    if (_accessMutex.WaitOne(1000))
                    {
                        XmlNode dataNode;
                        dataNode = GetData(SettingsManager.Settings.NewsFeedUrl.Value);
                        NewsList = ParseData(dataNode);
                        _accessMutex.ReleaseMutex();
                    }

                    _timer.Start();

                    Refresh(null, null);
                }
                catch (Exception ex)
                {
                    LogManager.LogLine(ex.Message);
                }
            }
            ).Start();
        }
        
        /// <summary>
        /// Updates displayer info
        /// </summary>
        /// <param name="s">Sender</param>
        /// <param name="e">Arguments</param>
        private void Refresh(object s, ElapsedEventArgs e)
        {
            new Task(() =>
            {
                if(_newsList != null && _newsList.Count > 0)
                {
                    if (_accessMutex.WaitOne(1000))
                    {
                        NewsElement first = NewsList[0];
                        NewsList.RemoveAt(0);
                        NewsList.Add(first);
                        _accessMutex.ReleaseMutex();
                    }
                }

                _switchTimer.Start();
            }
            ).Start();
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
                string xmlContent = wc.DownloadString(link);
                wc.Dispose();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);
                xmlDoc = doc.DocumentElement;
            }
            catch (Exception ex)
            {
                LogManager.LogLine(ex.Message);
                xmlDoc = null;
            }

            return xmlDoc;
        }

        /// <summary>
        /// Extract data from XML
        /// </summary>
        /// <param name="dataNode">XML node</param>
        /// <returns>Infos titles</returns>
        private List<NewsElement> ParseData(XmlNode dataNode)
        {
            List<NewsElement> ret = new List<NewsElement>(); ;

            if (dataNode != null)
            {          
                // Next 
                XmlNodeList nodes = dataNode.SelectNodes("channel/item");
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                Encoding utf8 = Encoding.UTF8;

                foreach (XmlNode node in nodes)
                {

                    byte[] utfBytes = utf8.GetBytes(node.SelectNodes("title")[0].InnerXml);
                    byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                    string title = utf8.GetString(isoBytes);

                    utfBytes = utf8.GetBytes(node.SelectNodes("description")[0].InnerXml);
                    isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                    string description = utf8.GetString(isoBytes);
                    description = description.Replace("<![CDATA[", "");
                    description = description.Replace("]]>", "");
                    description = description.Replace("&nbsp;", " ");
                    ret.Add(new NewsElement(title, description));
                }
            }
            return ret;
        }

        #endregion
    }
}