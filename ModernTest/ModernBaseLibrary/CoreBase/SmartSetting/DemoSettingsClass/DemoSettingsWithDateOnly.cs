
namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using System;

    using global::ModernBaseLibrary.CoreBase;

    public class DemoSettingsWithDateOnly : SmartSettingsBase
    {
        public DemoSettingsWithDateOnly(string filePath) : base(filePath)
        {
        }

        public DateOnly DateOnlyProperty { get; set; }
    }
}
