
namespace ModernUIDemo.MyControls
{
    using ModernIU.WPF.Base;

    public class TextBoxRtfHTMLControlsVM : PropertyBindingBase
    {
        private string _Message;

        public TextBoxRtfHTMLControlsVM()
        {
            this.Message = "Keine Nachricht";
        }

        public string Message
        {
            get { return _Message; }
            set 
            { 
                this.SetProperty(ref this._Message, value); 
            }
        }


        private string culture;

        public string Culture
        {
            get { return this.culture; }
            set
            {
                this.culture = value;
                switch (this.culture)
                {
                    case "en":
                        this.Message = "The report was sent to <b>your local printer</b>.<br/><br/>Press <u style='color:red'>Enter</u> to continue or <u style='color:red'>Back</u> to print again.";
                        break;
                    case "de":
                        this.Message = "Der Bericht wurde übermittelt an <b>Ihr lokaler Drucker</b>.<br/><br/>Klicke <u style='color:red'>Return</u> für weiter <u style='color: red'>Zurück</u> erneut zu drucken.";
                        break;
                    default:
                        this.Message = "The report was sent to <b>your local printer</b>.<br/><br/>Press <u style='color:red'>Enter</u> to continue or <u style='color:red'>Back</u> to print again.";
                        break;
                }
            }
        }
    }
}