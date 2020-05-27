// -----------------------------------------------------------------------
// <copyright file="ScreenInputEventArg.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Screen input event arguments</summary>
// -----------------------------------------------------------------------

namespace InputContract
{
    using System;
    using Common.Enums;

    /// <summary>
    /// Screen input event arguments
    /// </summary>
    public class ScreenInputEventArg : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets or sets X position
        /// </summary>
        public double XPos { get; set; } 

        /// <summary>
        /// Gets or sets Y position
        /// </summary>
        public double YPos { get; set; }

        /// <summary>
        /// Gest or sets gesture
        /// </summary>
        public InputGestureEnum Gesture { get; set; }

        #endregion

        #region Construtor

        /// <summary>
        /// Instatiates screen input event oject
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y positon</param>
        /// <param name="gesture">Gesture</param>
        public ScreenInputEventArg(double x, double y, InputGestureEnum gesture)
        {
            XPos = x;
            YPos = y;
            Gesture = gesture;
        }

        #endregion
    }
}
