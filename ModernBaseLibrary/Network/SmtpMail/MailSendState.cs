//-----------------------------------------------------------------------
// <copyright file="MailSendState.cs" company="Lifeprojects.de">
//     Class: MailSendState
//     Copyright � Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.03.2013</date>
//
// <summary>
// Enum-Klasse gibt den Status eines Mailversand per SMTP zur�ck (SMTP-Mailer)
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.SMTP
{
    using System.ComponentModel;

    public enum MailSendState : int
    {
        None = 0,
        MailSendOK = 1,
        [Description("Username/Passwort Login Fehlerhaft")]
        NetworkCredentialError = 2,
        [Description("Es ist ein Fehler mit dem Mailanhang aufgetreten")]
        AttachmentError = 3,
        [Description("Es ist ein allgemeinder Fehler beim mailversand aufgetreten")]
        GeneralMailSendError = 4,
        [Description("Es wurde keine Empf�ngermailadresse angegeben")]
        NoToAdress = 5,
        [Description("Es wurde keine Versendermailadresse angegeben")]
        NoFromAdress = 6,
        [Description("Es wurde kein Mailtitel angegeben")]
        NoSubject = 7,
        [Description("Der Benutzer zum versenden des Mails ist ung�ltig")]
        AuthenticUserNoValid = 8
    }
}