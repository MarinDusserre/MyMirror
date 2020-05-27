// -----------------------------------------------------------------------
// <copyright file="PlaylistItem.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class PlaylistItem</summary>
// -----------------------------------------------------------------------

namespace SpotifyWidget.Model
{
    using Common.ViewModel;

    /// <summary>
    /// Playlist item
    /// </summary>
    public class PlaylistItem : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the playlist Id
        /// </summary>
        public string Id
        {
            get => _id;
            private set => Set(ref _id, value);
        }

        /// <summary>
        /// Gets or Sets the playlist name
        /// </summary>
        public string Name
        {
            get => _name;
            private set => Set(ref _name, value);
        }


        /// <summary>
        /// Gets or Sets the playlist picture
        /// </summary>
        public string Image
        {
            get => _image;
            private set => Set(ref _image, value);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Gets or Sets the playlist Id
        /// </summary>
        private string _id;

        /// <summary>
        /// Gets or Sets the playlist name
        /// </summary>
        private string _name;

        /// <summary>
        /// Gets or Sets the playlist picture
        /// </summary>
        private string _image;

        #endregion

        #region Contructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PlaylistItem(string id, string name, string image)
        {
            _id = id;
            _name = name;
            _image = image;
        }

        #endregion
    }
}
