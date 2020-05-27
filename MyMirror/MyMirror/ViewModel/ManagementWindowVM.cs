// -----------------------------------------------------------------------
// <copyright file="ManagementWindowVM.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class ManagementWindowVM</summary>
// -----------------------------------------------------------------------

namespace MyMirror.ViewModel
{
    using Common.ViewModel;
    using System.Windows;
    using System.Windows.Input;
    using MyMirror.View;
    using MyMirror.Model;
    using System.Collections.Generic;
    using MyMirror.Properties;
    using WingetContract;
    using InputContract;

    /// <summary>
    /// View model for the management window
    /// </summary>
    internal class ManagementWindowVM : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// Gets exit button command
        /// </summary>
        public ICommand ExitButtonCommand { get; private set; }

        /// <summary>
        /// Gets return button command
        /// </summary>
        public ICommand ReturnButtonCommand { get; private set; }

        /// <summary>
        /// Gets save button command
        /// </summary>
        public ICommand SaveButtonCommand { get; private set; }

        /// <summary>
        /// Gets log button command
        /// </summary>
        public ICommand LogButtonCommand { get; private set; }
        
        /// <summary>
        /// Gets main model
        /// </summary>
        public MainModel MainModel
        {
            get => _mainModel;
            private set => Set(ref _mainModel, value);
        }

        /// <summary>
        /// Tab items list
        /// </summary>
        public List<ParametersTabItem> TabItems
        {
            get => _tabItems;
            private set => Set(ref _tabItems, value);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Main model
        /// </summary>
        private MainModel _mainModel;

        /// <summary>
        /// Tab items list
        /// </summary>
        private List<ParametersTabItem> _tabItems;

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ManagementWindowVM()
        {
            ExitButtonCommand = new RelayCommand(ExitButton);
            ReturnButtonCommand = new RelayCommand(ReturnButton);
            SaveButtonCommand = new RelayCommand(SaveButton);
            LogButtonCommand = new RelayCommand(LogButton);
            
            MainModel = MainModel.Instance;

            _mainModel.InitializeMainSettings();
            LoadParameters();
        }

        #endregion

        #region Private functions

        /// <summary>
        /// Handles exit command
        /// </summary>
        /// <param name="obj">Parameters</param>
        private void ExitButton(object obj)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Handles exit command
        /// </summary>
        /// <param name="obj">Parameters</param>
        private void ReturnButton(object obj)
        {
            ((Window)obj)?.Close();
        }

        /// <summary>
        /// Handles log command
        /// </summary>
        /// <param name="obj">Parameters</param>
        private void LogButton(object obj)
        {
            new LogWindow().ShowDialog();
        }

        /// <summary>
        /// Load parameters
        /// </summary>
        private void LoadParameters()
        {
            List<ParametersTabItem> tabItems = new List<ParametersTabItem>();

            // Add main items
            ParametersTabItem tab = new ParametersTabItem
            {
                Title = Resources.MainTabName,
                Items = MainModel.MainSettings.Settings.GetSettingsList()
            };
            tabItems.Add(tab);

            // Add widgets parameters
            foreach (IWidget widget in MainModel.WidgetList)
            {
                tab = new ParametersTabItem
                {
                    Title = widget.Name,
                    Items = widget.Settings.GetSettingsList()
                };

                tabItems.Add(tab);
            }

            // Add inputs parameters
            foreach (IScreenInput input in MainModel.ScreenInputs)
            {
                tab = new ParametersTabItem
                {
                    Title = input.Name,
                    Items = input.Settings.GetSettingsList()
                };

                tabItems.Add(tab);
            }

            TabItems = tabItems;
        }

        /// <summary>
        /// Handles exit command
        /// </summary>
        /// <param name="obj">Parameters</param>
        private void SaveButton(object obj)
        {
            // Update main parameters
            MainModel.MainSettings.Settings.SetSettingsList(_tabItems[0].Items);
            MainModel.MainSettings.Save();

            // Update widgets parameters
            int index = 1;
            foreach (IWidget widget in MainModel.WidgetList)
            {
                widget.Settings.SetSettingsList(_tabItems[index].Items);
                index++;
            }

            // Update input parameters
            foreach (IScreenInput input in MainModel.ScreenInputs)
            {
                input.Settings.SetSettingsList(_tabItems[index].Items);
                index++;
            }  

            //Restart application
            System.Windows.Forms.Application.Restart();
            Application.Current.Shutdown();
        }    

        #endregion
    }
}
