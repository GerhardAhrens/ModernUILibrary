//-----------------------------------------------------------------------
// <copyright file="MCheckBoxEx.cs" company="Lifeprojects.de">
//     Class: MCheckBoxEx
//     Copyright © Gerhard Ahrens, 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>27.07.2018</date>
//
// <summary>Class for UI Control CheckBox</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using ModernIU.Base;

    public sealed class MCheckBoxEx : CheckBox
    {
        public static readonly DependencyProperty ReadOnlyBackgroundColorProperty = DependencyProperty.Register("ReadOnlyBackgroundColor", typeof(Brush), typeof(MCheckBoxEx), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(222, 222, 222))));

        static MCheckBoxEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MCheckBoxEx), new FrameworkPropertyMetadata(typeof(MCheckBoxEx)));
        }

        public MCheckBoxEx()
        {
            string styleTextChk = new StyleText().Add("CheckBox", this.CheckBoxFlatCoreStyle()).Value;
            Style checkBoxStyle = XAMLBuilder<Style>.GetStyle(styleTextChk);
            this.Style = checkBoxStyle;

            this.FontSize = ControlBase.FontSize;
            this.FontFamily = ControlBase.FontFamily;
            this.BorderBrush = ControlBase.BorderBrush;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Padding = new Thickness(0);
            this.Margin = new Thickness(2);
            this.MinHeight = 18;
            this.Height = 25;
            this.ClipToBounds = false;
        }

        public Brush ReadOnlyBackgroundColor
        {
            get { return GetValue(ReadOnlyBackgroundColorProperty) as Brush; }
            set { this.SetValue(ReadOnlyBackgroundColorProperty, value); }
        }

        private string CheckBoxFlatCoreStyle()
        {
            string result = string.Empty;

            result = "<Setter Property=\"SnapsToDevicePixels\" Value=\"true\" />\r\n        " +
                "     <Setter Property=\"OverridesDefaultStyle\" Value=\"true\" />\r\n        " +
                "     <Setter Property=\"Background\" Value=\"WhiteSmoke\" />\r\n        " +
                "     <Setter Property=\"Height\" Value=\"25\" />\r\n        " +
                "     <Setter Property=\"Template\">\r\n" +
                "     <Setter.Value>\r\n" +
                "           <ControlTemplate TargetType=\"{x:Type CheckBox}\">\r\n" +
                "                    <BulletDecorator>\r\n" +
                "                    <BulletDecorator.Bullet>\r\n" +
                "                    <Grid\r\n" +
                "                          Width=\"{Binding RelativeSource={RelativeSource Self}, Path=Height, UpdateSourceTrigger=PropertyChanged}\"\r\n" +
                "                          Height=\"{TemplateBinding Height}\"\r\n" +
                "                          MinWidth=\"25\"\r\n" +
                "                          MinHeight=\"25\"\r\n" +
                "                          ShowGridLines=\"False\">\r\n" +
                "                          <Grid.ColumnDefinitions>\r\n" +
                "                                <ColumnDefinition Width=\"4*\" />\r\n" +
                "                                <ColumnDefinition Width=\"1*\" />\r\n" +
                "                                <ColumnDefinition Width=\"1*\" />\r\n" +
                "                                <ColumnDefinition Width=\"4*\" />\r\n" +
                "                                <ColumnDefinition Width=\"1*\" />\r\n" +
                "                                <ColumnDefinition Width=\"1*\" />\r\n" +
                "                                <ColumnDefinition Width=\"2*\" />\r\n" +
                "                                <ColumnDefinition Width=\"2*\" />\r\n" +
                "                         </Grid.ColumnDefinitions>\r\n" +
                "                         <Grid.RowDefinitions>\r\n" +
                     "                         <RowDefinition Height=\"3*\" />\r\n" +
                     "                         <RowDefinition Height=\"1*\" />\r\n" +
                     "                         <RowDefinition Height=\"1*\" />\r\n" +
                     "                         <RowDefinition Height=\"1*\" />\r\n" +
                     "                         <RowDefinition Height=\"4*\" />\r\n" +
                     "                         <RowDefinition Height=\"1*\" />\r\n" +
                     "                         <RowDefinition Height=\"1*\" />\r\n" +
                     "                         <RowDefinition Height=\"4*\" />\r\n" +
                     "                    </Grid.RowDefinitions>\r\n\r\n" +
                     "                    <Border Name=\"MainBorder\"\r\n" +
                     "                            Grid.RowSpan=\"9\"\r\n" +
                     "                            Grid.ColumnSpan=\"9\"\r\n" +
                     "                            Background=\"Transparent\"\r\n" +
                     "                            BorderThickness=\"1\"\r\n" +
                     "                            CornerRadius=\"4\" />\r\n" +
                     "                            <Path Name=\"CheckMark\"\r\n" +
                     "                                  Grid.Row=\"1\"\r\n" +
                     "                                  Grid.RowSpan=\"5\"\r\n" +
                     "                                  Grid.Column=\"2\"\r\n" +
                     "                                  Grid.ColumnSpan=\"5\"\r\n" +
                     "                                  Data=\"M9.07743946676476E-09,4.31805768640244L4.68740335877841,8.86361158398516C4.68740335877841,8.86361158398516,16.3281249985376,-2.42451336648723,16.3281249985376,-2.42451336648723L14.0622100581796,-4.77304938341948 4.68740335877846,4.31805791992662 2.22656251699567,1.93164208562579z\"\r\n" +
                     "                                  Fill=\"Green\"\r\n" +
                     "                                  Opacity=\"0\"\r\n" +
                     "                                  Stretch=\"Fill\"\r\n" +
                     "                                    Stroke=\"Green\" />\r\n" +
                     "                    <Border Name=\"InnerBorder\"\r\n" +
                     "                            Grid.Row=\"2\"\r\n" +
                     "                            Grid.RowSpan=\"5\"\r\n" +
                     "                            Grid.Column=\"1\"\r\n" +
                     "                            Grid.ColumnSpan=\"5\"\r\n" +
                     "                            Background=\"WhiteSmoke\"\r\n" +
                     "                            BorderBrush=\"#808080\"\r\n" +
                     "                            BorderThickness=\"1\" />\r\n\r\n" +
                     "                            <Path Name=\"InnerPath\"\r\n" +
                     "                                  Grid.Row=\"2\"\r\n" +
                     "                                  Grid.RowSpan=\"5\"\r\n" +
                     "                                  Grid.Column=\"1\"\r\n" +
                     "                                  Grid.ColumnSpan=\"5\"\r\n" +
                     "                                  Data=\"M31,5 L19.5,5 19.5,19.5 34.5,19.5 34.5,11.75\"\r\n" +
                     "                                  Stretch=\"Fill\"\r\n" +
                     "                                  Stroke=\"#808080\" />\r\n\r\n" +
                     "                            <Path Name=\"InderminateMark\"\r\n" +
                     "                                  Grid.Row=\"4\"\r\n" +
                     "                                  Grid.Column=\"3\"\r\n" +
                     "                                  Data=\"M0,4 L1,5 5,1 4,0\"\r\n" +
                     "                                  Fill=\"#808080\"\r\n" +
                     "                                  Opacity=\"0\"\r\n" +
                     "                                  Stretch=\"Fill\"\r\n" +
                     "                                  StrokeThickness=\"0\" />\r\n" +
                     "                            </Grid>\r\n" +
                     "                            </BulletDecorator.Bullet>\r\n" +
                     "                              <ContentPresenter\r\n" +
                     "                               Margin=\"4,0,4,0\"\r\n" +
                     "                               HorizontalAlignment=\"Left\"\r\n" +
                     "                               VerticalAlignment=\"Center\"\r\n" +
                     "                               RecognizesAccessKey=\"True\" />\r\n" +
                     "                              <VisualStateManager.VisualStateGroups>\r\n" +
                     "                              <VisualStateGroup x:Name=\"CheckStates\">\r\n" +
                     "                              <VisualState x:Name=\"Checked\">\r\n" +
                     "                              <Storyboard>\r\n" +
                     "                                  <DoubleAnimation Storyboard.TargetName=\"CheckMark\"\r\n" +
                     "                                                   Storyboard.TargetProperty=\"Opacity\"\r\n" +
                     "                                                   To=\"1\"\r\n" +
                     "                                                   Duration=\"0:0:0.2\" />\r\n" +
                     "                                    </Storyboard>\r\n" +
                     "                                </VisualState>\r\n" +
                     "                                <VisualState x:Name=\"Unchecked\">\r\n" +
                     "                                    <Storyboard>\r\n" +
                     "                                        <DoubleAnimation\r\n" +
                     "                                            Storyboard.TargetName=\"CheckMark\"\r\n" +
                     "                                            Storyboard.TargetProperty=\"Opacity\"\r\n" +
                     "                                            To=\"0\"\r\n" +
                     "                                            Duration=\"0:0:0.2\" />\r\n" +
                     "                                    </Storyboard>\r\n" +
                     "                                </VisualState>\r\n" +
                     "                                <VisualState x:Name=\"Indeterminate\">\r\n" +
                     "                                    <Storyboard>\r\n" +
                     "                                        <DoubleAnimation\r\n" +
                     "                                            Storyboard.TargetName=\"InderminateMark\"\r\n" +
                     "                                            Storyboard.TargetProperty=\"Opacity\"\r\n" +
                     "                                            To=\"1\"\r\n" +
                     "                                            Duration=\"0:0:0.2\" />\r\n" +
                     "                                    </Storyboard>\r\n" +
                     "                                </VisualState>\r\n" +
                     "                            </VisualStateGroup>\r\n" +
                     "                        </VisualStateManager.VisualStateGroups>\r\n" +
                     "                    </BulletDecorator>\r\n" +
                     "                    <ControlTemplate.Triggers>\r\n" +
                     "                        <Trigger Property=\"IsChecked\" Value=\"True\">\r\n" +
                     "                            <Setter TargetName=\"InnerBorder\" Property=\"Visibility\" Value=\"Collapsed\" />\r\n" +
                     "                            <Setter Property=\"Background\" Value=\"WhiteSmoke\" />\r\n" +
                     "                        </Trigger>\r\n" +
                     "                        <Trigger Property=\"IsPressed\" Value=\"True\">\r\n" +
                     "                            <Setter TargetName=\"MainBorder\" Property=\"Background\" Value=\"#81d2eb\" />\r\n" +
                     "                        </Trigger>\r\n" +
                     "                        <Trigger Property=\"IsEnabled\" Value=\"False\">\r\n" +
                     "                            <Setter TargetName=\"CheckMark\" Property=\"Fill\" Value=\"#cccccc\" />\r\n" +
                     "                            <Setter TargetName=\"CheckMark\" Property=\"Stroke\" Value=\"#cccccc\" />\r\n" +
                     "                            <Setter TargetName=\"InnerPath\" Property=\"Stroke\" Value=\"#cccccc\" />\r\n" +
                     "                            <Setter TargetName=\"InderminateMark\" Property=\"Fill\" Value=\"#cccccc\" />\r\n" +
                     "                            <Setter TargetName=\"InnerBorder\" Property=\"BorderBrush\" Value=\"#cccccc\" />\r\n" +
                     "                        </Trigger>\r\n" +
                     "                    </ControlTemplate.Triggers>\r\n" +
                "           </ControlTemplate>\r\n" +
                "    </Setter.Value>\r\n" +
                "</Setter>";

            return result;
        }
    }
}