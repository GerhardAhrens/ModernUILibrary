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
                (string InfoText, string CustomText, int MaxLength, double FontSize) text = (string.Empty, string.Empty,20, 14);
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
                (string InfoText, string CustomText, int MaxLength, double FontSize) text = (string.Empty, addText,20, 14);
                Type type = Type.GetType($"{rootNamespace}.{name}");
                this.ShowDialogInternal(type, text, callBack, null);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void ShowDialog(string name, int countDown, (string InfoText, string CustomText, int MaxLength, double FontSize) addText, Action<bool?, object> callBack)
        {
            string rootNamespace = GetCurrentNamespace();

            try
            {
                Type type = Type.GetType($"{rootNamespace}.{name}");
                this.ShowDialogInternal(type, countDown, addText, callBack, null);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void ShowDialog(string name, (string InfoText, string CustomText, int MaxLength, double FontSize) addText, Action<bool?, object> callBack)
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

        public void ShowDialog(string name,(string InfoText, string CustomText, double FontSize) option, Action<bool?, object> callBack)
        {
            string rootNamespace = GetCurrentNamespace();

            try
            {
                Type type = Type.GetType($"{rootNamespace}.{name}");
                this.ShowDialogInternal(type, -1, option, callBack, null);
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
                    (string InfoText, string CustomText, int MaxLength, double FontSize) option = (string.Empty, string.Empty,20, 14);
                    this.ShowDialogInternal(type, option, callBack, typeof(TViewModel));
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
                    (string InfoText, string CustomText, int MaxLength, double FontSize) option = (string.Empty, addText,20, 14);
                    this.ShowDialogInternal(type, option, callBack, typeof(TViewModel));
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void ShowDialog<TViewModel>((string InfoText, string CustomText, int MaxLength, double FontSize) addText, Action<bool?, object> callBack)
        {
            try
            {
                if (_mappings.ContainsKey(typeof(TViewModel)))
                {

                    Type type = _mappings[typeof(TViewModel)];
                    if (type != null)
                    {
                        this.ShowDialogInternal(type, addText, callBack, typeof(TViewModel));
                    }
                }
                else
                {
                    throw new ArgumentException($"Object '{typeof(TViewModel).Name}' not found!");
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public void ShowDialog<TViewModel>((string InfoText, string CustomText, double FontSize) addText, Action<bool?, object> callBack)
        {
            ShowDialog<TViewModel>(-1, addText, callBack);
        }

        public void ShowDialog<TViewModel>(int countDown, (string InfoText, string CustomText, double FontSize) addText, Action<bool?, object> callBack)
        {
            try
            {
                if (_mappings.ContainsKey(typeof(TViewModel)))
                {

                    Type type = _mappings[typeof(TViewModel)];
                    if (type != null)
                    {
                        this.ShowDialogInternal(type, countDown, addText, callBack, typeof(TViewModel));
                    }
                }
                else
                {
                    throw new ArgumentException($"Object '{typeof(TViewModel).Name}' not found!");
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        private void ShowDialogInternal(Type type, (string InfoText, string CustomText, int MaxLength, double FontSize) addText, Action<bool?,object> callBack, Type vmType)
        {
            this.ShowDialogInternal(type, -1, addText, callBack, vmType);
        }

        private void ShowDialogInternal(Type type, int countDown, (string InfoText, string CustomText, int MaxLength, double FontSize) addText, Action<bool?, object> callBack, Type vmType)
        {
            NotificationWindow dlg = new NotificationWindow();
            dlg.Topmost = false;
            dlg.ShowInTaskbar = false;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dlg.WindowState = WindowState.Normal;
            dlg.Owner = Application.Current.MainWindow;

            EventHandler closeEventHandler = null;
            closeEventHandler = (s, e) =>
            {
                callBack(dlg.DialogResult, dlg.Tag);
                dlg.Closed -= closeEventHandler;
            };

            dlg.Closed += closeEventHandler;

            if (type != null)
            {
                INotificationServiceMessage content = Activator.CreateInstance(type) as INotificationServiceMessage;

                if (content != null)
                {
                    content.CountDown = countDown;
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


        private void ShowDialogInternal(Type type, int countDown, (string InfoText, string CustomText, double FontSize) addText, Action<bool?, object> callBack, Type vmType)
        {
            NotificationWindow dlg = new NotificationWindow();
            dlg.Topmost = false;
            dlg.ShowInTaskbar = false;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dlg.WindowState = WindowState.Normal;
            dlg.Owner = Application.Current.MainWindow;

            EventHandler closeEventHandler = null;
            closeEventHandler =  closeEventHandler = (s, e) =>
            {
                callBack(dlg.DialogResult, dlg.Tag);
                dlg.Closed -= closeEventHandler;
            };

            dlg.Closed += closeEventHandler;

            if (type != null)
            {
                INotificationServiceMessage content = Activator.CreateInstance(type) as INotificationServiceMessage;

                if (content != null)
                {
                    content.CountDown = countDown;
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
