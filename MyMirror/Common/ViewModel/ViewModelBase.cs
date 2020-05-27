// -----------------------------------------------------------------------
// <copyright file="ViewModelBase.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class ViewModelBase</summary>
// -----------------------------------------------------------------------

namespace Common.ViewModel
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Base class for view models
    /// </summary>
    public abstract class ViewModelBase : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Gets size dictionary
        /// </summary>
        public static Dictionary<float, float> SizeDict { get; private set; }

        /// <summary>
        /// Gets main window X center positon
        /// </summary>
        public static double XCenter { get; private set; }

        /// <summary>
        /// Gets main window Y center positon
        /// </summary>
        public static double YCenter { get; private set; }

        #endregion

        #region Private members

        /// <summary>
        /// Available font sizes
        /// </summary>
        private const int _availableSize = 180;

        /// <summary>
        /// Size coeficient
        /// </summary>
        private const float _sizeCoef = 0.01f;

        #endregion

        #region Protected methodes

        /// <summary>
        /// Create size dictionary
        /// </summary>
        protected void CreateSizeDictionnary()
        {
            if (SizeDict == null)
            {
                float height = (float)Application.Current.MainWindow.ActualHeight;

                // Create new dictionnary
                Dictionary<float, float> dic = new Dictionary<float, float>();

                for (int i = 1; i < _availableSize + 1; i++)
                {
                    dic.Add(i, 16 / 9 * _sizeCoef * i * height);
                }

                SizeDict = dic;

                XCenter = SystemParameters.PrimaryScreenWidth / 2;
                YCenter = SystemParameters.PrimaryScreenHeight / 2;

                NotifyPropertyChanged(nameof(SizeDict));
            }
        }

        #endregion
    }
}