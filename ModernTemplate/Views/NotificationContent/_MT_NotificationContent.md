# Verzeichnis NotificationContent

Im Verzeichnis **NotificationContent** sind die verschiedenen Meldungstypen abgelegt. Ein Meldungstyp besteht immer aus einen UserControl erstellt. 
Der grundsätzliche Aufbau dabei ist immer gleich.
- Titelzeile
- Contentbereich
- Fusszeile mit Buttonelemente vom Typ *NotificationBoxButton*.

Beispiel des Code-Behinde einer *QuestionYesNo* Meldung

```csharp
public partial class QuestionYesNo : UserControl, INotificationServiceMessage
{
    public QuestionYesNo()
    {
        this.InitializeComponent();
        WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
    }

    public int CountDown { get; set; }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        (string InfoText, string CustomText, double FontSize) textOption = ((string InfoText, string CustomText, double FontSize))this.Tag;

        this.TbHeader.Text = textOption.Item1;
        this.TbFull.Text = textOption.Item2;
        this.TbFull.FontSize = textOption.Item3;
    }

    private void BtnYes_Click(object sender, RoutedEventArgs e)
    {
        Window window = this.Parent as Window;
        window.Tag = new Tuple<NotificationBoxButton>(NotificationBoxButton.Yes);
        window.DialogResult = true;
        window.Close();
    }

    private void BtnNo_Click(object sender, RoutedEventArgs e)
    {
        Window window = this.Parent as Window;
        window.Tag = new Tuple<NotificationBoxButton>(NotificationBoxButton.No);
        window.DialogResult = false;
        window.Close();
    }
}
```

Der grundlegende Aufbau ist unabhängig vom *Notification Typ* immer gleich.

