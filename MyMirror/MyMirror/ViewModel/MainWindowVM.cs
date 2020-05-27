// -----------------------------------------------------------------------
// <copyright file="MainWindowVM.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class MainWindowVM</summary>
// -----------------------------------------------------------------------

namespace MyMirror.ViewModel
{
    using Common.Enums;
    using Common.ViewModel;
    using InputContract;
    using MyMirror.Model;
    using MyMirror.Properties;
    using MyMirror.View;
    using System;
    using System.Collections.Generic;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;
    using WingetContract;
    using WingetContract.Enum;

    /// <summary>
    /// View model for the main window
    /// </summary>
    internal class MainWindowVM : ViewModelBase
    {
        #region Event

        /// <summary>
        /// Show element event
        /// </summary>
        public event EventHandler<ShowElementEventArgs> ShowElementEvent;

        #endregion

        #region Properties

        /// <summary>
        /// Gets widgets dictionary
        /// </summary>
        public Dictionary<int, IWidget> Widgets
        {
            get =>_widgets;
            private set => Set(ref _widgets, value);
        }

        /// <summary>
        /// Gets central widget
        /// </summary>
        public IWidget CenterWidget
        {
            get => _centerWidget;
            private  set => Set(ref _centerWidget, value);
        }

        /// <summary>
        /// Gets click circle animation
        /// </summary>
        public ClickCircle ClickCircle
        {
            get => _clickCircle;
            private set => Set(ref _clickCircle, value);
        }

        /// <summary>
        /// Gets sound visibility
        /// </summary>
        public bool SoundVisibility
        {
            get => _soundVisibility;
            private set => Set(ref _soundVisibility, value);
        }
    
        /// <summary>
        /// Gets main window loaded command
        /// </summary>
        public ICommand MainWindowLoadedCommand { get; private set; }

        /// <summary>
        /// Gets main window key down command
        /// </summary>
        public ICommand KeyDownCommand { get; private set; }    

