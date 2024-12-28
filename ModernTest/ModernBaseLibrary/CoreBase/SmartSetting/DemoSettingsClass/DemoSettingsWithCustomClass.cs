namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using global::ModernBaseLibrary.CoreBase;

    public partial class DemoSettingsWithCustomClass : SmartSettingsBase
    {
        public CustomClass CustomClassProperty { get; set; }

        public DemoSettingsWithCustomClass(string filePath) : base(filePath)
        {
        }
    }

    public partial class DemoSettingsWithCustomClass
    {
        public class CustomClass
        {
            public int IntProperty { get; set; }

            public string StringProperty { get; set; }
        }
    }
}