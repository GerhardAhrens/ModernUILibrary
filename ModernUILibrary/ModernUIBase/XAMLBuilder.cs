/*
 * <copyright file="XAMLBuilder.cs" company="Lifeprojects.de">
 *     Class: XAMLBuilder
 *     Copyright © Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>21.02.2024 19:59:06</date>
 * <Project>CurrentProject</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernIU.Base
{
    using System.IO;
    using System.Text;
    using System.Windows.Markup;
    using System.Xml;

    public sealed class XAMLBuilder<TResult>
    {
        public static TResult GetStyle(string content)
        {
            return (TResult)LoadXaml<TResult>(content);
        }

        private static T LoadXaml<T>(string xamlString)
        {
            if (string.IsNullOrEmpty(xamlString) == false)
            {
                using (var stringReader = new StringReader(xamlString))
                {
                    using (var xmlReader = XmlReader.Create(stringReader))
                    {
                        return (T)XamlReader.Load(xmlReader);
                    }
                }
            }
            else
            {
                return default(T);
            }
        }
    }

    public sealed class StyleText
    {
        private StringBuilder text = null;

        public StyleText()
        {
            this.text = new StringBuilder();
        }

        public string Value
        { 
            get
            { 
                return this.text.ToString();
            } 
        }


        public StyleText Add(string tagetType, StringBuilder content)
        {
            text.AppendLine("<Style").Append(" ");
            text.AppendLine($"xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"").Append(" ");
            text.AppendLine($"xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"").Append(" ");
            text.AppendLine($"xmlns:PresentationOptions=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation/options\"").Append(" ");
            text.AppendLine($"xmlns:sys=\"clr-namespace:System;assembly=mscorlib\"").Append(" ");
            text.AppendLine($"TargetType=\"{tagetType}\">").Append(" ");
            text.Insert(text.Length - 1, content);
            text.AppendLine("</Style>");
            return this;
        }

        public StyleText Add(string tagetType, string content)
        {
            text.AppendLine("<Style").Append(" ");
            text.AppendLine($"xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"").Append(" ");
            text.AppendLine($"xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"").Append(" ");
            text.AppendLine($"xmlns:PresentationOptions=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation/options\"").Append(" ");
            text.AppendLine($"xmlns:sys=\"clr-namespace:System;assembly=mscorlib\"").Append(" ");
            text.AppendLine($"TargetType=\"{tagetType}\">").Append(" ");
            text.Insert(text.Length - 1, content);
            text.AppendLine("</Style>");
            return this;
        }
    }
}
