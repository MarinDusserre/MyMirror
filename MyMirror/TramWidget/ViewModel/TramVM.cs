// -----------------------------------------------------------------------
// <copyright file="TramVM.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class TramVM</summary>
// -----------------------------------------------------------------------

namespace TramWidget.ViewModel
{
    using TramWidget.Model;
    using Common.ViewModel;

    /// <summary>
    /// Contains Tram widget view model
    /// </summary>
    internal class TramVM : ViewModelBase
    {
        #region Properties

        /// <summary>
        /// Gets Tram Model
        /// </summary>
        public TramModel TramModel { get; private set; }

        #endregion

        #region Contructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public TramVM()
        {
            TramModel = new TramModel();
        }

        #endregion

        #region Public methodes

        /// <summary>
        /// Initialize model
        /// </summary>
        public void Initialize()
        {
            TramModel.Initialize();
        }

        #endregion
    }
}
