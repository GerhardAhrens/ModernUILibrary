//-----------------------------------------------------------------------
// <copyright file="NotifiactionBoxOption.cs" company="Lifeprojects.de">
//     Class: NotifiactionBoxOption
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>24.08.2018</date>
//
// <summary>Class with Parameter Definitions for MessageBoxEx</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    public class NotificationBoxOption
    {
        public bool Topmost { get; set; } = false;

        public Window Onwer { get; set; } = null;

        public string Caption { get; set; }

        public int DialogWidth { get; set; } = 500;

        public string InstructionHeading { get; set; }

        public int InstructionHeadingFontSize { get; set; } = 18;

        public string InstructionText { get; set; }

        public IEnumerable<string> InstructionSource { get; set; }

        public MessageBoxButton MessageBoxButton { get; set; }

        public NotificationIcon InstructionIcon { get; set; }

        public ImageSource Icon { get; set; }

        public NotificationResult NotificationResult { get; set; }

        public int AutoCloseDialogTime { get; set; } = 0;

        public string ButtonLeft { get; set; } = null;

        public string ButtonRight { get; set; } = null;

        public string ButtonMiddle { get; set; } = null;

        public string OptionBoxText { get; set; }

        public bool? OptionBoxValue { get; set; }

        public string Language { get; set; }
    }
}
