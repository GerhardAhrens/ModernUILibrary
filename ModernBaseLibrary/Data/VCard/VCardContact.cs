//-----------------------------------------------------------------------
// <copyright file="VCardContact.cs" company="Lifeprojects.de">
//     Class: VCardContact
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>06.08.2022</date>
//
// <summary>
// Create vCard Export
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.VCard
{
    using System;
    using System.Collections.Generic;

    public class VCardContact
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FormattedName { get; set; }

        public string Organization { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string Photo { get; set; }

        public List<VCardPhone> Phones { get; set; }

        public List<VCardAddress> Addresses { get; set; }
    }

    public class VCardPhone
    {
        public string Number { get; set; }
        public string Type { get; set; }
    }

    public class VCardAddress
    {
        public string Description { get; set; }
        public string Type { get; set; }
    }
}

