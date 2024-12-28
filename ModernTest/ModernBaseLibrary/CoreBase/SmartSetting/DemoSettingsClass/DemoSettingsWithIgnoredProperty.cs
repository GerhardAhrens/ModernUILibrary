namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using System.Text.Json.Serialization;

    using global::ModernBaseLibrary.CoreBase;

    public class DemoSettingsWithIgnoredProperty : SmartSettingsBase
    {
        public DemoSettingsWithIgnoredProperty(string filePath) : base(filePath)
        {
        }

        public int IntProperty { get; set; }

        [JsonIgnore]
        public string IgnoredProperty { get; set; }
    }
}