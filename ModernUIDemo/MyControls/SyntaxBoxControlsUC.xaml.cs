﻿namespace ModernUIDemo.MyControls
{
    using System.ComponentModel;
    using System.Diagnostics.Eventing.Reader;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Versioning;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using ModernIU.Controls;

    /// <summary>
    /// Interaktionslogik für SyntaxBoxControlsUC.xaml
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class SyntaxBoxControlsUC : UserControl, INotifyPropertyChanged
    {
        const string LINE_COMMENT = "//";

        public SyntaxBoxControlsUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.Tag != null)
            {
                string sourceDemo = (string)this.Tag;
                Assembly assembly = Assembly.GetExecutingAssembly();
                string resourceName = $"ModernUIDemo.Resources.Source.{sourceDemo}";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            sourceDemo = reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        sourceDemo = $"Die Source Datei '{resourceName}' wurde nicht gefunden!";
                    }
                }

                this.TxtSyntaxBox.Text = sourceDemo;
            }
            else
            {
                string sourceDemo = string.Empty;
                Assembly assembly = Assembly.GetExecutingAssembly();
                var resourceName = "ModernUIDemo.Resources.Source.Demo_A.txt";
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            sourceDemo = reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        sourceDemo = $"Die Source Datei '{resourceName}' wurde nicht gefunden!";
                    }
                }

                this.TxtSyntaxBox.Text = sourceDemo;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool value = (bool)this.TxtSyntaxBox.GetValue(SyntaxBox.EnabledProperty);

            this.TxtSyntaxBox.SetValue(SyntaxBox.EnabledProperty, !value);
        }

        private static int FindLineStart(TextLine Line)
        {
            for (int i = 0; i < Line.Text.Length; i++)
            {
                if (!Char.IsWhiteSpace(Line.Text[i]))
                {
                    return (i);
                }
            }
            if (Line.Text.EndsWith(Environment.NewLine))
                return (Line.Text.Length - Environment.NewLine.Length);
            else
                return (Line.Text.Length);
        }

        public void OnCommentCommand(object sender, ExecutedRoutedEventArgs args)
        {
            int
                selStart = this.TxtSyntaxBox.SelectionStart,
                selLength = this.TxtSyntaxBox.SelectionLength,
                selEnd = selStart + selLength,
                firstLine = this.TxtSyntaxBox.GetLineIndexFromCharacterIndex(selStart),
                lastLine = selLength > 0
                    ? this.TxtSyntaxBox.GetLineIndexFromCharacterIndex(selStart + selLength - 1)
                    : firstLine;

            // Gets the currently selected lines
            var affectedLines = this.TxtSyntaxBox.Text.GetLines(firstLine, lastLine, out int totalLines).ToList();

            // These are the offset of the selection start/end from the END
            // of the first/last affected lines. They are used to reset the
            // selection after the operation.
            int selStartOffset = affectedLines[0].EndIndex - selStart;
            int selEndOffset = affectedLines[affectedLines.Count - 1].EndIndex - selEnd;

            // Find the smallest line start among the affected lines.
            // This is where we'll throw in the line comments.
            int insPos = affectedLines.Select(FindLineStart).Min();

            // Increase indent for the affected block.
            var indentedBlock = String.Join("", affectedLines.Select((line) => line.Text.Substring(0, insPos)
                    + LINE_COMMENT
                    + line.Text.Substring(insPos))
                .ToArray());

            // Update the text
            StringBuilder sb = new StringBuilder();
            sb.Append(this.TxtSyntaxBox.Text.Substring(0, affectedLines[0].StartIndex));
            sb.Append(indentedBlock);
            sb.Append(this.TxtSyntaxBox.Text.Substring(affectedLines[affectedLines.Count - 1].StartIndex + affectedLines[affectedLines.Count - 1].Text.Length));
            this.TxtSyntaxBox.Text = sb.ToString();

            // Reset the selection and caret
            var firstAffected = this.TxtSyntaxBox.Text.GetLines(firstLine, firstLine, out totalLines).Single();
            var lastAffected = this.TxtSyntaxBox.Text.GetLines(lastLine, lastLine, out totalLines).Single();
            selStart = firstAffected.EndIndex - selStartOffset;
            selEnd = lastAffected.EndIndex - selEndOffset;
            selLength = selEnd - selStart;
            this.TxtSyntaxBox.Select(selStart, selLength);
        }

        public void OnUncommentCommand(object sender, ExecutedRoutedEventArgs args)
        {
            int
                selStart = this.TxtSyntaxBox.SelectionStart,
                selLength = this.TxtSyntaxBox.SelectionLength,
                selEnd = selStart + selLength,
                firstLine = this.TxtSyntaxBox.GetLineIndexFromCharacterIndex(selStart),
                lastLine = selLength > 0
                    ? this.TxtSyntaxBox.GetLineIndexFromCharacterIndex(selStart + selLength - 1)
                    : firstLine;

            var affectedLines = this.TxtSyntaxBox.Text.GetLines(firstLine, lastLine, out int totalLines).ToList();

            // These are the offset of the selection start/end from the END
            // of the first/last affected lines. They are used to reset the
            // selection after the operation.
            int selStartOffset = affectedLines[0].EndIndex - selStart;
            int selEndOffset = affectedLines[affectedLines.Count - 1].EndIndex - selEnd;

            // Remove any comment prefix for the affected block.
            var unindentedBlock = String.Join("", affectedLines
                .Select((line) =>
                {
                    int start = FindLineStart(line);
                    if (line.Text.Substring(start).StartsWith(LINE_COMMENT))
                    {
                        return (line.Text.Substring(0, start) + line.Text.Substring(start + LINE_COMMENT.Length));
                    }
                    return (line.Text);
                }).ToArray());

            // Update the text
            StringBuilder sb = new StringBuilder();
            sb.Append(this.TxtSyntaxBox.Text.Substring(0, affectedLines[0].StartIndex));
            sb.Append(unindentedBlock);
            sb.Append(this.TxtSyntaxBox.Text.Substring(affectedLines[affectedLines.Count - 1].StartIndex + affectedLines[affectedLines.Count - 1].Text.Length));
            this.TxtSyntaxBox.Text = sb.ToString();

            // Reset the selection and caret
            var firstAffected = this.TxtSyntaxBox.Text.GetLines(firstLine, firstLine, out totalLines).Single();
            var lastAffected = this.TxtSyntaxBox.Text.GetLines(lastLine, lastLine, out totalLines).Single();
            selStart = Math.Max(firstAffected.StartIndex, firstAffected.EndIndex - selStartOffset);
            selEnd = Math.Max(lastAffected.StartIndex, lastAffected.EndIndex - selEndOffset);
            selLength = selEnd - selStart;
            this.TxtSyntaxBox.Select(selStart, selLength);
        }
        #region PropertyChanged Implementierung
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion PropertyChanged Implementierung
    }

    public static class ToggleCommands
    {
        public static ICommand CommentCommand = new RoutedCommand();
        public static ICommand UncommentCommand = new RoutedCommand();
    }
}