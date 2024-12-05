//-----------------------------------------------------------------------
// <copyright file="StringHash.cs" company="Lifeprojects.de">
//     Class: StringHash
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>25.05.2023</date>
//
// <summary>
// Implementierung Passwortgenerator
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    using ModernBaseLibrary.Core;

    [SupportedOSPlatform("windows")]
    public class PasswordGenerator : DisposableCoreBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordGenerator"/> class.
        /// </summary>
        public PasswordGenerator()
        {
            char[] alphabetUpper = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();
            this.GetLetterUpper = new string(alphabetUpper);

            char[] alphabetLower = Enumerable.Range('a', 26).Select(x => (char)x).ToArray();
            this.GetLetterLower = new string(alphabetLower);

            char[] numbers = Enumerable.Range('0', 10).Select(x => (char)x).ToArray();
            this.GetNumbers = new string(numbers);

            this.GetSpecialChars = "*$-+?_&=!%{}/~#<>.";
        }

        public int PwdCount { get; set; } = 10;

        public int PasswordLength { get; set; } = 10;

        public string GetLetterUpper { get; private set; }

        public string GetLetterLower { get; private set; }

        public string GetNumbers { get; private set; }

        public string GetSpecialChars { get; private set; }

        public string Create(int letterTyp, int passwordLength, bool setNumbers, bool setSpecialChars)
        {
            string pwdResult = string.Empty;

            try
            {
                StringBuilder pwdChars = new StringBuilder(passwordLength);

                if (letterTyp == 0)
                {
                    if (setNumbers == true)
                    {
                        pwdResult = this.GetRandomString(passwordLength, this.GetNumbers);
                    }
                }
                else if (letterTyp == 1)
                {
                    pwdChars.Append(this.GetLetterUpper).Append(this.GetLetterLower);
                    if (setNumbers == true)
                    {
                        pwdChars.Append(this.GetNumbers);
                    }

                    if (setSpecialChars == true)
                    {
                        pwdChars.Append(this.GetSpecialChars);
                    }

                    pwdResult = this.GetRandomString(passwordLength, this.GetNumbers);
                }
                else if (letterTyp == 2)
                {
                    pwdChars.Append(this.GetLetterLower);
                    if (setNumbers == true)
                    {
                        pwdChars.Append(this.GetNumbers);
                    }

                    if (setSpecialChars == true)
                    {
                        pwdChars.Append(this.GetSpecialChars);
                    }

                    pwdResult = this.GetRandomString(passwordLength, this.GetNumbers);
                }
                else if (letterTyp == 3)
                {
                    pwdChars.Append(this.GetLetterUpper);
                    if (setNumbers == true)
                    {
                        pwdChars.Append(this.GetNumbers);
                    }

                    if (setSpecialChars == true)
                    {
                        pwdChars.Append(this.GetSpecialChars);
                    }

                    pwdResult = this.GetRandomString(passwordLength, this.GetNumbers);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return pwdResult;
        }

        public string CreatePwdNumbers(int passwordLength = 10)
        {
            string pwdResult = string.Empty;
            try
            {
                pwdResult = Create(0, passwordLength, true, false);
            }
            catch (Exception)
            {
                throw;
            }

            return pwdResult;
        }

        public IEnumerable<string> CreatePwdCollection(int letterTyp, int passwordLength, bool setNumbers, bool setSpecialChars)
        {
            List<string> result= new List<string>();

            try
            {
                StringBuilder pwdChars = new StringBuilder(passwordLength);

                if (letterTyp == 0)
                {
                    if (setNumbers == true)
                    {
                        for (int i = 0; i < this.PwdCount; i++)
                        {
                            string pwdResult = this.GetRandomString(passwordLength, this.GetNumbers);
                            result.Add(pwdResult);
                        }
                    }
                }
                else if (letterTyp == 1)
                {
                    pwdChars.Append(this.GetLetterUpper).Append(this.GetLetterLower);
                    if (setNumbers == true)
                    {
                        pwdChars.Append(this.GetNumbers);
                    }

                    if (setSpecialChars == true)
                    {
                        pwdChars.Append(this.GetSpecialChars);
                    }

                    for (int i = 0; i < this.PwdCount; i++)
                    {
                        string pwdResult = this.GetRandomString(passwordLength, pwdChars.ToString());
                        result.Add(pwdResult);
                    }
                }
                else if (letterTyp == 2)
                {
                    pwdChars.Append(this.GetLetterLower);
                    if (setNumbers == true)
                    {
                        pwdChars.Append(this.GetNumbers);
                    }

                    if (setSpecialChars == true)
                    {
                        pwdChars.Append(this.GetSpecialChars);
                    }

                    for (int i = 0; i < this.PwdCount; i++)
                    {
                        string pwdResult = this.GetRandomString(passwordLength, pwdChars.ToString());
                        result.Add(pwdResult);
                    }
                }
                else if (letterTyp == 3)
                {
                    pwdChars.Append(this.GetLetterUpper);
                    if (setNumbers == true)
                    {
                        pwdChars.Append(this.GetNumbers);
                    }

                    if (setSpecialChars == true)
                    {
                        pwdChars.Append(this.GetSpecialChars);
                    }

                    for (int i = 0; i < this.PwdCount; i++)
                    {
                        string pwdResult = this.GetRandomString(passwordLength, pwdChars.ToString());
                        result.Add(pwdResult);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static int PasswordStrength(string password, int minSize = 8)
        {
            if (string.IsNullOrEmpty(password) == true)
            {
                return 0;
            }

            string lowerCaseRegex = "(?=.*[a-z])";
            string upperCaseRegex = "(?=.*[A-Z])";
            string symbolsRegex = "(?=.*[*$-+?_&=!%{}/~#<>.])";
            string numericRegex = "(?=.*[0-9])";

            int passwordStrengthResult = 0;
            var contains = new List<PasswordStrengthContain>();
            if (new Regex($"^{lowerCaseRegex}").IsMatch(password))
            {
                contains.Add(new PasswordStrengthContain { Message = "lowercase" });
            }
            if (new Regex($"^{upperCaseRegex}").IsMatch(password))
            {
                contains.Add(new PasswordStrengthContain { Message = "uppercase" });
            }
            if (new Regex($"^{symbolsRegex}").IsMatch(password))
            {
                contains.Add(new PasswordStrengthContain { Message = "symbol" });
            }
            if (new Regex($"^{numericRegex}").IsMatch(password))
            {
                contains.Add(new PasswordStrengthContain { Message = "number" });
            }

            CultureInfo cultureInfo = CultureInfo.CurrentCulture;

            if (new Regex(@"^" + lowerCaseRegex + upperCaseRegex + numericRegex + symbolsRegex + "(?=.{8,})").IsMatch(password))
            {
                passwordStrengthResult = 3;
            }
            else if (new Regex(@"^((" + lowerCaseRegex + upperCaseRegex + ")|(" + lowerCaseRegex + numericRegex + ")|(" + upperCaseRegex + numericRegex + ")|(" + upperCaseRegex + symbolsRegex + ")|(" + lowerCaseRegex + symbolsRegex + ")|(" + numericRegex + symbolsRegex + "))(?=.{6,})").IsMatch(password))
            {
                passwordStrengthResult = 2;
            }
            else
            {
                passwordStrengthResult = 1;
            }

            return passwordStrengthResult;
        }

        public PasswordStrengthResult PasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password) == true)
            {
                return null;
            }


            string lowerCaseRegex = "(?=.*[a-z])";
            string upperCaseRegex = "(?=.*[A-Z])";
            string symbolsRegex = "(?=.*[*$-+?_&=!%{}/~#<>.])";
            string numericRegex = "(?=.*[0-9])";

            PasswordStrengthResult passwordStrengthResult = new PasswordStrengthResult();
            var contains = new List<PasswordStrengthContain>();
            if (new Regex($"^{lowerCaseRegex}").IsMatch(password))
            {
                contains.Add(new PasswordStrengthContain { Message = "lowercase" });
            }
            if (new Regex($"^{upperCaseRegex}").IsMatch(password))
            {
                contains.Add(new PasswordStrengthContain { Message = "uppercase" });
            }
            if (new Regex($"^{symbolsRegex}").IsMatch(password))
            {
                contains.Add(new PasswordStrengthContain { Message = "symbol" });
            }
            if (new Regex($"^{numericRegex}").IsMatch(password))
            {
                contains.Add(new PasswordStrengthContain { Message = "number" });
            }

            CultureInfo cultureInfo= CultureInfo.CurrentCulture;

            if (new Regex(@"^" + lowerCaseRegex + upperCaseRegex + numericRegex + symbolsRegex + "(?=.{8,})").IsMatch(password))
            {
                passwordStrengthResult.Id = 2;
                if (cultureInfo.Name == "de-DE")
                {
                    passwordStrengthResult.Value = "Stark";
                }
                else if (cultureInfo.Name == "en-US")
                {
                    passwordStrengthResult.Value = "Strong";
                }
                else
                {
                    passwordStrengthResult.Value = "Strong";
                }
            }
            else if (new Regex(@"^((" + lowerCaseRegex + upperCaseRegex + ")|(" + lowerCaseRegex + numericRegex + ")|(" + upperCaseRegex + numericRegex + ")|(" + upperCaseRegex + symbolsRegex + ")|(" + lowerCaseRegex + symbolsRegex + ")|(" + numericRegex + symbolsRegex + "))(?=.{6,})").IsMatch(password))
            {
                passwordStrengthResult.Id = 1;
                if (cultureInfo.Name == "de-DE")
                {
                    passwordStrengthResult.Value = "Mittel";
                }
                else if (cultureInfo.Name == "en-US")
                {
                    passwordStrengthResult.Value = "Medium";
                }
                else
                {
                    passwordStrengthResult.Value = "Medium";
                }
            }
            else
            {
                passwordStrengthResult.Id = 0;
                if (cultureInfo.Name == "de-DE")
                {
                    passwordStrengthResult.Value = "Schwach";
                }
                else if (cultureInfo.Name == "en-US")
                {
                    passwordStrengthResult.Value = "Weak";
                }
                else
                {
                    passwordStrengthResult.Value = "Weak";
                }
            }

            passwordStrengthResult.Contains = contains;
            passwordStrengthResult.Length = password.Length;
            return passwordStrengthResult;
        }

        protected override void DisposeManagedResources()
        {
            /* Behandeln von Managed Resources bem verlassen der Klasse */
        }

        protected override void DisposeUnmanagedResources()
        {
            /* Behandeln von UnManaged Resources bem verlassen der Klasse */
        }

        private string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            if (length < 0)
            {
                throw new ArgumentException("length must not be negative", "length");
            }

            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
            {
                throw new ArgumentException("length is too big", "length");
            }

            if (characterSet == null)
            {
                throw new ArgumentNullException("characterSet");
            }

            var characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
            {
                throw new ArgumentException("characterSet must not be empty", "characterSet");
            }

            var bytes = new byte[length * 8];
#pragma warning disable SYSLIB0023 // Typ oder Element ist veraltet
            new RNGCryptoServiceProvider().GetBytes(bytes);
#pragma warning restore SYSLIB0023 // Typ oder Element ist veraltet
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }

            return new string(result);
        }
    }

    public class PasswordStrengthResult
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public int Length { get; set; }

        public IList<PasswordStrengthContain> Contains { get; set; }
    }

    public class PasswordStrengthContain
    {
        public string Message { get; set; }
    }
}
