//-----------------------------------------------------------------------
// <copyright file="ValidationMessageFormatter.cs" company="Lifeprojects.de">
//     Class: ValidationMessageFormatter
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>21.07.2020</date>
//
// <summary>
//      Result Class for FluentValidation Implementierung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using ModernBaseLibrary.Core;

    public class ValidationMessageFormatter : DisposableCoreBase
    {
        private readonly Regex keyRegex = new Regex("{([^{}:]+)(?::([^{}]+))?}", RegexOptions.Compiled);
        private readonly Dictionary<string, object> messageArgument = null;

        public ValidationMessageFormatter()
        {
            messageArgument = new Dictionary<string, object>(2);
        }

        public object PropertyName { get; set; }

        public object PropertyValue { get; set; }

        public Type PropertyType { get; set; }

        public string RawMessage { get; set; }

        public int CountArgument
        {
            get { return messageArgument.Count; }
        }

        public void AppendArgument(string name, object value)
        {
            messageArgument[name] = value;
        }

        public override string ToString()
        {
            string tempMessage = ReplacePlaceholdersWithValues(this.RawMessage, this.messageArgument);

            return tempMessage;
        }

        private string ReplacePlaceholdersWithValues(string template, IDictionary<string, object> values)
        {
            return keyRegex.Replace(template, m =>
            {
                var key = m.Groups[1].Value;

                if (values.ContainsKey(key) == false)
                {
                    return m.Value;
                }

                var format = m.Groups[2].Success ? $"{{0:{m.Groups[2].Value}}}" : null;

                return format == null ? values[key]?.ToString() : string.Format(format, values[key]);
            });
        }
    }
}
