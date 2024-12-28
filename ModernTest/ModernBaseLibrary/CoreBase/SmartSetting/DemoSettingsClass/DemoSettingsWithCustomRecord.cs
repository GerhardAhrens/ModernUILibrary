namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using global::ModernBaseLibrary.CoreBase;

    public partial class DemoSettingsWithCustomRecord : SmartSettingsBase
    {
        public CustomRecord CustomRecordProperty { get; set; }

        public DemoSettingsWithCustomRecord(string filePath) : base(filePath)
        {
        }
    }

    public partial class DemoSettingsWithCustomRecord
    {
        public record CustomRecord(int IntProperty, string StringProperty);
    }
}