// -----------------------------------------------------------------------
// <copyright file="MouseInput.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class MouseInput</summary>
// -----------------------------------------------------------------------

namespace Common.Annimations
{
    using System;
    using System.Timers;
    using System.Windows.Controls;
    using System.Windows.Threading;

    /// <summary>
    /// Annimator for standard scrollbar
    /// </summary>
    public class ScrollBarAnimator : ElementAnimator
    {
        #region Private members

        /// <summary>
        /// Current audio level
        /// </summary>
        private ScrollViewer _scrollViewer;

        /// <summary>
        /// Horizontal target to scroll to
        /// </summary>
        private double _horizontalTarget;

        /// <summary>
        /// Vertical target to scroll to
        /// </summary>>
        private double _verticalTarget;

        /// <summary>
        /// Horizontal step to scroll each time
        /// </summary>
        private double _horizontalMovement;

        /// <summary>
        /// Horizontal step to scroll each time
        /// </summary>>
        private double _verticalMovement;

        /// <summary>
        /// Scroll horizontally
        /// </summary>
        private bool _horizontalScrolling;

        /// <summary>
        /// Scroll vertically
        /// </summary>>
        private bool _verticalScrolling;

        /// <summary>
        /// View dispatcher
        /// </summary>
        private Dispatcher _dispatcher;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scrollViewer">Scroll viewer to animate</param>
        public ScrollBarAnimator(ScrollViewer scrollViewer, Dispatcher dispatcher)
        {
            _scrollViewer = scrollViewer;
            _dispatcher = dispatcher;
            _horizontalScrolling = false;
            _verticalScrolling = false;
        }

        #endregion

        #region Public class

        /// <summary>
        /// Scrott to position
        /// </summary>
        /// <param name="horizontalPosition">Horizontal osition to scoll to</param>
        /// <param name="verticalPosition">Vertical position to scoll to</param>
        public void ScrollToPosition(double horizontalPosition, double verticalPosition)
        {
            _verticalScrolling = true;
            _horizontalScrolling = true;
            StartScrolling(horizontalPosition, verticalPosition);
        }


        /// <summary>
        /// Scrott to a given offset position
        /// </summary>
        /// <param name="horizontalPosition">Horizontal offset to scoll</param>
        /// <param name="verticalPosition">offset position to scoll tparam>
        public void ScrollToOffset(double horizontalOffSet, double verticalOffSet)
        {
            _verticalScrolling = true;
            _horizontalScrolling = true;
            ScrollToPosition(horizontalOffSet + _scrollViewer.HorizontalOffset, verticalOffSet + _scrollViewer.VerticalOffset);
        }

        /// <summary>
        /// Scrott to position
        /// </summary>
        /// <param name="verticalPosition">Vertical position to scoll to</param>
        public void ScrollToVerticalPosition(double verticalPosition)
        {
            _verticalScrolling = true;
            ScrollToPosition(0, verticalPosition);
        }


        /// <summary>
        /// Scrott to a given offset position
        /// </summary>
        /// <param name="verticalPosition">offset position to scoll tparam>
        public void ScrollToVerticalOffset(double verticalOffSet)
        {
            _verticalScrolling = true;
            ScrollToVerticalPosition(verticalOffSet + _scrollViewer.VerticalOffset);
        }

        /// <summary>
        /// Scrott to position
        /// </summary>
        /// <param name="horizontalPosition">Horizontal osition to scoll to</param>
        public void ScrollToHorizontalPosition(double horizontalPosition)
        {
            _horizontalScrolling = true;
            ScrollToPosition(horizontalPosition, 0);
        }


        /// <summary>
        /// Scroll to a given offset position
        /// </summary>
        /// <param name="horizontalPosition">Horizontal offset to scoll</param>
        public void ScrollToHorizontalOffset(double horizontalOffSet)
        {
            _horizontalScrolling = true;
            ScrollToHorizontalPosition(horizontalOffSet + _scrollViewer.HorizontalOffset);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Start the scrolling annimation 
        /// </summary>
        /// <param name="horizontalPosition">Horizontal osition to scoll to</param>
        /// <param name="verticalPosition">Vertical position to scoll to</param>
        private void StartScrolling(double horizontalPosition, double verticalPosition)
        {
            _horizontalTarget = horizontalPosition;
            _verticalTarget = verticalPosition;

            double actualH = _scrollViewer.HorizontalOffset;
            double actualV = _scrollViewer.VerticalOffset;

            _horizontalMovement = (_horizontalTarget - actualH) * Speed / (RefreshPeriode * 1000);
            _verticalMovement = (_verticalTarget - actualV) * Speed / (RefreshPeriode * 1000);

            _refreshTimer = new Timer(RefreshPeriode)
            {
                AutoReset = false
            };

            _refreshTimer.Elapsed += RefreshScroll;

            _refreshTimer.Start();
        }

        /// <summary>
        /// Handles refresh scroll events
        /// </summary>
        /// <param name="sender">Not used</param>
        /// <param name="e">Not used</param>
        private void RefreshScroll(object sender, ElapsedEventArgs e)
        {
            _dispatcher.BeginInvoke(new Action(() =>
            {
                if (Math.Abs((_scrollViewer.HorizontalOffset + _horizontalMovement) - _horizontalTarget) < Speed / RefreshPeriode)
                {
                    _scrollViewer.ScrollToHorizontalOffset(_horizontalTarget);
                    _horizontalScrolling = false;
                }
                else if (_horizontalScrolling)
                {
                    _scrollViewer.ScrollToHorizontalOffset(_scrollViewer.HorizontalOffset + _horizontalMovement);
                }

                if (Math.Abs((_scrollViewer.VerticalOffset + _verticalMovement) - _verticalTarget) < Speed / RefreshPeriode)
                {
                    _scrollViewer.ScrollToVerticalOffset(_verticalTarget);
                    _verticalScrolling = false;
                }
                else if (_verticalScrolling)
                {
                    _scrollViewer.ScrollToVerticalOffset(_scrollViewer.VerticalOffset + _verticalMovement);
                }

                if (_horizontalScrolling || _verticalScrolling)
                {
                    _refreshTimer.Start();
                }
            }));
        }

        #endregion
    }
}
