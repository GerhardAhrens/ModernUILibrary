//-----------------------------------------------------------------------
// <copyright file="ReferenceTypeAssertions.cs" company="Lifeprojects.de">
//     Class: ReferenceTypeAssertions
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.04.2021</date>
//
// <summary>
// Basisklasse für FluentAPI Type Extenstion
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.FluentAPI
{
    using System.Diagnostics;

    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public abstract class ReferenceTypeAssertions<TSubject, TAssertions> where TAssertions : ReferenceTypeAssertions<TSubject, TAssertions>
    {
        protected ReferenceTypeAssertions(TSubject subject)
        {
            Subject = subject;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public TSubject Subject { get; }
    }
}
