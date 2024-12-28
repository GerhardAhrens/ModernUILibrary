
namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using global::ModernBaseLibrary.CoreBase;

    internal class DemoSettings : SmartSettingsBase
    {
        public DemoSettings(string filePath) : base(filePath)
        {
        }
        public int IntProperty { get; set; }

        public bool BoolProperty { get; set; }

        public string StringProperty { get; set; }

        public string StringPropertyWithDefaultValue { get; set; } = "Default value";
    }
}
