// -----------------------------------------------------------------------
// <copyright file="WifiLedsManager.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class WifiLedsManager</summary>
// -----------------------------------------------------------------------

namespace MyMirror.Model.Led
{
    using Common.Log;
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using WingetContract.Enum;

    public class WifiLedsManager : ILedManager
    {
        #region private members

        /// <summary>
        /// API access mutex
        /// </summary>
        private readonly string _ipAddress;

        /// <summary>
        /// API access mutex
        /// </summary>
        private readonly Mutex _accessMutex;

        #endregion

        #region constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_ipAddress">IP address</param>
        public WifiLedsManager(string ipAddress)
        {
            _accessMutex = new Mutex();
            _ipAddress = ipAddress;
        }

        #endregion

        #region public methodes

        /// <inheritdoc />
        public void InitConnexion()
        {

        }

        /// <inheritdoc />
        public void InitAnnimation()
        {
            string parameters = WifiRequestNameEnum.Request.ToString() + "=" + WifiRequestNameEnum.Initialization.ToString();
            parameters += "&" + WifiRequestNameEnum.DiplayDuration.ToString() + "=" + 2000;
            SendRequest(parameters);
        }

        /// <inheritdoc />
        public void LightUpAllWhite(uint fadeInDuration, uint diplayDuration, uint fadeOutDuration)
        {
            LightUpSideWhite(WidgetPositionEnum.Center, fadeInDuration, diplayDuration, fadeOutDuration);
        }

        /// <inheritdoc />
        public void LightUpSideWhite(WidgetPositionEnum side, uint fadeInDuration, uint diplayDuration, uint fadeOutDuration)
        {
            string parameters = WifiRequestNameEnum.Request.ToString() + "=";
            bool found = true;

            switch (side)
            {
                case (WidgetPositionEnum.Center):
                    {
                        parameters += WifiRequestNameEnum.ShowAll.ToString();
                        break;
                    }
                case (WidgetPositionEnum.Top):
                    {
                        parameters += WifiRequestNameEnum.ShowTop.ToString();
                        break;
                    }
                case (WidgetPositionEnum.Right):
                    {
                        parameters += WifiRequestNameEnum.ShowRight.ToString();
                        break;
                    }
                case (WidgetPositionEnum.Bot):
                    {
                        parameters += WifiRequestNameEnum.ShowBot.ToString();
                        break;
                    }
                case (WidgetPositionEnum.Left):
                    {
                        parameters += WifiRequestNameEnum.ShowLeft.ToString();
                        break;
                    }
                default:
                    {
                        found = false;
                        break;
                    }
            }
            if (found)
            {
                parameters += "&" + WifiRequestNameEnum.FadeInDuration.ToString() + "=" + fadeInDuration;
                parameters += "&" + WifiRequestNameEnum.DiplayDuration.ToString() + "=" + diplayDuration;
                parameters += "&" + WifiRequestNameEnum.FadeOutDuration.ToString() + "=" + fadeOutDuration;

                SendRequest(parameters);
            }
        }

        /// <inheritdoc />
        public void LightUpPosWhite(byte xPosPercent, byte yPosPercent, uint duration)
        {
            /*string parameters = WifiRequestNameEnum.Request.ToString() + "=" + WifiRequestNameEnum.Position.ToString();
            parameters += "&" + WifiRequestNameEnum.PositionX.ToString() + "=" + xPosPercent;
            parameters += "&" + WifiRequestNameEnum.PositionY.ToString() + "=" + yPosPercent;
            parameters += "&" + WifiRequestNameEnum.DiplayDuration.ToString() + "=" + duration;
            SendRequest(parameters);*/
        }

        /// <inheritdoc />
        public void SetPartyMode(bool on)
        {
            string parameters = WifiRequestNameEnum.Request.ToString() + "=" + WifiRequestNameEnum.PartyMode.ToString();
            parameters += "&" + WifiRequestNameEnum.PartyMode.ToString() + "=" + (on ? "1" : "0");
            SendRequest(parameters);
        }

        #endregion

        #region private functions

        /// <summary>
        /// Send frame through com port
        /// </summary>
        /// <param name="uri">Uri to reach</param>
        private void SendRequest(string uri)
        {
            string curl = "http://" + _ipAddress + "/leds?" +  uri.ToLower();
            new Task(() =>
            {
                try
                {
                    if (_accessMutex.WaitOne())
                    {
                        WebRequest request = WebRequest.Create(curl);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        response.Close();
                        _accessMutex.ReleaseMutex();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogLine(ex.Message);
                    _accessMutex.ReleaseMutex();
                }
            }).Start();
        }

        #endregion
    }
}