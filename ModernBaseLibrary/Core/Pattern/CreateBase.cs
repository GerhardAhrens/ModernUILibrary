//-----------------------------------------------------------------------
// <copyright file="CreateBase.cs" company="Lifeprojects.de">
//     Class: CreateBase
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>12.12.2024 08:13:20</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Linq.Expressions;

    public static class CreateBase
    {
        public static T Create<T>(this T @this) where T : class, new()
        {
            return Utility<T>.Create();
        }
    }

    public static class Utility<T> where T : class, new()
    {
        static Utility()
        {
            Create = Expression.Lambda<Func<T>>(Expression.New(typeof(T).GetConstructor(Type.EmptyTypes))).Compile();
        }

        public static Func<T> Create { get; private set; }
    }
}
