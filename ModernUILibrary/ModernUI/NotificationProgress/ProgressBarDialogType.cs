//-----------------------------------------------------------------------
// <copyright file="ProgressBarDialogType.cs" company="lifeprojects.de">
//     Class: ProgressBarDialogType
//     Copyright © lifeprojects.de GmbH 2019
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>08.02.2019</date>
//
// <summary>
//  Class with ProgressBarDialogType Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    public class ProgressBarDialogType
    {
        public static ProgressBarDialogType WithLabelOnly = new ProgressBarDialogType(false, false, true);
        public static ProgressBarDialogType WithSubLabel = new ProgressBarDialogType(true, false, true);
        public static ProgressBarDialogType WithSubLabelAndCancel = new ProgressBarDialogType(true, true, true);

        public bool ShowSubLabel { get; set; }
        public bool ShowCancelButton { get; set; }
        public bool ShowProgressBarIndeterminate { get; set; }

        public ProgressBarDialogType()
        {
            this.ShowSubLabel = false;
            this.ShowCancelButton = false;
            this.ShowProgressBarIndeterminate = true;
        }

        public ProgressBarDialogType(bool showSubLabel, bool showCancelButton, bool showProgressBarIndeterminate)
        {
            this.ShowSubLabel = showSubLabel;
            this.ShowCancelButton = showCancelButton;
            this.ShowProgressBarIndeterminate = showProgressBarIndeterminate;
        }
    }
}
