// -----------------------------------------------------------------------
// <copyright file="PluginLoader.cs">
// Made by Marin DUSSERRE, 2020
// </copyright>
// <summary>Contains class PluginLoader</summary>
// -----------------------------------------------------------------------

namespace MyMirror.Model
{
    using Common.Log;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Plugins loading class Search and load all DLL file present in a specific folder and check if
    /// it's one plugin of application.
    /// </summary>
    /// <typeparam name="T">Plugin type</typeparam>
    internal class PluginLoader <T>
    {
        #region Properties

        /// <summary>
        /// Load all plugins present in the specified path
        /// </summary>
        /// <param name="path">Folder to analyze</param>
        /// <param name="format">File format to analyze</param>
        /// <returns>List of widgets</returns>
        public static ICollection<T> LoadPlugins(string path, string format = "*.dll")
        {
            ICollection<T> widgets = new List<T>();
            if (Directory.Exists(path))
            {
                string[] dllFileNames = Directory.GetFiles(path, format);

                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (string dllFile in dllFileNames)
                {
                    try
                    {
                        AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                        Assembly assembly = Assembly.Load(an);
                        assemblies.Add(assembly);
                    }
                    catch (BadImageFormatException e)
                    {
                        LogManager.LogLine(e.Message);
                    }
                }

                Type pluginType = typeof(T);
                ICollection<Type> pluginTypes = new List<Type>();
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly != null)
                    {
                        foreach (Type type in assembly.GetTypes())
                        {
                            if (!type.IsInterface && !type.IsAbstract)
                            {
                                if (type.GetInterface(pluginType.FullName) != null)
                                {
                                    pluginTypes.Add(type);
                                }
                            }
                        }
                    }
                }

                widgets = new List<T>(pluginTypes.Count);
                foreach (Type type in pluginTypes)
                {
                    T plugin = (T)Activator.CreateInstance(type);
                    widgets.Add(plugin);
                }
            }

            return widgets;
        }

        #endregion
    }
}
