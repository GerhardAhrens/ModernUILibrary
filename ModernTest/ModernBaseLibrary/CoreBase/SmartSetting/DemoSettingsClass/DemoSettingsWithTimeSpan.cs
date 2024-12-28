namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using System;

    using global::ModernBaseLibrary.CoreBase;

    public class DemoeSettingsWithTimeSpan : SmartSettingsBase
    {
        public DemoeSettingsWithTimeSpan(string filePath) : base(filePath)
        {
        }

        public TimeSpan TimeSpanProperty { get; set; }

    }
}
