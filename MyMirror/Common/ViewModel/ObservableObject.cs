// -----------------------------------------------------------------------
// <copyright file="ObservableObject.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class ObservableObject</summary>
// -----------------------------------------------------------------------

namespace Common.ViewModel
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Provide functions for data binding
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// PropertyChanged event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// PropertyChanged notify
        /// </summary>
        /// <param name="prop">Property handled</param>
        public void NotifyPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

        #region Protected methodes

        /// <summary>
        /// Modify variable value if necessary and notify a property changed event
        /// </summary>
        /// <typeparam name="T">Type of the variable</typeparam>
        /// <param name="field">Variable to set</param>
        /// <param name="value">New value of the variable</param>
        /// <param name="propertyName">Name of the changed property</param>
        protected void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                NotifyPropertyChanged(propertyName);
            }
        }

        #endregion
    }
}
