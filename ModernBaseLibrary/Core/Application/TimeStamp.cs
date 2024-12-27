//-----------------------------------------------------------------------
// <copyright file="TimeStamp.cs" company="Lifeprojects.de">
//     Class: TimeStamp
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>19.10.2021</date>
//
// <summary>
// Die Klasse erzeugt aus den TimeStamp-Angaben (vom ältesten Datum)
// einen string im Format 'dd.MM.yyyy HH:mm Username'
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TimeStamp : SingletonCoreBase<TimeStamp>
    {
        public string MaxEntry(DateTime createdOn, string createdBy, DateTime modifiedOn, string modifiedBy)
        {
            string result = string.Empty;
            Dictionary<DateTime, string> timeStamp = new Dictionary<DateTime, string>();
            if (timeStamp.ContainsKey(createdOn) == false)
            {
                timeStamp.Add(createdOn, createdBy);
            }

            if (timeStamp.ContainsKey(modifiedOn) == false)
            {
                timeStamp.Add(modifiedOn, modifiedBy);
            }

            KeyValuePair<DateTime, string> maxresult = timeStamp.OrderByDescending(o => o.Key).FirstOrDefault();
            result = $"{maxresult.Key.ToString("dd.MM.yyyy HH:mm")} - {maxresult.Value}";

            return result;
        }
    }
}