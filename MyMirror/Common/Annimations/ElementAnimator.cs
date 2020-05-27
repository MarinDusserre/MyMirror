// -----------------------------------------------------------------------
// <copyright file="ElementAnimator.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class ElementAnimator</summary>
// -----------------------------------------------------------------------

namespace Common.Annimations
{
    using System.Timers;

    /// <summary>
    /// Base class for element animation
    /// </summary>
    public abstract class ElementAnimator
    {
        #region Properties

        /// <summary>
        /// Annimation speed
        /// </summary>
        public int Speed { get; set; } = 10000;

        /// <summary>
        /// Annimation speed
        /// </summary>
        public int RefreshPeriode { get; set; } = 50;

        #endregion

        #region Protected members

        /// <summary>
        /// Animation refresh timer
        /// </summary>
        protected Timer _refreshTimer;

        #endregion
    }
}