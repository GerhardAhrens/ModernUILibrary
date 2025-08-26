namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    public class SearchableTextBlock : TextBlock
    {
        #region Constructors

        public SearchableTextBlock()
        {
            Init();
        }

        public SearchableTextBlock(Inline inline) : base(inline)
        {
            Init();
        }

        private void Init()
        {
            OnSearchWordsChanged += UpdateRegex;
            OnRegularExpressionChanged += RefreshHighlightedText;
            OnHighlightableTextChanged += RefreshHighlightedText;
            OnRegexOptionsChanged += RefreshHighlightedText;
        }

        #endregion

        #region Properties
        new private string Text
        {
            set
            {
                if (!RegularExpression.IsValidRegex())
                {
                    base.Text = value;
                    return;
                }
                Inlines.Clear();
                Inlines.AddRange(value.GetRunLines(this, RegularExpression, RegexOptions));
            }
        }

        #endregion

        #region Dependency Properties

        #region Regex
        public event EventHandler OnRegularExpressionChanged;

        public string RegularExpression
        {
            get
            {
                if (null == (string)GetValue(RegularExpressionProperty))
                    SetValue(RegularExpressionProperty, "");
                return (string)GetValue(RegularExpressionProperty);
            }
            set { SetValue(RegularExpressionProperty, value); }
        }

        public static readonly DependencyProperty RegularExpressionProperty =
            DependencyProperty.Register("RegularExpression", typeof(string), typeof(SearchableTextBlock), new PropertyMetadata(new PropertyChangedCallback(RegularExpressionPropertyChanged)));

        public static void RegularExpressionPropertyChanged(DependencyObject inDO, DependencyPropertyChangedEventArgs inArgs)
        {
            SearchableTextBlock stb = inDO as SearchableTextBlock;
            stb?.OnRegularExpressionChanged?.Invoke(stb, null);
        }
        #endregion

        #region Search Words
        public event EventHandler OnSearchWordsChanged;

        public List<string> SearchWords
        {
            get
            {
                if (null == (List<string>)GetValue(SearchWordsProperty))
                    SetValue(SearchWordsProperty, new List<string>());
                return (List<string>)GetValue(SearchWordsProperty);
            }
            set { SetValue(SearchWordsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchStringList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchWordsProperty =
            DependencyProperty.Register("SearchWords", typeof(List<string>), typeof(SearchableTextBlock), new PropertyMetadata(new PropertyChangedCallback(SearchWordsPropertyChanged)));

        public static void SearchWordsPropertyChanged(DependencyObject inDO, DependencyPropertyChangedEventArgs inArgs)
        {
            SearchableTextBlock stb = inDO as SearchableTextBlock;
            stb?.OnSearchWordsChanged?.Invoke(stb, null);
        }
        #endregion

        #region HighlightableText
        public event EventHandler OnHighlightableTextChanged;

        public string HighlightableText
        {
            get { return (string)GetValue(HighlightableTextProperty); }
            set { SetValue(HighlightableTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HighlightableText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightableTextProperty =
            DependencyProperty.Register("HighlightableText", typeof(string), typeof(SearchableTextBlock), new PropertyMetadata(new PropertyChangedCallback(HighlightableTextChanged)));

        public static void HighlightableTextChanged(DependencyObject inDO, DependencyPropertyChangedEventArgs inArgs)
        {
            SearchableTextBlock stb = inDO as SearchableTextBlock;
            if (stb == null)
                return;
            stb.Text = stb.HighlightableText;
            stb.OnHighlightableTextChanged?.Invoke(stb, null);
        }
        #endregion

        #region HighlightFontWeight
        public event EventHandler OnHighlightFontWeightChanged;

        public FontWeight HighlightFontWeight
        {
            get { return (FontWeight)GetValue(HighlightFontWeightProperty); }
            set { SetValue(HighlightFontWeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HighlightFontWeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightFontWeightProperty =
            DependencyProperty.Register("HighlightFontWeight", typeof(FontWeight), typeof(SearchableTextBlock), new PropertyMetadata(FontWeights.Normal, new PropertyChangedCallback(HighlightableFontWeightChanged)));


        public static void HighlightableFontWeightChanged(DependencyObject inDO, DependencyPropertyChangedEventArgs inArgs)
        {
            SearchableTextBlock stb = inDO as SearchableTextBlock;
            stb?.OnHighlightFontWeightChanged?.Invoke(stb, null);
        }
        #endregion

        #region HighlightForeground
        public event EventHandler OnHighlightForegroundChanged;

        public Brush HighlightForeground
        {
            get { return (Brush)GetValue(HighlightForegroundProperty); }
            set { SetValue(HighlightForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HighlightForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightForegroundProperty =
            DependencyProperty.Register("HighlightForeground", typeof(Brush), typeof(SearchableTextBlock), new PropertyMetadata(Brushes.Black, new PropertyChangedCallback(HighlightableForegroundChanged)));


        public static void HighlightableForegroundChanged(DependencyObject inDO, DependencyPropertyChangedEventArgs inArgs)
        {
            SearchableTextBlock stb = inDO as SearchableTextBlock;
            stb?.OnHighlightForegroundChanged?.Invoke(stb, null);
        }
        #endregion

        #region HighlightBackground
        public event EventHandler OnHighlightBackgroundChanged;

        public Brush HighlightBackground
        {
            get { return (Brush)GetValue(HighlightBackgroundProperty); }
            set { SetValue(HighlightBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HighlightBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightBackgroundProperty =
            DependencyProperty.Register("HighlightBackground", typeof(Brush), typeof(SearchableTextBlock), new PropertyMetadata(Brushes.Yellow, new PropertyChangedCallback(HighlightableBackgroundChanged)));


        public static void HighlightableBackgroundChanged(DependencyObject inDO, DependencyPropertyChangedEventArgs inArgs)
        {
            SearchableTextBlock stb = inDO as SearchableTextBlock;
            stb?.OnHighlightBackgroundChanged?.Invoke(stb, null);
        }
        #endregion

        #region RegexOptions
        public event EventHandler OnRegexOptionsChanged;

        public RegexOptions RegexOptions
        {
            get { return (RegexOptions)GetValue(RegexOptionsProperty); }
            set { SetValue(RegexOptionsProperty, value); }
        }

        public static readonly DependencyProperty RegexOptionsProperty =
            DependencyProperty.Register("RegexOptions", typeof(RegexOptions), typeof(SearchableTextBlock), new PropertyMetadata(RegexOptions.IgnoreCase, new PropertyChangedCallback(RegexOptionsChanged)));


        public static void RegexOptionsChanged(DependencyObject inDO, DependencyPropertyChangedEventArgs inArgs)
        {
            SearchableTextBlock stb = inDO as SearchableTextBlock;
            stb.OnRegexOptionsChanged?.Invoke(stb, null);
        }
        #endregion

        #endregion

        #region Methods
        public void AddSearchString(string inString)
        {
            SearchWords.Add(inString);
            OnSearchWordsChanged?.Invoke(this, null);
        }

        public void RefreshHighlightedText(object sender, EventArgs e)
        {
            Text = base.Text;
        }

        private void UpdateRegex(object sender, EventArgs e)
        {
            RegularExpression = SearchWords.ToRegex();
        }
        #endregion
    }
}
