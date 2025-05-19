//-----------------------------------------------------------------------
// <copyright file="DialogResource.cs" company="Lifeprojects.de">
//     Class: DialogResource
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>19.05.2025 13:56:59</date>
//
// <summary>
// Die Klasse liest über ResourceXAML einen Wert aus ApplicationResource_DE.xaml und gibt diesen zurück.
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core.Resource
{
    public class DialogResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DialogResource"/> class.
        /// </summary>
        public DialogResource(string dialogTitle)
        {
            this.DialogTitle = dialogTitle;
        }

        public string DialogTitle { get; private set; }
    }
}
