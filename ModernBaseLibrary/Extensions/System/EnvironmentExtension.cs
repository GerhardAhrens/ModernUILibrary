
//-----------------------------------------------------------------------
// <copyright file="EnvironmentExtension.cs" company="Lifeprojects.de">
//     Class: EnvironmentExtension
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.01.2023</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Extension
{
    public static class EnvironmentExtension
    {
        /// <summary>An Environment.SpecialFolder extension method that gets folder path.</summary>
        /// <param name="this">this.</param>
        /// <returns>The folder path.</returns>
        public static string GetFolderPath(this Environment.SpecialFolder @this)
        {
            return Environment.GetFolderPath(@this);
        }

        /// <summary>An Environment.SpecialFolder extension method that gets folder path.</summary>
        /// <param name="this">this.</param>
        /// <param name="option">The option.</param>
        /// <returns>The folder path.</returns>
        public static string GetFolderPath(this Environment.SpecialFolder @this, Environment.SpecialFolderOption option)
        {
            return Environment.GetFolderPath(@this, option);
        }
    }
}