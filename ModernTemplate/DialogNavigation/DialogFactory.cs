namespace ModernTemplate.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Extension;

    using ModernTemplate.Views.ContentControls;

    using ModernUI.MVVM.Base;

    /// <summary>
    /// Die Factory gibt auf Basis eines CommandButton das dazugehörige UserControl-Objekt zurück 
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class DialogFactory : IDialogFactory
    {
        private static Dictionary<Enum, Type> Views = null;

        static DialogFactory()
        {
            RegisterControls();
        }

        /// <summary>
        /// Rückgabe eines userControl auf Basis des übergebenen CommandButton
        /// </summary>
        /// <param name="mainButton">Enum mit CommandButton</param>
        /// <returns>Gewähltes UserControl</returns>
        public static FactoryResult Get(CommandButtons mainButton)
        {
            FactoryResult resultContent = null;
            using (LoadingWaitCursor wc = new LoadingWaitCursor())
            {
                using (LoadingViewTime lvt = new LoadingViewTime())
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (Views.ContainsKey(mainButton) == true)
                    {
                        UserControlBase resultInstance = CreateInstanceContent(mainButton, null);
                        resultContent = new FactoryResult(resultInstance);
                        resultContent.WorkContent.Focusable = true;
                        resultContent.WorkContent.Focus();
                        resultContent.UsedTime = lvt.Result();
                        resultContent.ButtonDescription = mainButton.ToDescription();
                    }
                }
            }

            return resultContent;
        }

        /// <summary>
        /// Rückgabe eines userControl auf Basis des übergebenen CommandButton
        /// </summary>
        /// <param name="mainButton">Enum mit CommandButton</param>
        /// <param name="changeViewArgs">Argument das dem UserControl mit übergeben werden soll</param>
        /// <returns>Gewähltes UserControl</returns>
        public static FactoryResult Get(CommandButtons mainButton, IChangeViewEventArgs changeViewArgs)
        {
            FactoryResult resultContent = null;
            using (LoadingWaitCursor wc = new LoadingWaitCursor())
            {
                using (LoadingViewTime lvt = new LoadingViewTime())
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    if (Views.ContainsKey(mainButton) == true)
                    {
                        UserControlBase resultInstance = CreateInstanceContent(mainButton, changeViewArgs);
                        resultContent = new FactoryResult(resultInstance);
                        resultContent.WorkContent.Focusable = true;
                        resultContent.WorkContent.Focus();
                        resultContent.UsedTime = lvt.Result();
                        resultContent.ButtonDescription = mainButton.ToDescription();
                    }
                }
            }

            return resultContent;
        }


        /// <summary>
        /// Registrieren der Content Controls
        /// </summary>
        private static void RegisterControls()
        {
            try
            {
                if (Views == null)
                {
                    Views = new Dictionary<Enum, Type>();
                    Views.Add(CommandButtons.Home, typeof(HomeRibbonUC));
                    Views.Add(CommandButtons.AppSettings, typeof(AppSettingsUC));
                    Views.Add(CommandButtons.AppAbout, typeof(AppAboutUC));
                    Views.Add(CommandButtons.Custom, typeof(CustomUC));
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        /// <summary>
        /// Erstellen einer Instanz eines UserControl des übergebenen CommandButton
        /// </summary>
        /// <param name="commandButton">>Enum mit CommandButton</param>
        /// <param name="changeViewArgs">Argument das dem UserControl mit übergeben werden soll</param>
        /// <returns>Gewähltes UserControl</returns>
        /// <exception cref="NotSupportedException"></exception>
        private static UserControlBase CreateInstanceContent(Enum commandButton, IChangeViewEventArgs changeViewArgs)
        {
            Type viewObject = Views[commandButton];

            if (viewObject != null && viewObject.IsAssignableTo(typeof(UserControlBase)) == true)
            {
                if (viewObject.GetConstructors().Count() >= 1)
                {
                    if (viewObject.GetConstructors()[0].GetParameters().Count() == 1)
                    {
                        ParameterInfo param = viewObject.GetConstructors()[0].GetParameters()[0];
                        Type typParam = Type.GetType($"{param.ParameterType.Namespace}.{param.ParameterType.Name}");
                        if (param != null && typParam.GetInterfaces().Contains(typeof(IChangeViewEventArgs)) == true)
                        {
                            return (UserControlBase)Activator.CreateInstance(viewObject, changeViewArgs);
                        }
                        else
                        {
                            throw new NotSupportedException($"Es wurde kein Konstruktor angegeben. Es muß ein Kontruktor der 'IChangeViewEventArgs' implementiert vorhanden sein. Control: {commandButton.ToDescription()}; Object: {viewObject.GetFriendlyTypeName()}");
                        }
                    }
                    else
                    {
                        return (UserControlBase)Activator.CreateInstance(viewObject);
                    }
                }
                else
                {
                    return (UserControlBase)Activator.CreateInstance(viewObject,1,null);
                }
            }
            else
            {
                throw new NotSupportedException($"Das UserControl implementiert kein 'UserControlBase'. Control: {commandButton.ToDescription()}; Object: {viewObject.GetFriendlyTypeName()}");
            }
        }
    }
}
