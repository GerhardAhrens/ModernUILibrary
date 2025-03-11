namespace ModernBaseLibrary.Network
{
    using System.Management;
    using System.Text;

    public class PhysicalMemory : BaseWin32Entity
    {
        public PhysicalMemory(ManagementBaseObject obj) : base(obj)
        {
            this.Value = ParseValue<long>(obj, "Capacity") / 1073741824;

        }

        public long Value { get; private set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Gefundener RAM: {this.Value} GB");
            sb.AppendLine(PrintProperties());


            return sb.ToString();
        }
    }
}