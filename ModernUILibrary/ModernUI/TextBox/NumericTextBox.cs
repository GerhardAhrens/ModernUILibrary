using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;

using ModernIU.Base;

namespace ModernIU.Controls
{
    public class NumericTextBox : MTextBoxBase
    {
        private UIElement PART_ErrorPath;

        static NumericTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericTextBox), new FrameworkPropertyMetadata(typeof(NumericTextBox)));
        }

        public string Pattern
        {
            get { return (string)GetValue(PatternProperty); }
            set { SetValue(PatternProperty, value); }
        }
        
        public static readonly DependencyProperty PatternProperty =
            DependencyProperty.Register("Pattern", typeof(string), typeof(NumericTextBox), new PropertyMetadata(string.Empty));

        public string ErrorContent
        {
            get { return (string)GetValue(ErrorContentProperty); }
            set { SetValue(ErrorContentProperty, value); }
        }
        
        public static readonly DependencyProperty ErrorContentProperty =
            DependencyProperty.Register("ErrorContent", typeof(string), typeof(NumericTextBox), new PropertyMetadata(string.Empty));

        public EnumPatternType PatternType
        {
            get { return (EnumPatternType)GetValue(PatternTypeProperty); }
            set { SetValue(PatternTypeProperty, value); }
        }
        
        public static readonly DependencyProperty PatternTypeProperty =
            DependencyProperty.Register("PatternType", typeof(EnumPatternType), typeof(NumericTextBox), new PropertyMetadata(EnumPatternType.None, PatternTypeCallback));

        private static void PatternTypeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericTextBox textBox = d as NumericTextBox;
            SetPatternAndTip((EnumPatternType)e.NewValue, textBox, false);
        }

        public EnumValidateTrigger ValidateTrigger
        {
            get { return (EnumValidateTrigger)GetValue(ValidateTriggerProperty); }
            set { SetValue(ValidateTriggerProperty, value); }
        }
        
        public static readonly DependencyProperty ValidateTriggerProperty =
            DependencyProperty.Register("ValidateTrigger", typeof(EnumValidateTrigger), typeof(NumericTextBox), new PropertyMetadata(EnumValidateTrigger.PropertyChanged, ValidateTriggerCallback));

        private static void ValidateTriggerCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericTextBox textBox = d as NumericTextBox;
            switch ((EnumValidateTrigger)e.NewValue)
            {
                case EnumValidateTrigger.PropertyChanged:
                    textBox.LostFocus -= textBox.NumbericTextBox_LostFocus;
                    textBox.TextChanged += textBox.NumbericTextBox_TextChanged;
                    break;
                case EnumValidateTrigger.LostFocus:
                    textBox.LostFocus += textBox.NumbericTextBox_LostFocus;
                    textBox.TextChanged -= textBox.NumbericTextBox_TextChanged;
                    break;
            }
        }

        public bool IsHasError
        {
            get { return (bool)GetValue(IsHasErrorProperty); }
            private set { SetValue(IsHasErrorProperty, value); }
        }
        
        public static readonly DependencyProperty IsHasErrorProperty =
            DependencyProperty.Register("IsHasError", typeof(bool), typeof(NumericTextBox), new PropertyMetadata(false));

        public bool IsShowErrorTip
        {
            get { return (bool)GetValue(IsShowErrorTipProperty); }
            set { SetValue(IsShowErrorTipProperty, value); }
        }
        
        public static readonly DependencyProperty IsShowErrorTipProperty =
            DependencyProperty.Register("IsShowErrorTip", typeof(bool), typeof(NumericTextBox), new PropertyMetadata(false));

        public DataTemplate ErrorContentTemplate
        {
            get { return (DataTemplate)GetValue(ErrorContentTemplateProperty); }
            set { SetValue(ErrorContentTemplateProperty, value); }
        }
        
        public static readonly DependencyProperty ErrorContentTemplateProperty =
            DependencyProperty.Register("ErrorContentTemplate", typeof(DataTemplate), typeof(NumericTextBox));

        public Brush ErrorBackground
        {
            get { return (Brush)GetValue(ErrorBackgroundProperty); }
            set { SetValue(ErrorBackgroundProperty, value); }
        }
        
        public static readonly DependencyProperty ErrorBackgroundProperty =
            DependencyProperty.Register("ErrorBackground", typeof(Brush), typeof(NumericTextBox));

        public bool IsStartValidate
        {
            get { return (bool)GetValue(IsStartValidateProperty); }
            set { SetValue(IsStartValidateProperty, value); }
        }
        
        public static readonly DependencyProperty IsStartValidateProperty =
            DependencyProperty.Register("IsStartValidate", typeof(bool), typeof(NumericTextBox), new PropertyMetadata(false, IsStartValidateCallback));

        private static void IsStartValidateCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericTextBox textBox = d as NumericTextBox;
            if((bool)e.NewValue)
            {
                textBox.ValidateTextBox();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            switch (this.ValidateTrigger)
            {
                case EnumValidateTrigger.PropertyChanged:
                    this.TextChanged += NumbericTextBox_TextChanged;
                    this.LostFocus -= NumbericTextBox_LostFocus;
                    break;
                case EnumValidateTrigger.LostFocus:
                    this.TextChanged -= NumbericTextBox_TextChanged;
                    this.LostFocus += NumbericTextBox_LostFocus;
                    break;
            }

            //this.PART_ErrorPath = this.GetTemplateChild("PART_ErrorPath") as UIElement;
            //if(this.PART_ErrorPath != null)
            //{
            //    this.PART_ErrorPath.MouseEnter += (o, e) =>
            //    {
            //        this.IsShowErrorTip = true;
            //    };
            //    this.PART_ErrorPath.MouseLeave += (o, e) =>
            //    {
            //        this.IsShowErrorTip = false;
            //    };
            //}

            NumericTextBox.SetPatternAndTip(this.PatternType, this, true);
        }

        private void NumbericTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.ValidateTextBox();
        }

        private void NumbericTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this.ValidateTextBox();
        }

        private void ValidateTextBox()
        {
            if (!string.IsNullOrWhiteSpace(this.Pattern))
            {
                Regex regex = new Regex(this.Pattern);
                if (!regex.IsMatch(this.Text))
                {
                    VisualStateManager.GoToState(this, "InputError", true);
                    this.IsHasError = true;
                    this.IsShowErrorTip = !string.IsNullOrWhiteSpace(this.ErrorContent);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Normal", true);
                    this.IsHasError = false;
                    this.IsShowErrorTip = false;
                }
            }
        }

        private static void SetPatternAndTip(EnumPatternType patternType, NumericTextBox textBox, bool isFirstLoad)
        {
            switch (patternType)
            {
                case EnumPatternType.NotEmpty:
                    textBox.Pattern = "\\S";
                    textBox.ErrorContent = string.IsNullOrWhiteSpace(textBox.ErrorContent) ? "Kann nicht leer sein" : textBox.ErrorContent;
                    break;
                case EnumPatternType.OnlyNumber:
                    textBox.Pattern = "^[0-9]*$";
                    textBox.ErrorContent = string.IsNullOrWhiteSpace(textBox.ErrorContent) ? "Nur numerische Eingaben" : textBox.ErrorContent;
                    break;
                case EnumPatternType.IPV4:
                    textBox.Pattern = "\\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\b";
                    textBox.ErrorContent = string.IsNullOrWhiteSpace(textBox.ErrorContent) ? "IP-Adresse ist falsch, bitte geben Sie die richtige IPV4-Adresse ein" : textBox.ErrorContent;
                    break;
                case EnumPatternType.IPV6:
                    textBox.Pattern = "(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))";
                    textBox.ErrorContent = string.IsNullOrWhiteSpace(textBox.ErrorContent) ? "IP-Adresse ist falsch, bitte geben Sie die richtige IPV6-Adresse ein" : textBox.ErrorContent;
                    break;
                case EnumPatternType.Email:
                    textBox.Pattern = "[\\w!#$%&'*+/=?^_`{|}~-]+(?:\\.[\\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\\w](?:[\\w-]*[\\w])?\\.)+[\\w](?:[\\w-]*[\\w])?";
                    textBox.ErrorContent = string.IsNullOrWhiteSpace(textBox.ErrorContent) ? "Falsche E-Mail Adresse" : textBox.ErrorContent;
                    break;
                case EnumPatternType.MobilePhone:
                    textBox.Pattern = "^((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,5-9]))\\d{8}$";
                    textBox.ErrorContent = string.IsNullOrWhiteSpace(textBox.ErrorContent) ? "手机号码输入不正确" : textBox.ErrorContent;
                    break;
                case EnumPatternType.Telephone:
                    textBox.Pattern = "d{3}-d{8}|d{4}-d{7}";
                    textBox.ErrorContent = string.IsNullOrWhiteSpace(textBox.ErrorContent) ? "电话号码输入不正确" : textBox.ErrorContent;
                    break;
            }
        }
    }
}
