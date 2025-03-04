//-----------------------------------------------------------------------
// <copyright file="IFormatter.cs" company="Lifeprojects.de">
//     Class: IFormatter
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.10.2022</date>
//
// <summary>Interface Definition of Logger IFormatter Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Logger
{
    /// <summary>
    /// Formatter interface.
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        /// Format record to message.
        /// </summary>
        /// <param name="record"></param>
        /// <returns>Formated message</returns>
        string FormatMessage(Record record);
    }
}
