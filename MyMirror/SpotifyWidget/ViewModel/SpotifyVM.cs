// -----------------------------------------------------------------------
// <copyright file="SpotifyVM.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class SpotifyVM</summary>
// -----------------------------------------------------------------------

namespace SpotifyWidget.ViewModel
{
    using SpotifyWidget.Model;
    using WingetContract;
    using System.Windows;
    using Common.ViewModel;
    using SpotifyWidget.View;

    /// <summary>
    /// Contains Soptify widget view model
    /// </summary>
    internal class SpotifyVM : ViewModelBase, ISoundManageable
    {
        #region Properties

        /// <summary>
        /// Gets Spotify Model
        /// </summary>
        public SpotifyModel SpotifyModel { get; private set; }

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public SpotifyVM()
        {
            SpotifyModel = new SpotifyModel();
        }

        #endregion

        #region Public methodes

        /// <summary>
        /// Initialize model
        /// </summary>
        public void Initialize()
        {
            SpotifyModel.Initialize();
        }

        /// <inheritdoc />
        public void SetSoundVolume(int volume)
        {
            SpotifyModel.SetSound(volume);
        }

        /// <summary>
        /// Handles inout click
        /// </summary>
        /// <param name="xPos">Click X pos</param>
        /// <param name="xPos">Click X pos</param>
        /// <param name="fullWidget">Widget ref</param>
        public void InputClick(int xPos, int yPos, SpotifyWidgetFull fullWidget)
        {
            if(yPos < (int)Application.Current.MainWindow.ActualWidth * 1 / 5)
            {
                SpotifyModel.SwitchView();
            }
            else
            {
                if(SpotifyModel.ShowMusicManagementPanel && yPos < (int)Application.Current.MainWindow.ActualWidth * 3 / 4)
                {
                    if (xPos < (int)Application.Current.MainWindow.ActualWidth / 3)
                    {
                        SpotifyModel.Previous();
                    }
                    else if (xPos < (int)Application.Current.MainWindow.ActualWidth * 2 / 3)
                    {
                        SpotifyModel.Play();
                    }
                    else if (xPos < (int)Application.Current.MainWindow.ActualWidth)
                    {
                        SpotifyModel.Next();
                    }
                }
                else if (SpotifyModel.ShowPlaylistManagementPanel)
                {
                    double margin = 0.2;
                    int targetPos = 0;
                    int itemSize = (int)SizeDict[85];

                    if (xPos > Application.Current.MainWindow.ActualWidth * (1 - margin))
                    {
                        targetPos = 1;
                    }
                    else if (xPos < Application.Current.MainWindow.ActualWidth * margin)
                    {
                        targetPos = -1;
                    }
                    else
                    {
                        SpotifyModel.StartCentralPlayList();
                    }

                    SpotifyModel.PlayListCentreItem += targetPos;
                    if (targetPos != 0)
                    {
                        fullWidget.OnScrollClick(SpotifyModel.PlayListCentreItem * itemSize);
                    }
                }
            }
        }

        #endregion
    }
}