//-----------------------------------------------------------------------
// <copyright file="ErrorLogArgs.cs" company="Lifeprojects.de">
//     Class: ExceptionView
//     Copyright © company="Lifeprojects.de" 2019
// </copyright>
//
// <author>Gerhard Ahrens - company="Lifeprojects.de"</author>
// <email>developer@lifeprojects.de</email>
// <date>25.03.2019</date>
//
// <summary>
// Arguments for ErrorLog Class/Viewer
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;

    public class ErrorLogArgs
    {
        public string ErrorLevel { get; set; }

        public string Title { get; set; }

        public string ErrorText { get; set; }

        public string UserAction { get; set; }

        public string Version { get; set; }

        public Exception Exception { get; set; }

        public bool ApplicationExit { get; set; }
    }
}
