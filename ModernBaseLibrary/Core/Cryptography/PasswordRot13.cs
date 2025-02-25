namespace ModernBaseLibrary.Cryptography
{
    using System.Collections.Generic;

    using ModernBaseLibrary.Core;

    public class PasswordRot13
    {
        /// <summary>Rot13 encode the password, for simple security.</summary>
        /// <param name="password">The password to encode.</param>
        /// <returns>The encoded password.</returns>
        public static string HashPassword(string password)
        {
            password.IsArgumentNullOrEmpty("password");

            List<char> retString = new List<char>();

            for (int i = 0; i < password.Length; i++)
            {
                if (password[i] <= 'Z' && password[i] >= 'A')
                {
                    retString.Add((char)(((int)password[i] - 'A' + 13) % 26 + 'A'));
                }
                else if (password[i] <= 'z' && password[i] >= 'a')
                {
                    retString.Add((char)(((int)password[i] - 'a' + 13) % 26 + 'a'));
                }
                else
                {
                    retString.Add(password[i]);
                }
            }

            return new string(retString.ToArray());
        }

        /// <summary>Rot13 decode the password, for simple security.</summary>
        /// <param name="password">The password to decode.</param>
        /// <returns>The decoded password.</returns>
        public static string UnHashPassword(string password)
        {
            password.IsArgumentNullOrEmpty("password");

            if (string.IsNullOrEmpty(password) == false)
            {
                List<char> retString = new List<char>();

                for (int i = 0; i < password.Length; i++)
                {
                    char newLet = (char)(password[i] - 13);
                    if (password[i] >= 'A' && password[i] <= 'Z')
                    {
                        if (newLet < 'A') newLet = (char)(newLet + 26);
                    }
                    else if (password[i] >= 'a' && password[i] <= 'z')
                    {
                        if (newLet < 'a') newLet = (char)(newLet + 26);
                    }
                    else
                    {
                        newLet = password[i];
                    }

                    retString.Add(newLet);
                }

                return new string(retString.ToArray());
            }
            else
            {
                return string.Empty;
            }

        }
    }
}
