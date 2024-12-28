namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using System.Text.Json.Serialization;

    using global::ModernBaseLibrary.CoreBase;

    public class DemoSettingsWithNamedProperty : SmartSettingsBase
    {
        public DemoSettingsWithNamedProperty(string filePath) : base(filePath)
        {
        }

        [JsonPropertyName("foo")]
        public int IntProperty { get; set; }
    }
}