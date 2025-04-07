//-----------------------------------------------------------------------
// <copyright file="IFactoryArgs.cs" company="Lifeprojects.de">
//     Class: IFactoryArgs
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

namespace ModernInsideVM.Core
{
    using ModernInsideVM.Core;

    public interface IFactoryArgs
    {
        public CommandButtons MenuButton { get; set; }
        public string Sender { get; set; }

        public Guid EntityId { get; set; }

        public bool IsNew { get; set; }

        public bool IsRefresh { get; set; }

        public int RowPosition { get; set; }
    }
}
