// -----------------------------------------------------------------------
// <copyright file="ILedManager.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains interface ILedManager</summary>
// -----------------------------------------------------------------------

namespace MyMirror.Model.Led
{
    using WingetContract.Enum;
    public interface ILedManager
    {
        /// <summary>
        /// Initialize connection with Leds
        /// </summary>
        void InitConnexion();

        /// <summary>
        /// Send init annimation request
        /// </summary>
        void InitAnnimation();

        /// <summary>
        /// Light up all leds in white for a given duration
        /// </summary>
        /// <param name="fadeInDuration">Fade in duration</param>
        /// <param name="diplayDuration">Fade out duration</param>
        /// <param name="fadeOutDuration">Display duration</param>
        void LightUpAllWhite(uint fadeInDuration, uint diplayDuration, uint fadeOutDuration);

        /// <summary>
        /// Light up all leds side in white for a given duration
        /// </summary>
        /// <param name="side">Side to lite up</param>
        /// <param name="fadeInDuration">Fade in duration</param>
        /// <param name="diplayDuration">Fade out duration</param>
        /// <param name="fadeOutDuration">Display duration</param>
        void LightUpSideWhite(WidgetPositionEnum side, uint fadeInDuration, uint diplayDuration, uint fadeOutDuration);

        /// <summary>
        /// Light up position
        /// </summary>
        /// <param name="xPosPercent">xPos from top left corner in %</param>
        /// <param name="yPosPercent">yPos from top left corner in %</param>
        /// <param name="duration">Display duration</param>
        void LightUpPosWhite(byte xPosPercent, byte yPosPercent, uint duration);

        /// <summary>
        /// Set party mode On or Off
        /// </summary>
        /// <param name="on">Is On</param>
        void SetPartyMode(bool on);
    }
}
