namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using global::ModernBaseLibrary.CoreBase;

    public partial class DemoSettingsWithCustomImmutableClass : SmartSettingsBase
    {
        public CustomClass CustomClassProperty { get; set; }

        public DemoSettingsWithCustomImmutableClass(string filePath) : base(filePath)
        {
        }
    }

    public partial class DemoSettingsWithCustomImmutableClass
    {
        public class CustomClass
        {
            public int IntProperty { get; }

            public string StringProperty { get; }

            public CustomClass(int intProperty, string stringProperty)
            {
                IntProperty = intProperty;
                StringProperty = stringProperty;
            }
        }
    }
}