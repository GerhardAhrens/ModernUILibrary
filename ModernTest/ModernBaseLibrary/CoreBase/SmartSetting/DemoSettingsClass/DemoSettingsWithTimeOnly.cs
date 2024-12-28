
namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using System;

    using global::ModernBaseLibrary.CoreBase;

    public class DemoSettingsWithTimeOnly : SmartSettingsBase
    {
        public DemoSettingsWithTimeOnly(string filePath) : base(filePath)
        {
        }

        public TimeOnly TimeOnlyProperty { get; set; }
    }
}