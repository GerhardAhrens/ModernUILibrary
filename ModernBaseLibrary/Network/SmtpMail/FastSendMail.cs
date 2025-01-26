//-----------------------------------------------------------------------
// <copyright file="FastSendMail.cs" company="Lifeprojects.de">
//     Class: FastSendMail
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.03.2013</date>
//
// <summary>
// Klasse zum versenden von Mails per SMTP (incl. Anhang)
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.SMTP
{
    public sealed class FastSendMail
    {
        private static readonly string SMTPServer = string.Empty;
        private static readonly int SMTPPort = 25;

        public static MailSendState Send(MailContent mailContent)
        {
            SMTPMailer mail = new SMTPMailer(SMTPServer, SMTPPort);
            mail.Authentication = AuthenticationMode.NoAuthentication;
            mail.ListOfAttachmentName = mailContent.ListOfAttachmentName;
            mail.To = mailContent.To;
            mail.From = mailContent.From;
            mail.CC = mailContent.CC;
            mail.Body = mailContent.Body;
            mail.Subject = mailContent.Subject;
            mail.IsHtml = true;
            MailSendState sendMailResult = mail.SendEmail();

            return sendMailResult;
        }
    }
}
