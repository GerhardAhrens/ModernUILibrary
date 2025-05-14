//-----------------------------------------------------------------------
// <copyright file="RadioButtonEx.cs" company="Lifeprojects.de">
//     Class: RadioButtonEx
//     Copyright © Gerhard Ahrens, 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>27.09.2018</date>
//
// <summary>
//     Die Klasse liest aus /themes/generic.xaml die Resourcen aus
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;

    using ModernBaseLibrary.Core;

    public class ResourceReader : SingletonCoreBase<ResourceReader>
    {
        private const string RESNAME = "themes/generic.xaml";
        private string assemblyName = string.Empty;
        private ResourceDictionary resourceDictionary = null;

        private ResourceReader()
        {
            this.Init();
        }

        public int Count { get; private set; }

        public string ResourceName { get; private set; }

        public IEnumerable<ResourceDictionary> GetResourceCollection(string filter = null)
        {
            IEnumerable<ResourceDictionary> resCollection = null;

            try
            {
                if (string.IsNullOrEmpty(filter) == false)
                {
                    if (this.Read(RESNAME).MergedDictionaries.Any(w => w.Source.ToString().Contains(filter) == true) == true)
                    {
                        resCollection = this.Read(RESNAME).MergedDictionaries.Where(w => w.Source.ToString().Contains(filter) == true);
                    }
                }
                else
                {
                    resCollection = this.Read(RESNAME).MergedDictionaries;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return resCollection;
        }

        public T ReadAs<T>(string resourceName)
        {
            T result = default(T);

            try
            {
                ResourceDictionary resourceDictionary = Read(RESNAME);
                if (resourceDictionary != null)
                {
                    if (resourceDictionary.Contains(resourceName) == true)
                    {
                        var style = resourceDictionary[resourceName];
                        if (style != null)
                        {
                            if (typeof(T) == typeof(Brush))
                            {
                                result = (T)resourceDictionary[resourceName];
                            }
                            else if (typeof(T) == typeof(ImageSource))
                            {
                                result = (T)resourceDictionary[resourceName];
                            }
                            else
                            {
                                result = (T)Convert.ChangeType(style, typeof(T));
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public T ReadAs<T>(string resourceName, string resourceDict, Assembly assembly = null)
        {
            T result = default(T);

            try
            {
                if (assembly != null)
                {
                    assembly = Assembly.GetExecutingAssembly();
                }
                else
                {
                    assembly = Assembly.GetEntryAssembly();
                }

                ResourceDictionary resourceDictionary = ReadEx(resourceDict, assembly);
                if (resourceDictionary != null)
                {
                    if (resourceDictionary.Contains(resourceName) == true)
                    {
                        var style = resourceDictionary[resourceName];
                        if (style != null)
                        {
                            if (typeof(T) == typeof(Brush))
                            {
                                result = (T)resourceDictionary[resourceName];
                            }
                            else if (typeof(T) == typeof(ImageSource))
                            {
                                result = (T)resourceDictionary[resourceName];
                            }
                            else
                            {
                                result = (T)Convert.ChangeType(style, typeof(T));
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public Brush ReadAsBrush(string styleName)
        {
            Brush result = null;

            try
            {
                ResourceDictionary resourceDictionary = this.Read(RESNAME);
                if (resourceDictionary != null)
                {
                    if (resourceDictionary.Contains(styleName) == true)
                    {
                        result = (Brush)resourceDictionary[styleName];
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        private void Init()
        {
            if (UnitTestDetector.IsInUnitTest == true)
            {
                assemblyName = Assembly.GetCallingAssembly().GetName().Name;
            }
            else
            {
                assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            }
        }

        private ResourceDictionary Read(string resName)
        {
            try
            {
                if (resourceDictionary == null)
                {
                    resourceDictionary = new ResourceDictionary();
                    resourceDictionary.Source = new Uri($"{assemblyName};component/{resName}", UriKind.RelativeOrAbsolute);

                    this.Count = resourceDictionary.Count;
                    this.ResourceName = resName;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return resourceDictionary;
        }

        private ResourceDictionary ReadEx(string resName, Assembly assembly = null)
        {
            try
            {
                if (assembly != null)
                {
                    assemblyName = assembly.GetName().Name;
                }
                else
                {
                    assemblyName = Assembly.GetEntryAssembly().GetName().Name;
                }

                resourceDictionary = new ResourceDictionary();
                resourceDictionary.Source = new Uri($"{assemblyName};component/{resName}", UriKind.RelativeOrAbsolute);
                this.Count = resourceDictionary.Count;
                this.ResourceName = resName;
            }
            catch (Exception)
            {
                throw;
            }

            return resourceDictionary;
        }
    }
}
