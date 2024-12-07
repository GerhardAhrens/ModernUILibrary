namespace ModernIU.Controls
{
    public class TextLine
    {
        public int LineNumber;
        /// <summary>
        /// The start index of the line relative to the entire text.
        /// </summary>
        public int StartIndex;
        public string Text;
        public int EndIndex => StartIndex + Text?.Length ?? 0;

    }
}
