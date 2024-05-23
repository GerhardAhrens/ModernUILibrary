//-----------------------------------------------------------------------
// <copyright file="NotifiactionBoxIcons.cs" company="Lifeprojects.de">
//     Class: NotifiactionBoxIcons
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>24.08.2018</date>
//
// <summary>Class with SystemIcons from Windows</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.Versioning;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    [SupportedOSPlatform("windows")]
    public static class NotifiactionBoxIcons
    {
        private static readonly Dictionary<string, ImageSource> instructionIcon;

        private const string ApplicationIconID = "SystemIcons.Application";
        private const string AsteriskIconID = "SystemIcons.Asterisk";
        private const string ErrorIconID = "SystemIcons.Error";
        private const string ExclamationIconID = "SystemIcons.Exclamation";
        private const string HandIconID = "SystemIcons.Hand";
        private const string InformationIconID = "SystemIcons.Information";
        private const string QuestionIconID = "SystemIcons.Question";
        private const string WarningIconID = "SystemIcons.Warning";
        private const string WinLogoIconID = "SystemIcons.WinLogo";

        static NotifiactionBoxIcons()
        {
            instructionIcon = new Dictionary<string, ImageSource>();

            var systemIcons = GetSystemIcons();
            foreach (var key in systemIcons.Keys)
            {
                instructionIcon[key] = Imaging.CreateBitmapSourceFromHIcon(systemIcons[key].Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
        }

        public static ImageSource Application
        {
            get { return instructionIcon[ApplicationIconID]; }
        }

        public static ImageSource Asterisk
        {
            get { return instructionIcon[AsteriskIconID]; }
        }

        public static ImageSource Error
        {
            get { return instructionIcon[ErrorIconID]; }
        }

        public static ImageSource Exclamation
        {
            get { return instructionIcon[ExclamationIconID]; }
        }

        public static ImageSource Hand
        {
            get { return instructionIcon[HandIconID]; }
        }

        public static ImageSource Information
        {
            get { return instructionIcon[InformationIconID]; }
        }

        public static ImageSource Question
        {
            get { return instructionIcon[QuestionIconID]; }
        }

        public static ImageSource Warning
        {
            get { return instructionIcon[WarningIconID]; }
        }

        public static ImageSource WinLogo
        {
            get { return instructionIcon[WinLogoIconID]; }
        }

        public static ImageSource GetIcon(NotifiactionIcon icon)
        {
            if (icon == NotifiactionIcon.Application)
            {
                return instructionIcon[ApplicationIconID];
            }
            else if (icon == NotifiactionIcon.Asterisk)
            {
                return instructionIcon[AsteriskIconID];
            }
            else if (icon == NotifiactionIcon.Error)
            {
                return instructionIcon[ErrorIconID];
            }
            else if (icon == NotifiactionIcon.Exclamation)
            {
                return instructionIcon[ExclamationIconID];
            }
            else if (icon == NotifiactionIcon.Hand)
            {
                return instructionIcon[HandIconID];
            }
            else if (icon == NotifiactionIcon.Information)
            {
                return instructionIcon[InformationIconID];
            }
            else if (icon == NotifiactionIcon.Question)
            {
                return instructionIcon[QuestionIconID];
            }
            else if (icon == NotifiactionIcon.Warning)
            {
                return instructionIcon[WarningIconID];
            }
            else if (icon == NotifiactionIcon.WinLogo)
            {
                return instructionIcon[WinLogoIconID];
            }
            else
            {
                return instructionIcon[WinLogoIconID];
            }
        }

        private static Dictionary<string, System.Drawing.Icon> GetSystemIcons()
        {
            var systemIcons = new Dictionary<string, System.Drawing.Icon>();
            systemIcons[ApplicationIconID] = SystemIcons.Application;
            systemIcons[AsteriskIconID] = SystemIcons.Asterisk;
            systemIcons[ErrorIconID] = SystemIcons.Error;
            systemIcons[ExclamationIconID] = SystemIcons.Exclamation;
            systemIcons[HandIconID] = SystemIcons.Hand;
            systemIcons[InformationIconID] = SystemIcons.Information;
            systemIcons[QuestionIconID] = SystemIcons.Question;
            systemIcons[WarningIconID] = SystemIcons.Warning;
            systemIcons[WinLogoIconID] = SystemIcons.WinLogo;
            return systemIcons;
        }
    }
}