// -----------------------------------------------------------------------
// <copyright file="COMLedManager.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class COMLedManager</summary>
// -----------------------------------------------------------------------

namespace MyMirror.Model.Led
{
    using Common.Log;
    using System;
    using System.IO.Ports;
    using System.Threading;
    using WingetContract.Enum;

    public class COMLedManager : ILedManager
    {
        #region private members

        /// <summary>
        /// Connection time out
        /// </summary>
        private const int CONNECTION_TIME_OUT = 1000;

        /// <summary>
        /// Serial port
        /// </summary>
        private SerialPort _comPort;

        /// <summary>
        /// API access mutex
        /// </summary>
        private readonly Mutex _accessMutex;

        #endregion

        #region constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Port">COM port</param>
        public COMLedManager(string port)
        {
            _comPort = new SerialPort()
            {
                BaudRate = 9600,
                PortName = port
            };
            _accessMutex = new Mutex();
        }

        #endregion

        #region public methodes

        /// <inheritdoc />
        public void InitConnexion()
        {
            try
            {
                _comPort.Open();
            }
            catch (Exception ex)
            {
                LogManager.LogLine(ex.Message);
            }
        }

        /// <inheritdoc />
        public void InitAnnimation()
        {
            byte[] frame = { (byte)COMFrameIdsEnum.Initialization, 0, 2000 / 256, 2000 % 256 };
            SendFrame(frame);
        }

        /// <inheritdoc />
        public void LightUpAllWhite(uint fadeInDuration, uint diplayDuration, uint fadeOutDuration)
        {
            LightUpSideWhite(WidgetPositionEnum.Center, fadeInDuration, diplayDuration, fadeOutDuration);
        }

        /// <inheritdoc />
        public void LightUpSideWhite(WidgetPositionEnum side, uint fadeInDuration, uint diplayDuration, uint fadeOutDuration)
        {
            byte sideId = 0;
            bool found = true;
            switch (side)
            {
                case (WidgetPositionEnum.Center):
                    {
                        sideId = (byte)COMFrameIdsEnum.Initialization;
                        break;
                    }
                case (WidgetPositionEnum.Top):
                    {
                        sideId = (byte)COMFrameIdsEnum.ShowTop;
                        break;
                    }
                case (WidgetPositionEnum.Right):
                    {
                        sideId = (byte)COMFrameIdsEnum.ShowRight;
                        break;
                    }
                case (WidgetPositionEnum.Bot):
                    {
                        sideId = (byte)COMFrameIdsEnum.ShowBot;
                        break;
                    }
                case (WidgetPositionEnum.Left):
                    {
                        sideId = (byte)COMFrameIdsEnum.ShowLeft;
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
                byte[] frame = { (byte)sideId, 0,
                    (byte)(fadeInDuration / 256),
                    (byte)(fadeInDuration % 256),
                    (byte)(diplayDuration / 256),
                    (byte)(diplayDuration % 256),
                    (byte)(fadeOutDuration / 256),
                    (byte)(fadeOutDuration % 256) };
                SendFrame(frame);
            }
        }

        /// <inheritdoc />
        public void LightUpPosWhite(byte xPosPercent, byte yPosPercent, uint duration)
        {
            byte[] frame = { (byte)COMFrameIdsEnum.Position, 0,
                            xPosPercent,
                            yPosPercent,
                            (byte)(duration / 256),
                            (byte)(duration % 256)};
            SendFrame(frame);
        }

        /// <inheritdoc />
        public void SetPartyMode(bool on)
        {
            byte[] frame = { (byte)COMFrameIdsEnum.PartyMode, 0,
                            (byte)(on ? 0xFF : 0x0)};
            SendFrame(frame);
        }

        #endregion

        #region private functions

        /// <summary>
        /// Send frame through com port
        /// </summary>
        /// <param name="frame">Frame to send</param>
        private void SendFrame(byte[] frame)
        {
            frame[1] = (byte)frame.Length;
            if (_accessMutex.WaitOne())
            {
                if (_comPort.IsOpen)
                {
                    try
                    {
                        _comPort.Write(frame, 0, frame.Length);
                    }
                    catch (Exception)
                    {
                        InitConnexion();
                    }
                }
                _accessMutex.ReleaseMutex();
            }
        }

        #endregion
    }
}
