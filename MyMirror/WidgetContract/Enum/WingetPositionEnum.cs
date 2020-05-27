// -----------------------------------------------------------------------
// <copyright file="WidgetPositionEnum.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains enum WidgetPositionEnum</summary>
// -----------------------------------------------------------------------

namespace WingetContract.Enum
{
    /// <summary>
    /// Enumerate widgets possible positions
    /// </summary>
    public enum WidgetPositionEnum
    {
        /// <summary>
        /// Top left corner
        /// </summary>
        TopLeft = 0,

        /// <summary>
        /// Center top
        /// </summary>
        Top = 1,

        /// <summary>
        /// Top right corner
        /// </summary>
        TopRight = 2,

        /// <summary>
        /// Center left
        /// </summary>
        Left = 3,

        /// <summary>
        /// Center right
        /// </summary>
        Right = 4,

        /// <summary>
        /// Center bottom
        /// </summary>
        Bot = 5,
            
        /// <summary>
        /// Center, full screen
        /// </summary>
        Center = 12,

        /// <summary>
        /// Not defined
        /// </summary>
        None = 100,
    }
}
