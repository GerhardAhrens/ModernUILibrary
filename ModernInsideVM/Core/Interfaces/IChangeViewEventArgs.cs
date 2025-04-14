//-----------------------------------------------------------------------
// <copyright file="IChangeViewEventArgs.cs" company="Lifeprojects.de">
//     Class: IChangeViewEventArgs
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>25.03.2025 06:54:33</date>
//
// <summary>
// Interface Class for 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core.Interfaces
{
    using ModernTemplate.Core;

    public partial interface IChangeViewEventArgs
    {
        public CommandButtons MenuButton { get; set; }

        public string Sender { get; set; }

        public Guid EntityId { get; set; }

        public bool IsNew { get; set; }

        public bool IsRefresh { get; set; }

        public int RowPosition { get; set; }

        /// <summary>
        /// Wechsel von Dialog
        /// </summary>
        public CommandButtons FromPage { get; set; }
    }
}