        /// <summary>
        /// Gets main model
        /// </summary>
        public MainModel MainModel
        {
            get => _mainModel;
            private set => Set(ref _mainModel, value);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Main model
        /// </summary>
        private MainModel _mainModel;

        /// <summary>
        /// Widgets dictionary
        /// </summary>
        private Dictionary<int, IWidget> _widgets;

        /// <summary>
        /// Widgets visibility timers
        /// </summary>
        private Dictionary<int, Timer> _wingetVisibilityTimers;

        /// <summary>
        /// Sound visibility timer
        /// </summary>
        private Timer _soundTimer;

        /// <summary>
        /// Sleep mode timer
        /// </summary>
        private Timer _sleepModeTimer;

        /// <summary>
        /// Central widget
        /// </summary>
        private IWidget _centerWidget;

        /// <summary>
        /// Click circle animation
        /// </summary>
        public ClickCircle  _clickCircle;

        /// <summary>
        /// Sound visibility
        /// </summary>
        private bool _soundVisibility;

        /// <summary>
        /// If sleep mode is activated
        /// </summary>
        private bool _sleepMode;
        
        /// <summary>
        /// Management window
        /// </summary>
        private ManagementWindow _managementWindow;

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindowVM()
        {
            MainWindowLoadedCommand = new RelayCommand(MainWindowLoaded);
            KeyDownCommand = new RelayCommand(KeyDown);

            _soundTimer = new Timer(1000);
            _soundTimer.Elapsed += OnSoundTimer;


            ClickCircle = new ClickCircle();
        }

        #endregion

        #region Private functions

        /// <summary>
        /// Handles main windows loaded event
        /// </summary>
        /// <param name="obj">Parameters</param>
        private void MainWindowLoaded(object obj)
        {
            CreateSizeDictionnary();

            MainModel = MainModel.Instance;

            _sleepModeTimer = new Timer(_mainModel.MainSettings.Settings.SleepTimer.Value);
            _sleepModeTimer.Elapsed += OnSleepModeTimer;

            _mainModel.LoadWinget();
            _mainModel.LoadInput();

            foreach (IScreenInput input in _mainModel.ScreenInputs)
            {
                input.ScreenInputEvent += OnScreenInputEvent;
            }

            Widgets = new Dictionary<int, IWidget>();
            _wingetVisibilityTimers = new Dictionary<int, Timer>();

            LoadWidget();

            _mainModel.SetMainMessage(String.Format(Resources.StartMainMessage, _mainModel.MainSettings.Settings.UserName.Value), 5000);
            _mainModel.LedManager.InitConnexion();
            _mainModel.LedManager.InitAnnimation();
        }

        /// <summary>
        /// Handles main window key down event
        /// </summary>
        /// <param name="obj"></param>
        private void KeyDown(object obj)
        {
            if (_managementWindow == null || !_managementWindow.IsActive)
            {
                _managementWindow = new ManagementWindow();
                _managementWindow.ShowDialog();
            }
        }

        /// <summary>
        /// Load widgets
        /// </summary>
        private void LoadWidget()
        {
            CenterWidget = null;

            foreach (IWidget widget in _mainModel.WidgetList)
            {
                if(widget.Name == _mainModel.MainSettings.Settings.TopRightWidget.Value)
                {
                    AddWidgetToPos(widget, WidgetPositionEnum.TopRight);
                }
                else if (widget.Name == _mainModel.MainSettings.Settings.TopWidget.Value)
                {
                    AddWidgetToPos(widget, WidgetPositionEnum.Top);
                }
                else if (widget.Name == _mainModel.MainSettings.Settings.TopLeftWidget.Value)
                {
                    AddWidgetToPos(widget, WidgetPositionEnum.TopLeft);
                }
                else if (widget.Name == _mainModel.MainSettings.Settings.LeftWidget.Value)
                {
                    AddWidgetToPos(widget, WidgetPositionEnum.Left);
                }
                else if (widget.Name == _mainModel.MainSettings.Settings.RightWidget.Value)
                {
                    AddWidgetToPos(widget, WidgetPositionEnum.Right);
                }
                else if (widget.Name == _mainModel.MainSettings.Settings.BotWidget.Value)
                {
                    AddWidgetToPos(widget, WidgetPositionEnum.Bot);
                }
            }

            _mainModel.AddVolume(0);

            NotifyPropertyChanged(nameof(Widgets));
        }

        /// <summary>
        /// Add widget to position
        /// </summary>
        /// <param name="widget">Widget to add</param>
        /// <param name="pos">Widget position</param>
        private void AddWidgetToPos(IWidget widget, WidgetPositionEnum pos)
        {
            Widgets[(int)pos] = widget;
            _wingetVisibilityTimers[(int)pos] = new Timer(500);
            _wingetVisibilityTimers[(int)pos].Elapsed += OnVisibilityTimer;
            widget.Initialize();
        }

        /// <summary>
        /// Handles screen input events
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void OnScreenInputEvent(object sender, ScreenInputEventArg e)
        {
            Application.Current?.Dispatcher.Invoke(new Action(() =>
            {
                ClickCircle.Opacity = 0f;

                // If sleep mode exit : exit
                if (_sleepMode && e.Gesture != InputGestureEnum.None)
                {
                    _sleepMode = false;
                    CenterWidget = null;
                }

                if (e.Gesture == InputGestureEnum.Exit)
                {
                    if (CenterWidget == null)
                    {
                        _mainModel.LedManager.LightUpAllWhite(0, 2900, 500);

                        // If no central winget : show everything 3s
                        foreach (WidgetPositionEnum pos in (WidgetPositionEnum[])Enum.GetValues(typeof(WidgetPositionEnum)))
                        {
                            ShowWinget(pos, true, 3000);
                        }
                    }
                    else
                    {
                        // Else exit it
                        CenterWidget = null;
                        _mainModel.LedManager.SetPartyMode(false);
                    }
                }
                else if (e.Gesture == InputGestureEnum.Click || e.Gesture == InputGestureEnum.Position)
                {
                    // Flash on click
                    if (e.Gesture == InputGestureEnum.Click)
                    {
                        _mainModel.LedManager.LightUpAllWhite(0, 50, 0);
                    }
                    else
                    {
                        _mainModel.LedManager.LightUpPosWhite((byte)(100 * e.XPos / App.Current.MainWindow.ActualWidth), (byte)(100 * e.YPos / App.Current.MainWindow.ActualHeight), 200);
                    }

                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)e.XPos, (int)e.YPos);

                    ClickCircle.XPos = e.XPos;
                    ClickCircle.YPos = e.YPos;
                    ClickCircle.Opacity = 1f;
                    ClickCircle.Size = e.Gesture == InputGestureEnum.Position ? SizeDict[8] : SizeDict[5];

                    if (CenterWidget == null)
                    {
                        // Get cursor position
                        WidgetPositionEnum cursorPosition = GetClickPos(e.XPos, e.YPos);
                        _mainModel.LedManager.LightUpSideWhite(cursorPosition, 300, 500, 300);
                        
                        // Get associated widget
                        if (Widgets.ContainsKey((int)cursorPosition))
                        {
                            if (e.Gesture == InputGestureEnum.Click)
                            {
                                // Show widget full version
                                CenterWidget = Widgets[(int)cursorPosition];
                                _mainModel.LedManager.SetPartyMode(true);
                            }
                            else
                            {
                                // Show widget reduce version
                                ShowWinget(cursorPosition, true);
                            }
                        }                        
                    }
                    else
                    {
                        CenterWidget.InputEvent((int)e.XPos, (int)e.YPos, e.Gesture);
                    }

                }
                else if (e.Gesture == InputGestureEnum.RollIn || e.Gesture == InputGestureEnum.Rollout)
                {
                    SoundVisibility = true;
                    _soundTimer.Stop();
                    _soundTimer.Start();

                    // Manage sound
                    _mainModel.AddVolume(e.Gesture == InputGestureEnum.RollIn ? 2 : -2);
                }

                // Manage sleep mode
                if (e.Gesture == InputGestureEnum.None)
                {
                    if (!_sleepModeTimer.Enabled)
                    {
                        _sleepModeTimer.Start();
                    }
                }
                else
                {
                    if (_sleepModeTimer.Enabled)
                    {
                        _sleepModeTimer.Stop();
                    }
                }
            }));
        }

