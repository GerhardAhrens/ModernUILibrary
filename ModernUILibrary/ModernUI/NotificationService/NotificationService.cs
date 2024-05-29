//-----------------------------------------------------------------------
// <copyright file="SPDialogService.cs" company="Lifeprojects.de">
//     Class: SPDialogService
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>10.11.2023 14:30:16</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;

    public class NotificationService : INotificationService
    {
        private static Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();

        public static void RegisterDialog<TView, TViewModel>()
        {
            try
            {
                if (_mappings.ContainsKey(typeof(TViewModel)) == false)
                {
                    _mappings.Add(typeof(TViewModel), typeof(TView));
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public static void RegisterDialog<TView>()
        {
            try
            {
                if (_mappings.ContainsKey(typeof(TView)) == false)
                {
                    _mappings.Add(typeof(TView), typeof(TView));
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }
        public void ShowDialog(string name, Action<bool?, object> callBack)
        {
            string rootNamespace = GetCurrentNamespace();

            try
            {
                Tuple<string, string, double> text = new Tuple<string, string, double>(string.Empty, string.Empty, 14);
                Type type = Type.GetType($"{rootNamespace}.{name}");
                this.ShowDialogInternal(type, text, callBack, null);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void ShowDialog(string name, string addText, Action<bool?, object> callBack)
        {
            string rootNamespace = GetCurrentNamespace();

            try
            {
                Tuple<string, string, double> text = new Tuple<string, string, double>(string.Empty, addText, 14);
                Type type = Type.GetType($"{rootNamespace}.{name}");
                this.ShowDialogInternal(type, text, callBack, null);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void ShowDialog(string name, Tuple<string, string, double> addText, Action<bool?, object> callBack)
        {
            string rootNamespace = GetCurrentNamespace();

            try
            {
                Type type = Type.GetType($"{rootNamespace}.{name}");
                this.ShowDialogInternal(type, addText, callBack, null);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void ShowDialog<TViewModel>(Action<bool?,object> callBack)
        {
            try
            {
                Type type = _mappings[typeof(TViewModel)];
                if (type != null)
                {
                    Tuple<string, string, double> text = new Tuple<string, string, double>(string.Empty, string.Empty, 14);
                    this.ShowDialogInternal(type, text, callBack, typeof(TViewModel));
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void ShowDialog<TViewModel>(string addText,Action<bool?, object> callBack)
        {
            try
            {
                Type type = _mappings[typeof(TViewModel)];
                if (type != null)
                {
                    Tuple<string, string, double> text = new Tuple<string, string, double>(string.Empty, addText, 14);
                    this.ShowDialogInternal(type, text, callBack, typeof(TViewModel));
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void ShowDialog<TViewModel>(Tuple<string, string, double> addText, Action<bool?, object> callBack)
        {
            try
            {
                Type type = _mappings[typeof(TViewModel)];
                if (type != null)
                {
                    this.ShowDialogInternal(type, addText, callBack, typeof(TViewModel));
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }
        private void ShowDialogInternal(Type type, Tuple<string, string, double> addText, Action<bool?,object> callBack, Type vmType)
        {
            NotificationWindow dlg = new NotificationWindow();
            dlg.Topmost = false;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dlg.WindowState = WindowState.Normal;
            dlg.Owner = Application.Current.MainWindow;

            EventHandler closeEventHandler = null;
            closeEventHandler = (s, e) =>
            {
                callBack(dlg.DialogResult,dlg.Tag);
                dlg.Closed -= closeEventHandler;
            };

            dlg.Closed += closeEventHandler;

            if (type != null)
            {
                UserControl content = Activator.CreateInstance(type) as UserControl;

                if (content != null)
                { 
                    content.Tag = addText;

                    if (vmType != null)
                    {
                        var vm = Activator.CreateInstance(vmType);
                        (content as FrameworkElement).DataContext = vm;
                    }

                    dlg.Content = content;
                }

                dlg.ShowDialog();
            }
        }

        private bool IsHtmlBody(string text)
        {
            bool result = false;

            if (text.ToLower().StartsWith("<html>") == true && text.ToLower().Contains("<body>") == true && text.ToLower().StartsWith("</html>") == true && text.ToLower().Contains("</body>") == true)
            {
                result = true;
            }

            return result;
        }

        private string CreateHtmlContent(string htmlText, bool setScroll = false)
        {
            string scrollOption = setScroll == true ? "yes" : "no";

            StringBuilder htmlContent = new StringBuilder();
            htmlContent.Append($"<html><body scroll=\"{scrollOption}\">");
            htmlContent.Append(htmlText);
            htmlContent.Append("</body></html>");

            return htmlContent.ToString();
        }

        private string GetCurrentNamespace()
        {
            string n1 = GetType().Namespace;

            var myType = typeof(NotificationWindow);
            var n2 = myType.Namespace;

            return System.Reflection.Assembly.GetExecutingAssembly().EntryPoint.DeclaringType.Namespace;
        }
    }
}
