//-----------------------------------------------------------------------
// <copyright file="ActionDialogType.cs" company="lifeprojects.de">
//     Class: ActionDialogType
//     Copyright © lifeprojects.de GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>20.07.2019</date>
//
// <summary>
//  Class with ActionDialogType Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    public class ActionDialogType
    {
        public static ActionDialogType HeaderText = new ActionDialogType(true, false);
        public static ActionDialogType HeaderInstructionText = new ActionDialogType(true, true, false);
        public static ActionDialogType AllTextAndCancel = new ActionDialogType(true, true,true);
        public static ActionDialogType AllTextWihoutCancel = new ActionDialogType(true, true, false);

        public bool ShowHeaderText { get; private set; }

        public bool ShowInstructionText { get; private set; }

        public bool ShowActionText { get; private set; }

        public bool ShowCancelButton { get; private set; }

        public ActionDialogType()
        {
            this.ShowHeaderText = true;
            this.ShowInstructionText = false;
            this.ShowActionText = true;
            this.ShowCancelButton = false;
        }

        public ActionDialogType(bool showActionText, bool showCancelButton)
        {
            this.ShowHeaderText = true;
            this.ShowInstructionText = false;
            this.ShowActionText = showActionText;
            this.ShowCancelButton = showCancelButton;
        }

        public ActionDialogType(bool ShowHeaderInstructionText, bool showActionText, bool showCancelButton)
        {
            this.ShowHeaderText = ShowHeaderInstructionText;
            this.ShowInstructionText = ShowHeaderInstructionText;
            this.ShowActionText = showActionText;
            this.ShowCancelButton = showCancelButton;
        }
    }
}
