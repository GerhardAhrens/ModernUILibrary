//-----------------------------------------------------------------------
// <copyright file="IHandler.cs" company="Lifeprojects.de">
//     Class: IHandler
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.10.2022</date>
//
// <summary>Interface Definition of Logger IHandler Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Logger
{
    /// <summary>
    /// Handler interface.
    /// </summary>
    public interface IHandler
    {
        void Push(Record record);

        void Flush();

        string DefaultLogFilename(Record record);
    }
}
