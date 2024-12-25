
namespace ModernBaseLibrary.Extension
{
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class EncodeExtensions
    {
        public static string EncodeBase64<T>(this T obj) =>
                Convert.ToBase64String(Encoding.ASCII.GetBytes(obj?.ToString() ?? throw new Exception("T Base64 is null.")));

        public static string DecodeBase64(this string base64) =>
            Encoding.UTF8.GetString(Convert.FromBase64String(base64 ?? throw new Exception("String Base64 is null.")));

        public static T DecodeBase64<T>(this string base64)
        {
            object obj = Encoding.UTF8.GetString(Convert.FromBase64String(base64.ToString() ?? throw new Exception("String Base64 is null.")));
            return (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture); ;
        }

        public static string EncodeBase64ByteArray(this byte[] obj) =>
        Convert.ToBase64String(obj);

        public static byte[] DecodeBase64ByteArray(this string base64) =>
            Convert.FromBase64String(base64 ?? throw new Exception("String Base64 is null."));

        public static bool IsBase64String(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this))
            {
                return false;
            }

            return (@this.Trim().Length % 4 == 0) && Regex.IsMatch(@this, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }
    }
}
