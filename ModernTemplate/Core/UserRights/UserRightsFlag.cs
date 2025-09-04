//-----------------------------------------------------------------------
// <copyright file="RightsFlag.cs" company="Lifeprojects.de">
//     Class: FlagService
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>22.07.2025 12:52:06</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Reflection;

    using ModernBaseLibrary.Extension;

    public enum UserRightsFlag : int
    {
        None = 0,
        [Description("Lesen")]
        Read = 1,
        [Description("Schreiben")]
        Write = 2,
        [Description("Exportieren von Daten")]
        Export = 3,
        [Description("Drucken von Daten")]
        Print = 4,
        [Description("Administrator Funktionen")]
        Administrator = 5
    }

    [DebuggerDisplay("Key={this.Key};Value={this.Value}")]

    public class UserRightsItem 
    {
        public UserRightsItem(int key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public UserRightsItem(Enum keyValue)
        {
            this.Key = (int)(object)keyValue;
            this.Value = keyValue.ToString();
        }

        public int Key { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"Key={this.Key};Value={this.Value}";
        }
    }

    public class EnumToIEnumerable
    {
        public static IEnumerable<UserRightsItem> Get<TEnum>()
        {
            Type type = typeof(TEnum);
            if (type.IsEnum)
            {
                List<UserRightsItem> result = new List<UserRightsItem>();
                Array values = Enum.GetValues(type);
                foreach (var value in values)
                {
                    if ((int)value > 0 || value.ToString().ToLower() != "none")
                    {
                        TEnum enumvalue = (TEnum)Enum.Parse(typeof(TEnum), value.ToString(), true);
                        string valueText = GetDescription(enumvalue);
                        result.Add(new UserRightsItem((int)value, valueText));
                    }
                }

                return result;
            }
            else
            {
                throw new ArgumentException($"Der übergebene Typ '{type.Name}' muß ein Enum sein");
            }
        }

        private static string GetDescription<TEnum>(TEnum enumKey)
        {
            Type type = enumKey.GetType();
            if (type.IsEnum == false)
            {
                throw new ArgumentException($"Der Typ '{type.Name}' muss vom Typ Enum sein", "enumKey");
            }

            MemberInfo[] memberInfo = type.GetMember(enumKey.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] descAttribute = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (descAttribute != null && descAttribute.Length > 0)
                {
                    return ((DescriptionAttribute)descAttribute[0]).Description;
                }
            }

            return enumKey.ToString();
        }
    }
}
