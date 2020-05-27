// -----------------------------------------------------------------------
// <copyright file="SettingBase.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class SettingBase</summary>
// -----------------------------------------------------------------------

namespace Common.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Base classe for ISettingBase implementation
    /// </summary>
    public abstract class SettingBase : ISettingsBase
    {
        #region Events

        /// <inheritdoc />
        public event EventHandler<EventArgs> SettingsUpdated;

        #endregion

        #region Public methodes
        
        /// <inheritdoc />
        public void GenerateDefaultSettings()
        {
            foreach (PropertyInfo item in this.GetType().GetProperties())
            {           
                item.SetValue(this, item.PropertyType.GetConstructor(Type.EmptyTypes).Invoke(new object[] { }));

                ((ISettingItemBase)(item.GetValue(this, null))).Name = item.Name;
                ((ISettingItemBase)(item.GetValue(this, null))).StringValue = GetProperty("Default" + item.Name);
                ((ISettingItemBase)(item.GetValue(this, null))).InitializeFields(GetResources());
            }
        }

        /// <inheritdoc />
        public List<ISettingItemBase> GetSettingsList()
        {
            List<ISettingItemBase> ret = new List<ISettingItemBase>();
            foreach (PropertyInfo item in this.GetType().GetProperties())
            {
                ISettingItemBase setting = (ISettingItemBase)item.GetValue(this, null);
                TranslateSetting(setting);
                ret.Add(setting);
            }

            return ret;
        }

        /// <inheritdoc />
        public void SetSettingsList(List<ISettingItemBase> settings)
        {
            if (settings?.Count == GetSettingsList().Count)
            {
                int index = 0;
                foreach (PropertyInfo item in this.GetType().GetProperties())
                {
                    ((ISettingItemBase)(item.GetValue(this, null))).StringValue = settings[index].StringValue;
                    index++;
                }
            };

            SettingsUpdated?.Invoke(this, null);
        }

        #endregion

        #region Private methodes

        /// <summary>
        /// Gets resources class for translation
        /// </summary>
        /// <returns>Resources class for translation</returns>
        protected abstract Type GetResources();

        /// <summary>
        /// Translate a setting name
        /// </summary>
        /// <param name="setting">Setting</param>
        private void TranslateSetting(ISettingItemBase setting)
        {
            setting.Translation = GetProperty(setting.Name);
        }

        /// <summary>
        /// Try to get properties if it exists
        /// </summary>
        /// <param name="setting">Propertie name</param>
        private string GetProperty(string name)
        {
            string ret = name;
            try
            {
                ret = GetResources().GetProperty(name).GetValue(null) as string ?? name;
            }
            catch
            {
            }

            return ret;
        }

        #endregion
    }
}
