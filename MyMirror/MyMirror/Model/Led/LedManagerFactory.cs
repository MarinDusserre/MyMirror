// -----------------------------------------------------------------------
// <copyright file="LedManagerFactory.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class LedManagerFactory</summary>
// -----------------------------------------------------------------------
namespace MyMirror.Model.Led
{
    /// <summary>
    /// Create Led manager
    /// </summary>
    public static class LedManagerFactory
    {
        public static ILedManager GetLedManager(string mode, string port)
        {
            if(mode.ToLower() == LedModeEnum.Wifi.ToString().ToLower())
            {
                return new WifiLedsManager(port);
            }
            else
            {
                return new COMLedManager(port);
            }
        }
    }
}
