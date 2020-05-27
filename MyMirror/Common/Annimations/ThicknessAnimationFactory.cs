// -----------------------------------------------------------------------
// <copyright file="ThicknessAnimationFactory.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class ThicknessAnimationFactory</summary>
// -----------------------------------------------------------------------

namespace Common.Annimations
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Provides functions to create thickness animations
    /// </summary>
    static public class ThicknessAnimationFactory
    {
        #region Public methode

        /// <summary>
        /// Get animation from right original pos to in
        /// </summary>
        /// <param name="size">Anniamation width</param>
        /// <param name="duration">Annimation duation</param>
        /// <returns>Generated thickness annimation</returns>
        public static ThicknessAnimation GetRightInAnimation(double size, int duration)
        {
            return new ThicknessAnimation
            {
                From = new Thickness(size, 0, -size, 0),
                To = new Thickness(0, 0, 0, 0),
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
            };
        }

        /// <summary>
        /// Get animation from right original pos to out
        /// </summary>
        /// <param name="size">Anniamation width</param>
        /// <param name="duration">Annimation duation</param>
        /// <returns>Generated thickness annimation</returns>
        public static ThicknessAnimation GetRightOutAnimation(double size, int duration)
        {
            return new ThicknessAnimation
            {
                From = new Thickness(0, 0, 0, 0),
                To = new Thickness(size, 0, -size, 0),
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
            };
        }

        /// <summary>
        /// Get animation from left original pos to in
        /// </summary>
        /// <param name="size">Anniamation width</param>
        /// <param name="duration">Annimation duation</param>
        /// <returns>Generated thickness annimation</returns>
        public static ThicknessAnimation GetLeftInAnimation(double size, int duration)
        {
            return new ThicknessAnimation
            {
                From = new Thickness(-size, 0, size, 0),
                To = new Thickness(0, 0, 0, 0),
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
            };
        }

        /// <summary>
        /// Get animation from left original pos to out
        /// </summary>
        /// <param name="size">Anniamation width</param>
        /// <param name="duration">Annimation duation</param>
        /// <returns>Generated thickness annimation</returns>
        public static ThicknessAnimation GetLeftOutAnimation(double size, int duration)
        {
            return new ThicknessAnimation
            {
                From = new Thickness(0, 0, 0, 0),
                To = new Thickness(-size, 0, size, 0),
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
            };
        }

        /// <summary>
        /// Get animation from bot original pos to in
        /// </summary>
        /// <param name="size">Anniamation height</param>
        /// <param name="duration">Annimation duation</param>
        /// <returns>Generated thickness annimation</returns>
        public static ThicknessAnimation GetBotInAnimation(double size, int duration)
        {
            return new ThicknessAnimation
            {
                From = new Thickness(0, size, 0, -size),
                To = new Thickness(0, 0, 0, 0),
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
            };
        }

        /// <summary>
        /// Get animation from bot original pos to out
        /// </summary>
        /// <param name="size">Anniamation height</param>
        /// <param name="duration">Annimation duation</param>
        /// <returns>Generated thickness annimation</returns>
        public static ThicknessAnimation GetBotOutAnimation(double size, int duration)
        {
            return new ThicknessAnimation
            {
                From = new Thickness(0, 0, 0, 0),
                To = new Thickness(0, size, 0, -size),
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
            };
        }

        /// <summary>
        /// Get animation from top original pos to in
        /// </summary>
        /// <param name="size">Anniamation height</param>
        /// <param name="duration">Annimation duation</param>
        /// <returns>Generated thickness annimation</returns>
        public static ThicknessAnimation GetTopInAnimation(double size, int duration)
        {
            return new ThicknessAnimation
            {
                From = new Thickness(0, -size, 0, size),
                To = new Thickness(0, 0, 0, 0),
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
            };
        }

        /// <summary>
        /// Get animation from top original pos to out
        /// </summary>
        /// <param name="size">Anniamation height</param>
        /// <param name="duration">Annimation duation</param>
        /// <returns>Generated thickness annimation</returns>
        public static ThicknessAnimation GetTopOutAnimation(double size, int duration)
        {
            return new ThicknessAnimation
            {
                From = new Thickness(0, 0, 0, 0),
                To = new Thickness(0, -size, 0, size),
                Duration = new Duration(TimeSpan.FromMilliseconds(duration)),
            };
        }

        #endregion
    }
}
