using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace ModernTemplate;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private const string DEFAULTLANGUAGE = "de-DE";
    private const string SHORTNAME = "ModernTemplate";
    private static readonly string MessageBoxTitle = "ModernTemplate Application";
    private static readonly string UnexpectedError = "An unexpected error occured.";
    private string exePath = string.Empty;
    private string exeName = string.Empty;

    public App()
    {
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
#if DEBUG
        PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
        PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Critical | SourceLevels.Error | SourceLevels.Warning;
        //PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.All;
        PresentationTraceSources.RoutedEventSource.Listeners.Add(new ConsoleTraceListener());
        PresentationTraceSources.RoutedEventSource.Switch.Level = SourceLevels.All;
#endif
    }
}

