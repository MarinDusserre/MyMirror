// -----------------------------------------------------------------------
// <copyright file="SpotifyModel.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class SpotifyModel</summary>
// -----------------------------------------------------------------------

namespace SpotifyWidget.Model
{
    using SpotifyWidget.Properties;
    using System;
    using System.Threading.Tasks;
    using System.Timers;
    using SpotifyAPI.Web;
    using SpotifyAPI.Web.Auth;
    using SpotifyAPI.Web.Enums;
    using SpotifyAPI.Web.Models;
    using System.Threading;
    using Common.ViewModel;
    using Common.Log;
    using Common.Settings;
    using System.Collections.Generic;

    /// <summary>
    /// Contains Tram widget model
    /// </summary>
    internal class SpotifyModel : ObservableObject
    {
        #region Properties
        
        /// <summary>
        /// Gets windget settings
        /// </summary>
        public SettingsManager<SpotifySettings> SettingsManager { get; internal set; } 

        /// <summary>
        /// Is music running
        /// </summary>
        public bool IsPlaying { get => _isPlaying; private set => Set(ref _isPlaying, value); }

        /// <summary>
        /// Gets song title
        /// </summary>
        public string SongTitle { get => _songTitle; private set => Set(ref _songTitle, value); }

        /// <summary>
        /// Gets artist name
        /// </summary>
        public string Artist { get => _artist; private set => Set(ref _artist, value); }

        /// <summary>
        /// Gets song duration
        /// </summary>
        public int SongDuration { get => _songDuration; private set => Set(ref _songDuration, value); }

        /// <summary>
        /// Gets the visibility of music panel
        /// </summary>
        public bool ShowMusicManagementPanel { get => _showMusicManagementPanel; private set => Set(ref _showMusicManagementPanel, value); }

        /// <summary>
        /// Gets the visibility of playlist panel
        /// </summary>
        public bool ShowPlaylistManagementPanel { get => _showPlaylistManagementPanel; private set => Set(ref _showPlaylistManagementPanel, value); }

        /// <summary>
        /// Gets the switch button text
        /// </summary>
        public string SwitchWindowButton { get => _switchWindowButton; private set => Set(ref _switchWindowButton, value); }

        /// <summary>
        /// Gets the visibility of next arow
        /// </summary>
        public bool ShowNextArrow
        {
            get => _showNextArrow;
            private set => Set(ref _showNextArrow, value);
        }

        /// <summary>
        /// Gets the visibility of previous arow
        /// </summary>
        public bool ShowPreviousArrow
        {
            get => _showPreviousArrow;
            private set => Set(ref _showPreviousArrow, value);
        }

        /// <summary>
        /// Gets song progress
        /// </summary>
        public int SongProgress
        {
            get => _songProgress;
            private set
            {
                Set(ref _songProgress, value);
                NotifyPropertyChanged(nameof(SongProgressPercent));
            }
        }

        /// <summary>
        /// Gets song progress in percent
        /// </summary>
        public float SongProgressPercent { get => _songDuration == 0 ? 0 : (float)_songProgress / (float)_songDuration; }

        /// <summary>
        /// Gets playlists list
        /// </summary>
        public List<PlaylistItem> Playlists { get => _playlists; private set => Set(ref _playlists, value); }

        /// <summary>
        /// Gets or sets center Playlist index
        /// </summary>
        public int PlayListCentreItem
        {
            get => _playListCentreItem;
            set
            {
                _playListCentreItem = value >= 0 && value < _playlists?.Count? value : _playListCentreItem;
                ShowPreviousArrow = _playListCentreItem != 0;
                ShowNextArrow = _playListCentreItem != (_playlists?.Count -1);
            }
        }

        #endregion

        #region Private members

        /// <summary>
        /// Refresh timer
        /// </summary>
        private System.Timers.Timer _timer;

        /// <summary>
        /// Is music running
        /// </summary>
        private bool _isPlaying;

        /// <summary>
        /// Song title
        /// </summary>
        private string _songTitle;

        /// <summary>
        /// Artist name
        /// </summary>
        private string _artist;

        /// <summary>
        /// Song duration
        /// </summary>
        private int _songDuration; 

        /// <summary>
        /// Song progress
        /// </summary>
        private int _songProgress;

        /// <summary>
        /// Visibility of music panel
        /// </summary>
        private bool _showMusicManagementPanel;

        /// <summary>
        /// Visibility of playlist panel
        /// </summary>
        private bool _showPlaylistManagementPanel;

