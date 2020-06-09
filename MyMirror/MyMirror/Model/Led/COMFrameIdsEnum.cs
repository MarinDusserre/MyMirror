// -----------------------------------------------------------------------
// <copyright file="COMFrameIdsEnum.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains enum COMFrameIdsEnum</summary>
// -----------------------------------------------------------------------

namespace MyMirror.Model.Led
{
    /// <summary>
    /// Enumerate all recognized frame Ids
    /// </summary>
    public enum COMFrameIdsEnum
    {
        /// <summary>
        /// Initialization frame ID
        /// </summary>
        Initialization = 1,

        /// <summary>
        /// Show all frame ID
        /// </summary>
        ShowAll = 2,

        /// <summary>
        /// Show top frame ID
        /// </summary>
        ShowTop = 3,

        /// <summary>
        /// Show right frame ID
        /// </summary>
        ShowRight = 4,

        /// <summary>
        /// Show bot frame ID
        /// </summary>
        ShowBot = 5,

        /// <summary>
        /// Show left frame ID
        /// </summary>
        ShowLeft = 6,

        /// <summary>
        /// Position frame ID
        /// </summary>
        Position = 7,

        /// <summary>
        /// Party mode frame ID
        /// </summary>
        PartyMode = 8
    }
}