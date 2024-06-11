
namespace ModernUIDemo.MyControls
{
    using ModernUIDemo.Core;

    public class TextBoxRtfLControlsVM : BindableBase
    {
        private string _DocumentTextRTF;

        public TextBoxRtfLControlsVM()
        {
            this.DocumentTextRTF = "Keine Nachricht";
        }

        public string DocumentTextRTF
        {
            get { return _DocumentTextRTF; }
            set 
            { 
                this.SetProperty(ref this._DocumentTextRTF, value); 
            }
        }
    }
}