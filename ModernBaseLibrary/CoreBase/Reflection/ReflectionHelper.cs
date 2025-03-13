//-----------------------------------------------------------------------
// <copyright file="ReflectionHelper.cs" company="Lifeprojects.de">
//     Class: ReflectionHelper
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>13.03.2025 11:26:56</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.CoreBase.Reflection
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class RootAssemblyHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RootAssemblyHelper"/> class.
        /// </summary>
        static RootAssemblyHelper()
        {
        }

        public static string RootAssemblyName { get; set; } = "ModernBaseLibrary";

        public static string AssemblyName { get; private set; }

        public static string RootAssemblyLocation { get; private set; }

        public static Assembly RootAssembly { get { return GetRootAssembly(RootAssemblyName); } }

        private static Assembly GetRootAssembly(string rootAssemplyName = "ModernBaseLibrary")
        {
            Assembly loadedAssembly = null;

            try
            {
                if (AppDomain.CurrentDomain != null)
                {
                    
                    loadedAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(w => w.FullName.Contains(rootAssemplyName) == true);
                    if (loadedAssembly != null)
                    {
                        RootAssemblyLocation = loadedAssembly.Location;
                        AssemblyName = Path.GetFileName(loadedAssembly.Location);
                    }
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }

            return loadedAssembly;
        }
    }
}
