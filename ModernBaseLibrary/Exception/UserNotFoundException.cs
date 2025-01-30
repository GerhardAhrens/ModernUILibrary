namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    public class UserNotFoundException : BaseException
    {
        private const string MESSAGE = "Es ist ein Fehler beim prüfen eines AD-Benutzer aufgetreten.";

        public UserNotFoundException() : base(string.Empty)
        {
            this.Data.Add("Msg", MESSAGE);
            this.Data.Add("UserId", "Unbekannte Userid");
            this.ErrorLevel = ErrorLevel.Error;
        }

        public UserNotFoundException(string pMessage) : base(pMessage)
        {
            this.Data.Add("Msg", pMessage);
            this.ErrorLevel = ErrorLevel.Error;
        }

        public UserNotFoundException(string pMessage, string fileName) : base(pMessage)
        {
            this.Data.Add("Msg", pMessage);
            this.Data.Add("File", fileName);
            this.ErrorLevel = ErrorLevel.Error;
        }

        public UserNotFoundException(string pMessage, Exception pInnerException) : base(pMessage, pInnerException)
        {
            this.Data.Add("Msg", pMessage);
            this.ErrorLevel = ErrorLevel.Error;
        }

        public override IDictionary CustomMessage()
        {
            if (this.Data.IsNullOrEmpty() == true)
            {
                this.Data.Add("Msg", MESSAGE);
                this.Data.Add("UserId", "Unbekannte Userid");

                return this.Data;
            }
            else
            {
                return this.Data;
            }
        }
    }
}
