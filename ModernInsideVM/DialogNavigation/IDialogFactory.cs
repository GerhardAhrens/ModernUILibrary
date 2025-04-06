//-----------------------------------------------------------------------
// <copyright file="IDialogFactory.cs" company="Lifeprojects.de">
//     Class: IDialogFactory
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>25.03.2025 09:20:07</date>
//
// <summary>
// Interface Class for 
// </summary>
//-----------------------------------------------------------------------

namespace ModernInsideVM.Core
{
    public interface IDialogFactory
    {
        public abstract static FactoryResult Get(CommandButtons mainButton);

        public abstract static FactoryResult Get(CommandButtons mainButton, IFactoryArgs changeViewArgs);
    }
}
