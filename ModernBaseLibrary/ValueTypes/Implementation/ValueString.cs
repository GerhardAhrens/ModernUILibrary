namespace ModernBaseLibrary.ValueTypes
{
    using System.Diagnostics.CodeAnalysis;

    public sealed class ValueString : ValueBase
    {
        public string Content { get; }
        public ValueString(string content) => Content = content;

        public override bool Equals([AllowNull] ValueBase other)
        {
            if (other is null) return false;
            if (other is ValueString value) return Content?.Equals(value.Content) ?? Content is null && value.Content is null;
            return false;
        }

        public override int GetHashCode() => Content?.GetHashCode() ?? 0;

        public override string ToString() => $"Value(\"{Content}\")";
    }
}
