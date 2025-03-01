namespace ModernBaseLibrary.Core
{
    public abstract class BinaryTextEncoding
    {
        public abstract string Encode(byte[] bytes);

        public abstract byte[] Decode(string input);
    }
}