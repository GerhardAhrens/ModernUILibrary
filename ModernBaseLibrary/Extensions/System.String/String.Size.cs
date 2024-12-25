namespace ModernBaseLibrary.Extension
{
    using System.Text;

    public static partial class StringExtension
    {
        /// <summary>
        /// Calculates the amount of bytes occupied by the input string
        /// </summary>
        /// <param name="this">The input string to check</param>
        /// <returns>The total size of the input string in bytes</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        public static int Size(this string @this)
        {
            // preconditions
            if (@this == null)
            {
                throw new ArgumentNullException("input");
            }

            // simple implementation for utf16 which is the default encoding where chars are of a fixed size
            int result = @this.Length * sizeof(char);

            return result;
        }

        /// <summary>
        /// Calculates the amount of bytes occupied by the input string encoded as the encoding specified
        /// </summary>
        /// <param name="this">The input string to check</param>
        /// <param name="encoding">The encoding to use</param>
        /// <returns>The total size of the input string in bytes</returns>
        /// <exception cref="System.ArgumentNullException">input is null</exception>
        /// <exception cref="System.ArgumentNullException">encoding is null</exception>
        public static int Size(this string @this, Encoding encoding)
        {
            if (@this == null)
            {
                throw new ArgumentNullException("input");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            return encoding.GetByteCount(@this);
        }
    }
}
