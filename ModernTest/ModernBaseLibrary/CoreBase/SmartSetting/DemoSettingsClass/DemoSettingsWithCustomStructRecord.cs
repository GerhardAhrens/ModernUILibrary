namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using global::ModernBaseLibrary.CoreBase;

    public partial class DemoSettingsWithCustomStructRecord : SmartSettingsBase
    {
        public DemoSettingsWithCustomStructRecord(string filePath) : base(filePath)
        {
        }

        public CustomRecord CustomRecordProperty { get; set; }
    }

    public partial class DemoSettingsWithCustomStructRecord
    {
        public record struct CustomRecord(int IntProperty, string StringProperty);
    }
}
