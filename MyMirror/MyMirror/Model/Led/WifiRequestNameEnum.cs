// -----------------------------------------------------------------------
// <copyright file="WifiRequestNameEnum.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains enum WifiRequestNameEnum</summary>
// -----------------------------------------------------------------------

namespace MyMirror.Model.Led
{
    /// <summary>
    /// Enumerate all recognized frame Ids
    /// </summary>
    public enum WifiRequestNameEnum
    {
        /// <summary>
        /// Request name
        /// </summary>
        Request,

        /// <summary>
        /// Initialisation
        /// </summary>
        Initialization,

        /// <summary>
        /// Show all
        /// </summary>
        ShowAll,

        /// <summary>
        /// Show top
        /// </summary>
        ShowTop,

        /// <summary>
        /// Show right
        /// </summary>
        ShowRight,

        /// <summary>
        /// Show bot
        /// </summary>
        ShowBot,

        /// <summary>
        /// Show left
        /// </summary>
        ShowLeft,

        /// <summary>
        /// Position
        /// </summary>
        Position,

        /// <summary>
        /// Position X
        /// </summary>
        PositionX,

        /// <summary>
        /// Position Y
        /// </summary>
        PositionY,

        /// <summary>
        /// Party mode
        /// </summary>
        PartyMode,

        /// <summary>
        /// Diplay duration
        /// </summary>
        DiplayDuration,

        /// <summary>
        /// Fade in duration
        /// </summary>
        FadeInDuration,

        /// <summary>
        /// Fade out duration
        /// </summary>
        FadeOutDuration
    }
}