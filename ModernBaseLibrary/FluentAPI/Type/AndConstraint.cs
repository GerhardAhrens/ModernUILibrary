//-----------------------------------------------------------------------
// <copyright file="AndConstraint.cs" company="Lifeprojects.de">
//     Class: AndConstraint
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.04.2021</date>
//
// <summary>
// </summary>
//-----------------------------------------------------------------------
namespace ModernBaseLibrary.FluentAPI
{
    public class AndConstraint<T>
    {
        public T And { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndConstraint{T}"/> class.
        /// </summary>
        public AndConstraint(T parentConstraint)
        {
            And = parentConstraint;
        }
    }
}