        /// <summary>
        /// Show widget
        /// </summary>
        /// <param name="clickPos">Widget postion</param>
        /// <param name="show">Whether whow widgets</param>
        /// <param name="duration">Showing duration</param>
        private void ShowWinget(WidgetPositionEnum clickPos, bool show, double duration = 500)
        {
            if(_wingetVisibilityTimers.ContainsKey((int)clickPos))
            {
                _wingetVisibilityTimers[(int)clickPos].Stop();
                ShowElementEvent?.Invoke(this, new ShowElementEventArgs(clickPos, show));

                if (show)
                {
                    _wingetVisibilityTimers[(int)clickPos].Interval = duration;
                    _wingetVisibilityTimers[(int)clickPos].Start();
                }
            }
        }

        /// <summary>
        /// Handles widgets visibility timer events
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void OnVisibilityTimer(object sender, ElapsedEventArgs e)
        {
            WidgetPositionEnum pos = WidgetPositionEnum.None;

            foreach (var timer in _wingetVisibilityTimers)
            {
                if(timer.Value == (Timer)sender)
                {
                    pos = (WidgetPositionEnum)timer.Key;
                    break;
                }
            }
            ShowWinget(pos, false);
        }

        /// <summary>
        /// Handles sound visibility timer
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void OnSoundTimer(object sender, ElapsedEventArgs e)
        {
            SoundVisibility = false;
        }

        /// <summary>
        /// Handles sleep mode timer event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void OnSleepModeTimer(object sender, ElapsedEventArgs e)
        { 
            foreach (IWidget winget in _mainModel.WidgetList)
            {
                if (winget.Name == _mainModel.MainSettings.Settings.SleepWidget.Value)
                {
                    _sleepMode = true;

                    // Show widget full version
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        CenterWidget = winget;
                    }));
                    break;
                }
            }
        }

        /// <summary>
        /// Gets wiget position enum from X Y pos
        /// </summary>
        /// <param name="xPos">X os</param>
        /// <param name="yPos">y pos</param>
        /// <returns>WidgetPositionEnum value</returns>
        private WidgetPositionEnum GetClickPos(double xPos, double yPos)
        {
            WidgetPositionEnum ret = WidgetPositionEnum.None;

            if(2 * xPos < XCenter)
            {
                if(3 * yPos < YCenter)
                {
                    ret = WidgetPositionEnum.TopLeft;
                }
                else
                {
                    ret = WidgetPositionEnum.Left;
                }
            }
            else if (xPos > 1.5 * XCenter)
            {
                if (3 * yPos < YCenter)
                {
                    ret = WidgetPositionEnum.TopRight;
                }
                else
                {
                    ret = WidgetPositionEnum.Right;
                }
            }
            else
            {
                if (3 * yPos < YCenter)
                {
                    ret = WidgetPositionEnum.Top;
                }
                else if (yPos > 1.5 * YCenter)
                {
                    ret = WidgetPositionEnum.Bot;
                }
            }

            return ret;
        }

        #endregion
    }
}
