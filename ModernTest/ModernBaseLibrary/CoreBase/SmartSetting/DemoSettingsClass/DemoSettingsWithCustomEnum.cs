namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using global::ModernBaseLibrary.CoreBase;

    public partial class DemoSettingsWithCustomEnum : SmartSettingsBase
    {
        public CustomEnum CustomEnumProperty { get; set; }

        public DemoSettingsWithCustomEnum(string filePath) : base(filePath)
        {
        }
    }

    public partial class DemoSettingsWithCustomEnum
    {
        public enum CustomEnum
        {
            Foo,
            Bar
        }
    }
}
