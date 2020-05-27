// -----------------------------------------------------------------------
// <copyright file="MouseInput.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class MouseInput</summary>
// -----------------------------------------------------------------------

namespace MouseInput
{
    using Common.Enums;
    using Common.Settings;
    using InputContract;
    using System;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;
    using global::MouseInput.Properties;

    /// <summary>
    /// IScreenInput implementation for mouse and keyboard
    /// </summary>
    internal class MouseInput : IScreenInput
    {
        #region Events

        /// <inheritdoc />
        public event EventHandler<ScreenInputEventArg> ScreenInputEvent;

        #endregion

        #region Properties

        /// <inheritdoc />
        public string Name => Resources.Name;

        /// <inheritdoc />
        public ISettingsBase Settings => _settingsManager.Settings;

        #endregion

        #region Private members

        /// <summary>
        /// Update Timer
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// Current gesture
        /// </summary>
        private InputGestureEnum _gesture;

        /// <summary>
        /// Previous mouse position
        /// </summary>
        private System.Drawing.Point _previousPosition;

        /// <summary>
        /// Input settings manager
        /// </summary>
        private SettingsManager<MouseSettings> _settingsManager;

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MouseInput()
        {
            _settingsManager = new SettingsManager<MouseSettings>();
            _settingsManager.Initialize(Resources.SettingsFileName);

            _timer = new Timer(_settingsManager.Settings.RefreshPeriode.Value)
            {
                AutoReset = false
            };
            _timer.Elapsed += OnTimer;
            _timer.Start();

            _previousPosition = System.Windows.Forms.Cursor.Position;
            _gesture = InputGestureEnum.None;

            Application.Current.MainWindow.MouseWheel += OnMouseWheelEvent;
            Application.Current.MainWindow.MouseDown += OnMouseDownEvent;
        }

        #endregion

        #region Private functions

        /// <summary>
        /// Handles mouse down event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void OnMouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            _timer.Stop();            
            _gesture = e.ChangedButton == MouseButton.Left ? InputGestureEnum.Click : InputGestureEnum.Exit;
            OnTimer(null, null);
        }

        /// <summary>
        /// Handles mouse wheel event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void OnMouseWheelEvent(object sender, MouseWheelEventArgs e)
        {
            _timer.Stop();
            _gesture = e.Delta > 0 ? InputGestureEnum.RollIn : InputGestureEnum.Rollout;
            OnTimer(null, null);
        }

        /// <summary>
        /// Handles timer event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Args</param>
        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            _gesture = _gesture == InputGestureEnum.None && !System.Windows.Forms.Cursor.Position.Equals(_previousPosition) ?
                InputGestureEnum.Position : _gesture;

            _previousPosition = System.Windows.Forms.Cursor.Position;

            ScreenInputEvent?.Invoke(
            this,
            new ScreenInputEventArg(
                System.Windows.Forms.Cursor.Position.X,
                System.Windows.Forms.Cursor.Position.Y,
                _gesture));
            _gesture = InputGestureEnum.None;
            _timer.Start();
        }

        #endregion
    }
}
