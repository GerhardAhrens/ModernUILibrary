namespace ModernBaseLibrary.Extension
{
    using System.Diagnostics;

    [DebuggerDisplay("IntValue={IntValue}; Active={Active}")]
    public class Nummer : INummer
    {
        public bool Active { get; set; }

        public int IntValue { get; set; }
    }
}