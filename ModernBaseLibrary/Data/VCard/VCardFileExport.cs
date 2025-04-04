﻿//-----------------------------------------------------------------------
// <copyright file="VCardFileExport.cs" company="Lifeprojects.de">
//     Class: VCardFileExport
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
    using System.Text;

    using ModernBaseLibrary.Graphics;

    public static class VCardFileExport
    {
        const string NewLine = "\r\n";
        const string Separator = ";";
        const string Header = "BEGIN:VCARD\r\nVERSION:2.1";
        const string Name = "N:";
        const string FormattedName = "FN:";
        const string OrganizationName = "ORG:";
        const string TitlePrefix = "TITLE:";
        const string PhotoPrefix = "PHOTO;ENCODING=BASE64;JPEG:";
        const string PhonePrefix = "TEL;TYPE=";
        const string PhoneSubPrefix = ",VOICE:";
        const string AddressPrefix = "ADR;TYPE=";
        const string AddressSubPrefix = ":;;";
        const string EmailPrefix = "EMAIL:";
        const string Footer = "END:VCARD";

        public static string CreateVCard(VCardContact contact)
        {
            StringBuilder fw = new StringBuilder();
            fw.Append(Header);
            fw.Append(NewLine);

            //Full Name
            if (!string.IsNullOrEmpty(contact.FirstName) || !string.IsNullOrEmpty(contact.LastName))
            {
                fw.Append(Name);
                fw.Append(contact.LastName);
                fw.Append(Separator);
                fw.Append(contact.FirstName);
                fw.Append(Separator);
                fw.Append(NewLine);
            }

            //Formatted Name
            if (!string.IsNullOrEmpty(contact.FormattedName))
            {
                fw.Append(FormattedName);
                fw.Append(contact.FormattedName);
                fw.Append(NewLine);
            }

            //Organization name
            if (!string.IsNullOrEmpty(contact.Organization))
            {
                fw.Append(OrganizationName);
                fw.Append(contact.Organization);
                fw.Append(NewLine);

            }

            //Title
            if (!string.IsNullOrEmpty(contact.Title))
            {
                fw.Append(TitlePrefix);
                fw.Append(contact.Title);
                fw.Append(NewLine);
            }

            //Photo
            if (!string.IsNullOrEmpty(contact.Photo))
            {
                string photo = ImageUrlHelper.ConvertImageURLToBase64(contact.Photo);
                if (string.IsNullOrEmpty(photo) == false)
                {
                    fw.Append(PhotoPrefix);
                    fw.Append(photo);
                    fw.Append(NewLine);
                    fw.Append(NewLine);
                }
            }

            //Phones
            foreach (var item in contact.Phones)
            {
                fw.Append(PhonePrefix);
                fw.Append(item.Type);
                fw.Append(PhoneSubPrefix);
                fw.Append(item.Number);
                fw.Append(NewLine);
            }

            //Addresses
            foreach (var item in contact.Addresses)
            {
                fw.Append(AddressPrefix);
                fw.Append(item.Type);
                fw.Append(AddressSubPrefix);
                fw.Append(item.Description);
                fw.Append(NewLine);
            }

            //Email
            if (!string.IsNullOrEmpty(contact.Email))
            {
                fw.Append(EmailPrefix);
                fw.Append(contact.Email);
                fw.Append(NewLine);
            }

            fw.Append(Footer);

            return fw.ToString();
        }
    }
}
