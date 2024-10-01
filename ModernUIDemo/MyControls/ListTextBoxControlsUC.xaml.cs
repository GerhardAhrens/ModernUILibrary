using ModernUIDemo.Core;

namespace ModernUIDemo.MyControls
{
    using System.Runtime.Versioning;
    using System.Windows.Controls;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für ListTextBoxControlsUC.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class ListTextBoxControlsUC : UserControl
    {
        ListTextBoxControlsVM vmRoot = null;

        public ListTextBoxControlsUC()
        {
            this.InitializeComponent();
            vmRoot = new ListTextBoxControlsVM();
            this.DataContext = vmRoot;
            this.ArtikelSource = new DemoData().BuildData();
        }

        private List<Artikel> ArtikelSource { get; set; }

        private void Txt_OnTextChange(object sender, TextChangedEventArgs e)
        {
            TextBox txtInput = sender as TextBox;
            var artikelSource = from emp in this.ArtikelSource where emp.FullName.ToLower().Contains(txtInput.Text.ToLower()) select emp;
            txt.ItemSource = artikelSource;
        }

        private void Txt_OnSelectedItemChange(object sender, ListTextBoxSelectedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                tb.Text = "Ausgewählter Artikel :";
            }
            else
            {
                tb.Text = "Ausgewählter Artikel : " + e.SelectedItem.ToString();
            }
        }

        private void Txt2_OnTextChange(object sender, TextChangedEventArgs e)
        {
            TextBox txtInput = sender as TextBox;
            var artikelSource = from emp in this.ArtikelSource where emp.FullName.ToLower().Contains(txtInput.Text.ToLower()) select emp;
            txt2.ItemSource = artikelSource;
        }

        private void Txt2_OnSelectedItemChange(object sender, ListTextBoxSelectedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                tb2.Text = "Ausgewählter Artikel :";
            }
            else
            {
                tb2.Text = "Ausgewählter Artikel : " + e.SelectedItem.ToString();
            }
        }

        private void Txt3_OnTextChange(object sender, TextChangedEventArgs e)
        {
            TextBox txtInput = sender as TextBox;
            var artikelSource = from emp in this.ArtikelSource where emp.ArtikelNummer.ToString().Contains(txtInput.Text.ToLower()) select emp;
            txt3.ItemSource = artikelSource;
        }

        private void Txt3_OnSelectedItemChange(object sender, ListTextBoxSelectedEventArgs e)
        {
            if (e.SelectedValue == null)
            {
                tb3.Text = "Ausgewählter Artikel :";
            }
            else
            {
                tb3.Text = "Ausgewählter Artikel : " + e.SelectedValue.ToString();
            }
        }
    }
}

public class ListTextBoxControlsVM : BindableBase
{
    private IEnumerable<Artikel> _DialogDataView;
    private object _CurrentSelectedItem;
    private object _CurrentSelectedValue;

    public IEnumerable<Artikel> DialogDataView
    {
        get { return this._DialogDataView; }
        set
        {
            this.SetProperty(ref this._DialogDataView, value);
        }
    }

    public object CurrentSelectedItem
    {
        get { return this._CurrentSelectedItem; }
        set { this.SetProperty(ref this._CurrentSelectedItem, value); }
    }

    public object CurrentSelectedValue
    {
        get { return this._CurrentSelectedValue; }
        set
        {
            this.SetProperty(ref this._CurrentSelectedValue, value);
            this.Check(this._CurrentSelectedValue);
        }
    }

    private void Check(object obj)
    {
    }

    private void LoadData()
    {
        this.DialogDataView = new List<Artikel>(new DemoData().BuildData());
    }
}

public class DemoData
{
    private readonly List<Artikel> demoModels = null;
    public DemoData()
    {
        this.demoModels = new List<Artikel>();
    }

    public List<Artikel> BuildData()
    {
        this.demoModels.Add(new Artikel() { ArtikelId = Guid.NewGuid(), ArtikelNummer = 4711, FullName = "Mütze", Preis = 21.00M, Farbe = "Grün" });
        this.demoModels.Add(new Artikel() { ArtikelId = Guid.NewGuid(), ArtikelNummer = 4712, FullName = "Mütze", Preis = 19.00M, Farbe = "Blau" });
        this.demoModels.Add(new Artikel() { ArtikelId = Guid.NewGuid(), ArtikelNummer = 4211, FullName = "Hemd", Preis = 15.00M, Farbe = "Grün" });
        this.demoModels.Add(new Artikel() { ArtikelId = Guid.NewGuid(), ArtikelNummer = 4212, FullName = "Socken", Preis = 3.00M, Farbe = "Blau" });
        this.demoModels.Add(new Artikel() { ArtikelId = Guid.NewGuid(), ArtikelNummer = 1000, FullName = "Schuhe", Preis = 43.00M, Farbe = "Bunt" });
        this.demoModels.Add(new Artikel() { ArtikelId = Guid.NewGuid(), ArtikelNummer = 1010, FullName = "Schuhe", Preis = 46.00M, Farbe = "Grün" });

        return this.demoModels;
    }
}

public class Artikel
{
    public Guid ArtikelId { get; set; }

    public int ArtikelNummer { get; set; }

    public string FullName { get; set; }

    public decimal Preis { get; set; }

    public string Farbe { get; set; }

    public override string ToString()
    {
        return $"{this.ArtikelNummer}-{this.FullName}, {this.Farbe}, {this.Preis}";
    }
}