        /// <summary>
        /// Switch button text
        /// </summary>
        private string _switchWindowButton;

        /// <summary>
        /// Instance de l'objet web api
        /// </summary>
        private static SpotifyWebAPI _spotifyWebAPI;

        /// <summary>
        /// API access mutex
        /// </summary>
        private readonly Mutex _accessMutex;

        /// <summary>
        /// Expected sound volume
        /// </summary>
        private int _expectedSoundVolume;

        /// <summary>
        /// Gets the visibilit of next arow
        /// </summary>
        private bool _showNextArrow;

        /// <summary>
        /// Gets the visibilit of previous arow
        /// </summary>
        private bool _showPreviousArrow;

        /// <summary>
        /// If the play context has just changed
        /// </summary>
        private bool _contextJustChanged;

        /// <summary>
        /// Playlists list
        /// </summary>
        private List<PlaylistItem> _playlists;

        /// <summary>
        /// Center Playlist index
        /// </summary>
        private int _playListCentreItem;

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructeur
        /// </summary>
        public SpotifyModel()
        {
            SettingsManager = new SettingsManager<SpotifySettings>();
            SettingsManager.Initialize(Resources.SettingsFileName);

            ShowMusicManagementPanel = true;
            ShowPlaylistManagementPanel = false;
            SwitchWindowButton = Resources.GoToPlaylist;
            PlayListCentreItem = 0;

            _accessMutex = new Mutex();

            _timer = new System.Timers.Timer(1000)
            {
                AutoReset = false
            };
            _timer.Elapsed += Refresh;
        }

        #endregion

        #region Public methodes

        /// <summary>
        /// Performs a first update
        /// </summary>
        public void Initialize()
        {
            new Task( async () =>
            {
                try
                {
                    string clientId = SettingsManager.Settings.ClientId.Value;
                    string userId = SettingsManager.Settings.UserId.Value;
                    
                    LogManager.LogLine("Spotify initialization with clientId=" + clientId +  "userId=" + userId);

                    WebAPIFactory webAPIFactory = new WebAPIFactory(
                        Resources.Url,
                        8000,
                        clientId,
                        Scope.UserModifyPlaybackState,
                        TimeSpan.FromSeconds(20));
                    try
                    {
                        _spotifyWebAPI = await webAPIFactory.GetWebApi(false);
                        Paging<SimplePlaylist> playlists = _spotifyWebAPI.GetUserPlaylists(userId);
                        List<PlaylistItem> tempPlaylists = new List<PlaylistItem>();
                        foreach (SimplePlaylist playlist in playlists.Items)
                        {
                            tempPlaylists.Add(new PlaylistItem(playlist.Id, playlist.Name, playlist.Images[0].Url));
                        }
                        foreach (string playlist in SettingsManager.Settings.OtherPlayLists.Value.Split(';'))
                        {
                            PlaylistItem item = getPlaylistItem(playlist);
                            if(item != null)
                            {
                                tempPlaylists.Add(item);
                            }
                        }
                        Playlists = tempPlaylists;

                    }
                    catch (Exception ex)
                    {
                        SongTitle = Resources.Error;
                        LogManager.LogLine(ex.Message);
                    }

                    Refresh(null, null);
                    LogManager.LogLine("Spotify initialization OK");
                }
                catch (Exception ex)
                {
                    LogManager.LogLine("Spotify initialization Error");
                    LogManager.LogLine(ex.Message);
                }
                _timer.Start();
            }
            ).Start();  
        }

