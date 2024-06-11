
namespace ModernUIDemo.MyControls
{
    using System.Windows;

    using ModernUIDemo.Core;

    public class TextBoxRtfControlsVM : BindableBase
    {
        private string _DocumentTextRTF;

        public ICommandAggregator CmdAgg { get; } = new CommandAggregator();

        public TextBoxRtfControlsVM()
        {
            this.DocumentTextRTF = "Keine Nachricht";
            this.CmdAgg.AddOrSetCommand("GetRTFTextCommand", new RelayCommand(p1 => this.GetRTFTextHandler(), p2 => true));
            this.CmdAgg.AddOrSetCommand("SetRTFTextCommand", new RelayCommand(p1 => this.SetRTFTextHandler(), p2 => true));
        }

        public string DocumentTextRTF
        {
            get { return _DocumentTextRTF; }
            set 
            { 
                this.SetProperty(ref this._DocumentTextRTF, value); 
            }
        }

        private void GetRTFTextHandler()
        {
            string rtfText = this.DocumentTextRTF;
            MessageBox.Show(rtfText);
        }

        private void SetRTFTextHandler()
        {
            string rtfText = "{\\rtf1\\ansi\\ansicpg1252\\uc1\\htmautsp\\deff2{\\fonttbl{\\f0\\fcharset0 Times New Roman;}{\\f2\\fcharset0 Segoe UI;}}{\\colortbl\\red0\\green0\\blue0;\\red255\\green255\\blue255;}\\loch\\hich\\dbch\\pard\\plain\\ltrpar\\itap0{\\lang1033\\fs18\\f2\\cf0 \\cf0\\ql{\\f2 {\\lang1031\\ltrch Test, Test , }{\\lang1031\\b\\ltrch Test }{\\lang1031\\ltrch Gerhard Ahrens}\\li0\\ri0\\sa0\\sb0\\fi0\\ql\\par}\r\n}\r\n}";
            //rtfText = "Hallo PTA !!";
            this.DocumentTextRTF = rtfText;
        }
    }
}