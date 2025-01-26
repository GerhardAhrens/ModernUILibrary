//-----------------------------------------------------------------------
// <copyright file="SMTPMailer.cs" company="Lifeprojects.de">
//     Class: SMTPMailer
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
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Mail;

    public sealed class SMTPMailer : IDisposable
    {
        private const string Version = "1.0";
        private readonly string smptServer = "smtp.server.de";
        private readonly int smptPort = 25;
        private readonly string host = string.Empty;
        private readonly int port = 25;
        private MailMessage mailMessage = null;
        private SmtpClient smtpClient = null;
        private bool smtpAuthenticationOK = false;

        /// <summary>
        /// Initializes a new instance of the (for access without authentication) <see cref="SMTPMailer"/> class.
        /// </summary>
        public SMTPMailer()
        {
            this.host = this.smptServer;
            this.port = this.smptPort;
            this.Authentication = AuthenticationMode.NoAuthentication;
        }

        /// <summary>
        /// Initializes a new instance of the (for access without authentication)<see cref="SMTPMailer"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="port">The port.</param>
        public SMTPMailer(string server, int port = 25)
        {
            this.host = server;
            this.port = port;
            this.Authentication = AuthenticationMode.NoAuthentication;
        }

        /// <summary>
        /// Initializes a new instance (for login access) of the <see cref="SMTPMailer"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="port">The port.</param>
        /// <param name="pUser">The p user.</param>
        /// <param name="pPassword">The p password.</param>
        public SMTPMailer(string server, int port, string pUser, string pPassword)
        {
            this.host = server;
            this.port = port;
            this.CredentialsUser = pUser;
            this.CredentialsPassword = pPassword;
            this.Authentication = AuthenticationMode.NTLMAuthentication;
        }

        /// <summary>
        /// Initializes a new instance (for login access) of the <see cref="SMTPMailer"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="pUser">The p user.</param>
        /// <param name="pPassword">The p password.</param>
        public SMTPMailer(string server, string pUser, string pPassword)
        {
            this.host = server;
            this.port = 25;
            this.CredentialsUser = pUser;
            this.CredentialsPassword = pPassword;
            this.Authentication = AuthenticationMode.NTLMAuthentication;
        }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the credentials user.
        /// </summary>
        /// <value>
        /// The credentials user.
        /// </value>
        public string CredentialsUser { get; set; }

        /// <summary>
        /// Gets or sets the credentials password.
        /// </summary>
        /// <value>
        /// The credentials password.
        /// </value>
        public string CredentialsPassword { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the cc.
        /// </summary>
        /// <value>
        /// The cc.
        /// </value>
        public string CC { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is HTML.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is HTML; otherwise, <c>false</c>.
        /// </value>
        public bool IsHtml { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public MailPriority Priority { get; set; } = MailPriority.Normal;

        /// <summary>
        /// Gets or sets the name of the list of attachment.
        /// </summary>
        /// <value>
        /// The name of the list of attachment.
        /// </value>
        public List<string> ListOfAttachmentName { get; set; }

        /// <summary>
        /// Gets the count of attachment.
        /// </summary>
        /// <value>
        /// The count of attachment.
        /// </value>
        public int CountOfAttachment
        {
            get
            {
                int countAttachments = -1;
                if (this.ListOfAttachmentName != null)
                {
                    countAttachments = this.ListOfAttachmentName.Count;
                }

                return countAttachments;
            }
        }

        /// <summary>
        /// Gets or sets the send using.
        /// </summary>
        /// <value>
        /// The send using.
        /// </value>
        public SendUsingMode SendUsing { get; set; } = SendUsingMode.Network;

        /// <summary>
        /// Gets or sets the authentication.
        /// </summary>
        /// <value>
        /// The authentication.
        /// </value>
        public AuthenticationMode Authentication { get; set; } = AuthenticationMode.NoAuthentication;

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <returns></returns>
        public MailSendState SendEmail()
        {
            try
            {
                this.mailMessage = new MailMessage();
                this.smtpClient = new SmtpClient(this.host);

                if (string.IsNullOrEmpty(this.From) == true)
                {
                    return MailSendState.NoFromAdress;
                }

                this.mailMessage.From = new MailAddress(this.From);

                if (string.IsNullOrEmpty(this.To) == false)
                {
                    string[] toMailAdr = this.To.Split(';');
                    if (toMailAdr.Length == 0)
                    {
                        return MailSendState.NoToAdress;
                    }
                    else
                    {
                        foreach (string itemMailAdr in toMailAdr)
                        {
                            this.mailMessage.To.Add(itemMailAdr);
                        }
                    }
                }

                if (string.IsNullOrEmpty(this.Subject) == true)
                {
                    return MailSendState.NoSubject;
                }

                this.mailMessage.Subject = this.Subject;
                this.mailMessage.IsBodyHtml = this.IsHtml;
                this.mailMessage.Body = this.Body;
                this.mailMessage.Priority = this.Priority;

                if (string.IsNullOrEmpty(this.CC) == false)
                {
                    string[] toMailCC = this.CC.Split(';');
                    if (toMailCC.Length > 0)
                    {
                        foreach (string itemMailAdr in toMailCC)
                        {
                            this.mailMessage.CC.Add(itemMailAdr);
                        }
                    }
                }

                switch ((int)this.SendUsing)
                {
                    case 0:
                        this.smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        break;
                    case 1:
                        this.smtpClient.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                        break;
                    case 2:
                        this.smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                        break;
                    default:
                        this.smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        break;
                }

                if (this.Authentication == AuthenticationMode.NTLMAuthentication || this.Authentication == AuthenticationMode.PlainText)
                {
                    this.SmtpAuthentication();
                }
                else
                {
                    this.smtpClient.UseDefaultCredentials = false;
                    this.smtpClient.EnableSsl = false;
                }

                this.smtpClient.Port = this.port;

                /* Create and add the attachment */
                if (this.ListOfAttachmentName != null && this.ListOfAttachmentName.Count > 0)
                {
                    foreach (string itemAttachment in this.ListOfAttachmentName)
                    {
                        if (string.IsNullOrEmpty(itemAttachment) == false)
                        {
                            if (File.Exists(itemAttachment) == true)
                            {
                                System.Net.Mail.Attachment mailAttachment = new System.Net.Mail.Attachment(itemAttachment);
                                this.mailMessage.Attachments.Add(mailAttachment);
                            }
                        }
                    }
                }


                try
                {
                    if (this.Authentication == AuthenticationMode.NTLMAuthentication || this.Authentication == AuthenticationMode.PlainText)
                    {
                        if (this.smtpAuthenticationOK == true)
                        {
                            this.smtpClient.Send(this.mailMessage);
                        }
                        else
                        {
                            return MailSendState.AuthenticUserNoValid;
                        }
                    }
                    else
                    {
                        this.smtpClient.Send(this.mailMessage);
                    }

                    this.smtpClient = null;
                    return MailSendState.MailSendOK;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    return MailSendState.GeneralMailSendError;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return MailSendState.GeneralMailSendError;
            }
        }

        public void Dispose()
        {
            this.ReleaseResources();
        }

        private void SmtpAuthentication()
        {
            try
            {
                NetworkCredential mailAuthentication = new NetworkCredential(this.CredentialsUser, this.CredentialsPassword);
                this.smtpClient.EnableSsl = true;
                this.smtpClient.UseDefaultCredentials = false;
                this.smtpClient.Credentials = mailAuthentication;
                this.smtpAuthenticationOK = true;
            }
            catch (Exception ex)
            {
                string errText = ex.ToString();
                this.smtpAuthenticationOK = false;
            }
        }

        /// <summary>
        /// Releases the resources.
        /// </summary>
        private void ReleaseResources()
        {
            if (this.mailMessage != null)
            {
                this.mailMessage = null;
            }

            if (this.smtpClient != null)
            {
                this.smtpClient = null;
            }

            if (this.ListOfAttachmentName != null)
            {
                this.ListOfAttachmentName.Clear();
            }
        }
    }
}