// -----------------------------------------------------------------------
// <copyright file="ISoundManageable.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains interface ISoundManageable</summary>
// -----------------------------------------------------------------------

namespace WingetContract
{
    /// <summary>
    /// Contains interface for sound manageable widgets
    /// </summary>
    public interface ISoundManageable
    {
        #region Public functions

        /// <summary>
        /// Set widgets sound level
        /// </summary>
        /// <param name="volume">Sound level, from 0 to 100</param>
        void SetSoundVolume(int volume);

        #endregion
    }
}
