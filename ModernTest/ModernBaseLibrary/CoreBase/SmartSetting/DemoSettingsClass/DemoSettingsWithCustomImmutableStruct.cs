namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using global::ModernBaseLibrary.CoreBase;

    public partial class DemoSettingsWithCustomImmutableStruct : SmartSettingsBase
    {
        public CustomStruct CustomStructProperty { get; set; }

        public DemoSettingsWithCustomImmutableStruct(string filePath) : base(filePath)
        {
        }
    }

    public partial class DemoSettingsWithCustomImmutableStruct
    {
        public readonly struct CustomStruct
        {
            public int IntProperty { get; }

            public string StringProperty { get; }

            public CustomStruct(int intProperty, string stringProperty)
            {
                IntProperty = intProperty;
                StringProperty = stringProperty;
            }
        }
    }
}