        /// <summary>
        /// Play or pose music
        /// </summary>
        public void Play()
        {
            IsPlaying = !IsPlaying;

            new Task(() =>
            {
                try
                {
                    LogManager.LogLine("Play");

                    if (_accessMutex.WaitOne(1000))
                    {
                        PlaybackContext context = _spotifyWebAPI.GetPlayback();
                        if (context.Device != null)
                        {
                            ErrorResponse err;
                            if (context.IsPlaying)
                            {
                                err = _spotifyWebAPI.PausePlayback();
                                context.IsPlaying = false;
                            }
                            else
                            {
                                err = _spotifyWebAPI.ResumePlayback();
                                context.IsPlaying = true;
                            }

                            _contextJustChanged = true;
                        }
                        _accessMutex.ReleaseMutex();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogLine(ex.Message);
                    _accessMutex.ReleaseMutex();
                }
            }).Start();
        }

        /// <summary>
        /// Go to previous music
        /// </summary>
        public void Previous()
        {
            new Task(() =>
            {
                try
                {
                    LogManager.LogLine("Previous");

                    if (_accessMutex.WaitOne(1000))
                    {
                        ErrorResponse err = _spotifyWebAPI.SkipPlaybackToPrevious();
                        _accessMutex.ReleaseMutex();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogLine(ex.Message);
                    _accessMutex.ReleaseMutex();
                }
            }).Start();
        }

        /// <summary>
        /// Go to next music
        /// </summary>
        public void Next()
        {
            new Task(() =>
            {
                try
                {
                    LogManager.LogLine("Next");

                    if (_accessMutex.WaitOne(1000))
                    {
                        ErrorResponse err = _spotifyWebAPI.SkipPlaybackToNext();
                        _accessMutex.ReleaseMutex();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogLine(ex.Message);
                    _accessMutex.ReleaseMutex();
                }
            }).Start();
        }

        /// <summary>
        /// Go to next music
        /// </summary>
        public void StartCentralPlayList()
        {
            new Task(() =>
            {
                try
                {
                    LogManager.LogLine("StartCentralPlayList");

                    if (_accessMutex.WaitOne(1000))
                    {
                        PlaybackContext context = _spotifyWebAPI.GetPlayback();
                        if (context.Device != null)
                        {
                            ErrorResponse err = _spotifyWebAPI.ResumePlayback(context.Device.Id, Resources.PlaylistsId + Playlists[_playListCentreItem].Id);
                        }
                        _accessMutex.ReleaseMutex();
                        SwitchView();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogLine(ex.Message);
                    _accessMutex.ReleaseMutex();
                }
            }).Start();
        }

        
        /// <summary>
        /// Set sound
        /// </summary>
        /// <param name="volume">Volume in percent</param>
        public void SetSound(int volume)
        {
            _expectedSoundVolume = volume;
        }

        /// <summary>
        /// Switch view
        /// </summary>
        public void SwitchView()
        {
            bool playlistShown = _showPlaylistManagementPanel;
            ShowMusicManagementPanel = playlistShown;
            ShowPlaylistManagementPanel = !playlistShown;
            SwitchWindowButton = _showMusicManagementPanel ? Resources.GoToPlaylist : Resources.GoToMusicManagement;
        }

        #endregion

        #region Private methodes

        /// <summary>
        /// Generate a playlist item from playlist id
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns></returns>
        private PlaylistItem getPlaylistItem(string playlist)
        {
            LogManager.LogLine("Try to get playlist " + playlist);
            PlaylistItem item = null;
            try
            {
                FullPlaylist fullPlaylist = _spotifyWebAPI.GetPlaylist(SettingsManager.Settings.UserId.Value, playlist);
                item = new PlaylistItem(fullPlaylist.Id, fullPlaylist.Name, fullPlaylist.Images[0].Url);
            }
            catch(Exception ex)
            {
                LogManager.LogLine("Error" + ex.Message);
            }
            return item;
        }

        /// <summary>
        /// Updates tram timers
        /// </summary>
        /// <param name="s">Sender</param>
        /// <param name="e">Arguments</param>
        private void Refresh(object s, ElapsedEventArgs e)
        {
            new Task(() =>
            {
                bool error = false;

                try
                {
                    if (_accessMutex.WaitOne(1))
                    {
                        PlaybackContext context = _spotifyWebAPI.GetPlayback();
                        if (context.Item != null)
                        {
                            SongTitle = context.Item.Name;

                            SongDuration = context.Item.DurationMs / 1000;
                            SongProgress = context.ProgressMs / 1000;

                            IsPlaying = !_contextJustChanged ? context.IsPlaying : IsPlaying;
                            _contextJustChanged = false;
                            Artist = context.Item.Artists[0]?.Name;

                            AvailabeDevices devices = _spotifyWebAPI.GetDevices();

                            if (devices?.Devices[0]?.VolumePercent != _expectedSoundVolume)
                            {
                                _spotifyWebAPI.SetVolume(_expectedSoundVolume);
                            }
                        }

                        _accessMutex.ReleaseMutex();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.LogLine(ex.Message);
                    _accessMutex.ReleaseMutex();
                    error = true;
                }

                // If token is lost, get it again
                if (_spotifyWebAPI == null || error && _spotifyWebAPI.AccessToken == null)
                {
                    Initialize();
                }
                else
                {
                    _timer.Start();
                }
            }
            ).Start();
        }

        #endregion
    }
}