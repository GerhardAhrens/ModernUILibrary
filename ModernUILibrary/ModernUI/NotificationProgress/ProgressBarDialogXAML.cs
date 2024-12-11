//-----------------------------------------------------------------------
// <copyright file="ProgressBarDialogXAML.cs" company="lifeprojects.de">
//     Class: ProgressBarDialogXAML
//     Copyright © lifeprojects.de GmbH 2019
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>08.02.2019</date>
//
// <summary>
//  Class with DependencyProperty from XAML Windows on ProgressBarDialog
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows;

    using ModernIU.Base.Win32API;

    public class ProgressBarDialogXAML
    {
        private static readonly DependencyPropertyKey IsHiddenCloseButtonKey =
            DependencyProperty.RegisterAttachedReadOnly("IsCloseButtonHidden", typeof(bool), typeof(ProgressBarDialogXAML), new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HideCloseButtonProperty =
             DependencyProperty.RegisterAttached("HideCloseButton", typeof(bool), typeof(ProgressBarDialogXAML), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnHideCloseButtonPropertyChanged)));

        public static readonly DependencyProperty IsCloseButtonHiddenProperty = IsHiddenCloseButtonKey.DependencyProperty;

        public static bool GetHideCloseButton(FrameworkElement element)
        {
            return (bool)element.GetValue(HideCloseButtonProperty);
        }

        public static void SetHideCloseButton(FrameworkElement element, bool hideCloseButton)
        {
            element.SetValue(HideCloseButtonProperty, hideCloseButton);
        }

        public static bool GetIsCloseButtonHidden(FrameworkElement element)
        {
            return (bool)element.GetValue(IsCloseButtonHiddenProperty);
        }

        private static void OnHideCloseButtonPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Window window = d as Window;

            if (window != null)
            {
                var hideCloseButton = (bool)e.NewValue;

                if (hideCloseButton && !GetIsCloseButtonHidden(window))
                {
                    if (!window.IsLoaded)
                    {
                        window.Loaded += OnWindowLoaded;
                    }
                    else
                    {
                        Win32API.Instance.HideCloseButton(window);
                    }

                    SetIsCloseButtonHidden(window, true);
                }
                else if (!hideCloseButton && GetIsCloseButtonHidden(window))
                {
                    if (!window.IsLoaded)
                    {
                        window.Loaded -= OnWindowLoaded;
                    }
                    else
                    {
                        Win32API.Instance.ShowCloseButton(window);
                    }

                    SetIsCloseButtonHidden(window, false);
                }
            }
        }

        private static readonly RoutedEventHandler OnWindowLoaded = (s, e) =>
           {
               if (s is Window)
               {
                   Window window = s as Window;
                   if (window != null)
                   {
                       Win32API.Instance.HideCloseButton(window);
                       window.Loaded -= OnWindowLoaded;
                   }
               }
           };

        private static void SetIsCloseButtonHidden(FrameworkElement element, bool isCloseButtonHidden)
        {
            element.SetValue(IsHiddenCloseButtonKey, isCloseButtonHidden);
        }
    }
}