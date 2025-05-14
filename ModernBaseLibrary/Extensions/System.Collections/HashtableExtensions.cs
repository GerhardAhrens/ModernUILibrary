//-----------------------------------------------------------------------
// <copyright file="HashtableExtensions.cs" company="Lifeprojects.de">
//     Class: HashtableExtensions
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>14.05.2025 10:03:56</date>
//
// <summary>
// Extension Class for 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extensions.System.Collections
{
    using global::System.Collections;

    using ModernBaseLibrary.Extensions.System;

    public static class HashtableExtensions
    {
        public static T TryGet<T>(this Hashtable Table, string KeyName, T Default)
        {
            return Table.TryParse<T>(KeyName, out T Return) ? Return : Default;
        }

        public static bool TryParse<T>(this Hashtable Table, string KeyName, out T Output)
        {
            // set Output to default while we
            // check for all the false values
            Output = default(T);

            // not table
            if (Table == null)
            {
                return false;
            }
            else if (string.IsNullOrWhiteSpace(KeyName))
            {
                return false;
            }
            else if (!Table.Contains(KeyName))
            {
                return false;
            }
            else if (Table[KeyName].GetType() != typeof(T))
            {
                return false;
            }

            // convert
            Output = (T)Table[KeyName];
            return true;
        }
    }
}
