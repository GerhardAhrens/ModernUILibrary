//-----------------------------------------------------------------------
// <copyright file="KeyPress.cs" company="Lifeprojects.de">
//     Class: KeyPress
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.06.2020</date>
//
// <summary>
//      Die Klasse gibt die gedrückte Teste als String zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Input;

    public class KeyPress : Window
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        public KeyEventArgs e;
        public static string Text { get; set; }

        public KeyPress(KeyEventArgs e)
        {
            this.e = e;
            keybd_event(0x08, 0, 1, 0);
            Register();
        }


        public string Register()
        {
            switch (e.Key)
            {
                case Key.D0:
                    Text = "D0";
                    break;
                case Key.D1:
                    Text = "D1";
                    break;
                case Key.D2:
                    Text = "D2";
                    break;
                case Key.D3:
                    Text = "D3";
                    break;
                case Key.D4:
                    Text = "D0";
                    break;
                case Key.D5:
                    Text = "D5";
                    break;
                case Key.D6:
                    Text = "D6";
                    break;
                case Key.D7:
                    Text = "D7";
                    break;
                case Key.D8:
                    Text = "D8";
                    break;
                case Key.D9:
                    Text = "D9";
                    break;
                case Key.F1:
                    Text = "F1";
                    break;
                case Key.F2:
                    Text = "F2";
                    break;
                case Key.F3:
                    Text = "F3";
                    break;
                case Key.F4:
                    Text = "F4";
                    break;
                case Key.F5:
                    Text = "F5";
                    break;
                case Key.F6:
                    Text = "F6";
                    break;
                case Key.F7:
                    Text = "F7";
                    break;
                case Key.F8:
                    Text = "F8";
                    break;
                case Key.F9:
                    Text = "F9";
                    break;
                case Key.F10:
                    Text = "F10";
                    break;
                case Key.F11:
                    Text = "F11";
                    break;
                case Key.F12:
                    Text = "F12";
                    break;
                case Key.F13:
                    Text = "F13";
                    break;
                case Key.F14:
                    Text = "F14";
                    break;
                case Key.F15:
                    Text = "F15";
                    break;
                case Key.F16:
                    Text = "F16";
                    break;
                case Key.F17:
                    Text = "F17";
                    break;
                case Key.F18:
                    Text = "F18";
                    break;
                case Key.F19:
                    Text = "F19";
                    break;
                case Key.F20:
                    Text = "F20";
                    break;
                case Key.F21:
                    Text = "F21";
                    break;
                case Key.F22:
                    Text = "F22";
                    break;
                case Key.F23:
                    Text = "F23";
                    break;
                case Key.F24:
                    Text = "F24";
                    break;
                case Key.NumPad0:
                    Text = "NumPad0";
                    break;
                case Key.NumPad1:
                    Text = "NumPad1";
                    break;
                case Key.NumPad2:
                    Text = "NumPad2";
                    break;
                case Key.NumPad3:
                    Text = "NumPad3";
                    break;
                case Key.NumPad4:
                    Text = "NumPad4";
                    break;
                case Key.NumPad5:
                    Text = "NumPad5";
                    break;
                case Key.NumPad6:
                    Text = "NumPad6";
                    break;
                case Key.NumPad7:
                    Text = "NumPad7";
                    break;
                case Key.NumPad8:
                    Text = "NumPad8";
                    break;
                case Key.NumPad9:
                    Text = "NumPad9";
                    break;
                case Key.A:
                    Text = "A";
                    break;
                case Key.B:
                    Text = "B";
                    break;
                case Key.C:
                    Text = "C";
                    break;
                case Key.D:
                    Text = "D";
                    break;
                case Key.E:
                    Text = "E";
                    break;
                case Key.F:
                    Text = "F";
                    break;
                case Key.G:
                    Text = "G";
                    break;
                case Key.H:
                    Text = "H";
                    break;
                case Key.I:
                    Text = "I";
                    break;
                case Key.J:
                    Text = "J";
                    break;
                case Key.K:
                    Text = "K";
                    break;
                case Key.L:
                    Text = "L";
                    break;
                case Key.M:
                    Text = "M";
                    break;
                case Key.N:
                    Text = "N";
                    break;
                case Key.O:
                    Text = "O";
                    break;
                case Key.P:
                    Text = "P";
                    break;
                case Key.Q:
                    Text = "Q";
                    break;
                case Key.R:
                    Text = "R";
                    break;
                case Key.S:
                    Text = "S";
                    break;
                case Key.T:
                    Text = "T";
                    break;
                case Key.U:
                    Text = "U";
                    break;
                case Key.V:
                    Text = "V";
                    break;
                case Key.W:
                    Text = "W";
                    break;
                case Key.X:
                    Text = "X";
                    break;
                case Key.Y:
                    Text = "Y";
                    break;
                case Key.Z:
                    Text = "Z";
                    break;
                case Key.AbntC1:
                    Text = "AbntC1";
                    break;
                case Key.AbntC2:
                    Text = "AbntC2";
                    break;
                case Key.Add:
                    Text = "Add";
                    break;
                case Key.Apps:
                    Text = "Apps";
                    break;
                case Key.Attn:
                    Text = "Attn";
                    break;
                case Key.Back:
                    Text = "Back";
                    break;
                case Key.BrowserBack:
                    Text = "BrowserBack";
                    break;
                case Key.BrowserFavorites:
                    Text = "BrowserFavorites";
                    break;
                case Key.BrowserForward:
                    Text = "BrowserForward";
                    break;
                case Key.BrowserHome:
                    Text = "BrowserHome";
                    break;
                case Key.BrowserRefresh:
                    Text = "BrowserRefresh";
                    break;
                case Key.BrowserSearch:
                    Text = "BrowserSearch";
                    break;
                case Key.BrowserStop:
                    Text = "BrowserStop";
                    break;
                case Key.Cancel:
                    Text = "Cancel";
                    break;
                case Key.CapsLock:
                    Text = "CapsLock";
                    break;
                case Key.Clear:
                    Text = "Clear";
                    break;
                case Key.CrSel:
                    Text = "CrSel";
                    break;
                case Key.Decimal:
                    Text = "Decimal";
                    break;
                case Key.Delete:
                    Text = "Delete";
                    break;
                case Key.Divide:
                    Text = "Divide";
                    break;
                case Key.Down:
                    Text = "Down";
                    break;
                case Key.End:
                    Text = "End";
                    break;
                case Key.Enter:
                    Text = "Enter";
                    break;
                case Key.EraseEof:
                    Text = "EraseEof";
                    break;
                case Key.Escape:
                    Text = "Escape";
                    break;
                case Key.Execute:
                    Text = "Execute";
                    break;
                case Key.ExSel:
                    Text = "ExSel";
                    break;
                case Key.FinalMode:
                    Text = "FinalMode";
                    break;
                case Key.HangulMode:
                    Text = "HangulMode";
                    break;
                case Key.HanjaMode:
                    Text = "HanjaMode";
                    break;
                case Key.Help:
                    Text = "Help";
                    break;
                case Key.Home:
                    Text = "Home";
                    break;
                case Key.ImeAccept:
                    Text = "ImeAccept";
                    break;
                case Key.ImeConvert:
                    Text = "ImeConvert";
                    break;
                case Key.ImeModeChange:
                    Text = "ImeModeChange";
                    break;
                case Key.ImeNonConvert:
                    Text = "ImeNonConvert";
                    break;
                case Key.ImeProcessed:
                    Text = "ImeProcessed";
                    break;
                case Key.Insert:
                    Text = "Insert";
                    break;
                case Key.JunjaMode:
                    Text = "JunjaMode";
                    break;
                case Key.LaunchApplication1:
                    Text = "LaunchApplication1";
                    break;
                case Key.LaunchApplication2:
                    Text = "LaunchApplication2";
                    break;
                case Key.LaunchMail:
                    Text = "LaunchMail";
                    break;
                case Key.Left:
                    Text = "Left";
                    break;
                case Key.LeftAlt:
                    Text = "LeftAlt";
                    break;
                case Key.LeftCtrl:
                    Text = "LeftCtrl";
                    break;
                case Key.LeftShift:
                    Text = "LeftShift";
                    break;
                case Key.LineFeed:
                    Text = "LineFeed";
                    break;
                case Key.LWin:
                    Text = "LWin";
                    break;
                case Key.MediaNextTrack:
                    Text = "MediaNextTrack";
                    break;
                case Key.MediaPlayPause:
                    Text = "MediaPlayPause";
                    break;
                case Key.MediaPreviousTrack:
                    Text = "MediaPreviousTrack";
                    break;
                case Key.MediaStop:
                    Text = "MediaStop";
                    break;
                case Key.Multiply:
                    Text = "Multiply";
                    break;
                case Key.Next:
                    Text = "Next";
                    break;
                case Key.Oem1:
                    Text = "Oem1";
                    break;
                case Key.Oem102:
                    Text = "Oem102";
                    break;
                case Key.Oem2:
                    Text = "Oem2";
                    break;
                case Key.Oem3:
                    Text = "Oem3";
                    break;
                case Key.Oem4:
                    Text = "Oem4";
                    break;
                case Key.Oem5:
                    Text = "Oem5";
                    break;
                case Key.Oem6:
                    Text = "Oem6";
                    break;
                case Key.Oem7:
                    Text = "Oem7";
                    break;
                case Key.Oem8:
                    Text = "Oem8";
                    break;
                case Key.OemAttn:
                    Text = "OemAttn";
                    break;
                case Key.OemAuto:
                    Text = "OemAuto";
                    break;
                case Key.OemBackTab:
                    Text = "OemBackTab";
                    break;
                case Key.OemClear:
                    Text = "OemClear";
                    break;
                case Key.OemComma:
                    Text = "OemComma";
                    break;
                case Key.OemCopy:
                    Text = "OemCopy";
                    break;
                case Key.OemEnlw:
                    Text = "OemEnlw";
                    break;
                case Key.OemFinish:
                    Text = "OemFinish";
                    break;
                case Key.OemMinus:
                    Text = "OemMinus";
                    break;
                case Key.OemPeriod:
                    Text = "OemPeriod";
                    break;
                case Key.OemPlus:
                    Text = "OemPlus";
                    break;
                case Key.PageUp:
                    Text = "PageUp";
                    break;
                case Key.Pause:
                    Text = "Pause";
                    break;
                case Key.Play:
                    Text = "Play";
                    break;
                case Key.Print:
                    Text = "Print";
                    break;
                case Key.PrintScreen:
                    Text = "PrintScreen";
                    break;
                case Key.Right:
                    Text = "Right";
                    break;
                case Key.RightAlt:
                    Text = "RightAlt";
                    break;
                case Key.RightCtrl:
                    Text = "RightCtrl";
                    break;
                case Key.RightShift:
                    Text = "RightShift";
                    break;
                case Key.RWin:
                    Text = "RWin";
                    break;
                case Key.Scroll:
                    Text = "Scroll";
                    break;
                case Key.Select:
                    Text = "Select";
                    break;
                case Key.SelectMedia:
                    Text = "SelectMedia";
                    break;
                case Key.Separator:
                    Text = "Separator";
                    break;
                case Key.Sleep:
                    Text = "Sleep";
                    break;
                case Key.Space:
                    Text = "Space";
                    break;
                case Key.Subtract:
                    Text = "Subtract";
                    break;
                case Key.System:
                    Text = "System";
                    break;
                case Key.Tab:
                    Text = "Tab";
                    break;
                case Key.Up:
                    Text = "Up";
                    break;
                case Key.VolumeDown:
                    Text = "VolumeDown";
                    break;
                case Key.VolumeMute:
                    Text = "VolumeMute";
                    break;
                case Key.VolumeUp:
                    Text = "VolumeUp";
                    break;
                case Key.Zoom:
                    Text = "Zoom";
                    break;
                default:
                    Text = "Error: Key not found";
                    break;
            }
            return Text;
        }
    }
}