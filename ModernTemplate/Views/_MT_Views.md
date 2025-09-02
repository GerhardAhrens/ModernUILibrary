# Verzeichnis Views

Dialoge bzw. Views sind ein wichtiger Bestandteil vieler Anwendungen. Das Template unterstützt die Erstellung von Windows als Dialoge aber auch UserControl die in einem ContentControl geladen werden.
Da das Template für Single Page Anwendungen ausgelegt ist, kommen als Dialge in der Regel nur UserControls in Frage.

Startfolge einer mit dem Template erstelle Anwendung
- App
  Initalisierung der Anwendung, Konfiguration, Logging
- [DialogNavigation](https://github.com/GerhardAhrens/ModernUILibrary/blob/master/ModernTemplate/DialogNavigation/_MT_DialogNavigation.md)
  Verwaltung aller UserControls die als Dialoge/View in der Anwendung Verwendung finden.
- MainWindows
  Zentrale Stelle um die Steuerung der UserControls mittels einem Mediator steuern zu können.
- Home
  Der *Home*-Dialog (HomeRibbonUC) beinhaltet das MainMenü. Für das Template ist das Menü als ein Ribbon-Menü ausgelegt. Der Grund ist, warum das Menü auf einem eigenen UserControl liegt, ist die Möglichkeit das auch gegen ein anderes UserControl mit dann einem anderen Menü zu tauschen. mit einer dazwischen geschalteten Benutzerverwaltung wäre es hier möglich verschiedene Menüs pro Benutzer darstellen zu können.

Im *MainWindows.xaml.cs* wird der EventAggregator für die UserControl Steuerung initalisiert. jedes Window oder UserControl leitet von einer Basisklasse *WindowBase* oder *UserControlBase* ab. Darin sind grundlegende Funktionen zur Steuerung und dem Binding von UI Elementen.

```csharp
public partial class MainWindow : WindowBase, IDialogClosing
{
}
```

```csharp
base.EventAgg.Subscribe<ChangeViewEventArgs>(this.ChangeControl);
```

Aufruf des Home-UserControl
```csharp
ChangeViewEventArgs arg = new ChangeViewEventArgs();
arg.MenuButton = CommandButtons.Home;
this.ChangeControl(arg);
```

Die Methode läd nun über die Zuordnung des *CommandButtons* das entsprechende UserControl in das ContentControl.
```csharp
private void ChangeControl(ChangeViewEventArgs e)
{
    if (e.MenuButton == CommandButtons.CloseApp)
    {
        this.Close();
        return;
    }

    FactoryResult view = DialogFactory.Get(e.MenuButton, e);
    if (view is FactoryResult menuWorkArea)
    {
        using (ObjectRuntime objectRuntime = new ObjectRuntime())
        {
            StatusbarMain.Statusbar.SetNotification();
            this.CurrentCommandName = e.MenuButton;
            string keyUC = e.MenuButton.GetAttributeOfType<EnumKeyAttribute>().EnumKey;
            string titelUC = e.MenuButton.GetAttributeOfType<EnumKeyAttribute>().Description;
            if (menuWorkArea.WorkContent != null)
            {
                this.WorkContent = menuWorkArea.WorkContent;
                this.WorkContent.VerticalContentAlignment = VerticalAlignment.Stretch;
                this.WorkContent.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                this.WorkContent.Focusable = true;
            }

            WindowTitleMain.WindowTitleLine.SetWindowTitle(titelUC);

            App.Logger.Info($"Load UC '{titelUC}'; [{(int)e.MenuButton}]", true);
            StatusbarMain.Statusbar.SetNotification($"Bereit: {objectRuntime.ResultMilliseconds()}ms");
        }
    }
    else
    {
        App.Logger.Warning($"Der Dialog '{e.MenuButton}|{e.MenuButton.ToString()}' kann nicht gefunden werden.");
        throw new NotSupportedException($"Der Dialog '{e.MenuButton}|{e.MenuButton.ToString()}' kann nicht gefunden werden.");
    }
}
```

Nach dem das *Home*-Control aufgerufen wurde, steht das Ribbon-Menü zur Verfügung. Alle weiteren Funktionen werden über das *Ribbon-Menü* => *Home*-Control => über den Mediator und der Methode *ChangeControl* (im MainWindow) gesteuert.
Aufrufen eines Dialoges / Funktion über das Ribbon-Menü

Im *Home*-Control sind Commands für den CommandAggregator registriert.
```csharp
public override void InitCommands()
{
    this.CmdAgg.AddOrSetCommand(CommandButtons.CloseApp, new RelayCommand(this.CloseAppHandler));
    this.CmdAgg.AddOrSetCommand(CommandButtons.CustomA, new RelayCommand(this.CustomAHandler));
    this.CmdAgg.AddOrSetCommand(CommandButtons.CustomB, new RelayCommand(this.CustomBHandler));
    this.CmdAgg.AddOrSetCommand(CommandButtons.CatKatalogA, new RelayCommand(this.CatKatalogAHandler));
    this.CmdAgg.AddOrSetCommand(CommandButtons.CatKatalogB, new RelayCommand(this.CatKatalogBHandler));
    this.CmdAgg.AddOrSetCommand(CommandButtons.CatKatalogC, new RelayCommand(this.CatKatalogCHandler));
}
```

Im Xaml ist ein *RibbonButton* an den CommandAggregator gebunden. Hier soll der Klick für *CustomACommand* gestartet werden.
```xml
<RibbonButton
    x:Name="CustomDialogAButton"
    Command="{Binding Path=CmdAgg[CustomACommand]}"
    Label="Custom Dialog A"
    LargeImageSource="{StaticResource ResourceKey=IconCustomA}">
    <RibbonButton.ToolTip>
        <ModernUI:MToolTip Content="Beispieldialog aufrufen" PlacementEx="TopLeft" />
    </RibbonButton.ToolTip>
</RibbonButton>
```

Die dazugehörige *CustomAHandler*-Methode wird aufgerufen
```csharp
private void CustomAHandler(object p1)
{
    base.EventAgg.Publish<ChangeViewEventArgs>(new ChangeViewEventArgs
    {
        Sender = this.GetType().Name,
        MenuButton = CommandButtons.CustomA,
    });
}
```

Über die Zuordnung des *CommandButtons* wird das zugeordnete UserControl aus der *DialogFactory.cs* über die Methode *ChangeControl(ChangeViewEventArgs e)* geladen und dargestellt.
```csharp
private static void RegisterControls()
{
    try
    {
        if (Views == null)
        {
            Views = new Dictionary<Enum, Type>();
            Views.Add(CommandButtons.Home, typeof(HomeRibbonUC));
            Views.Add(CommandButtons.CustomA, typeof(CustomUC));
        }
    }
    catch (Exception ex)
    {
        string errorText = ex.Message;
        throw;
    }
}

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
```

Übergabe zum laden des UserControl
```csharp
base.EventAgg.Subscribe<ChangeViewEventArgs>(this.ChangeControl);
```

Laden und Darstellen des UserControl
```csharp
private void ChangeControl(ChangeViewEventArgs e)
{
}
```