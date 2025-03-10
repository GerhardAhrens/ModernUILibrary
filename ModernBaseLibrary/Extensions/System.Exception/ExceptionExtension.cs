namespace ModernBaseLibrary.Extension
{
    using System;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Helper class for <see cref="Exception" />
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Format a single exception as a text string
        /// </summary>
        /// <param name="exception"></param>
        public static string GetText(this Exception exception)
        {
            var builder = new StringBuilder();
            builder.AppendLine(exception.GetType().ToString());

            if (!string.IsNullOrWhiteSpace(exception.Message))
            {
                builder.AppendLine();
                builder.AppendLine(exception.Message);
            }

            if (!string.IsNullOrWhiteSpace(exception.StackTrace))
            {
                builder.AppendLine();
                builder.AppendLine(exception.StackTrace);
            }

            if (exception.InnerException != null)
            {
                builder.AppendLine();
                builder.AppendLine(new string('-', 50));
                builder.AppendLine();
                builder.Append(exception.InnerException.GetText());
            }

            return builder.ToString();
        }

        /// <summary>
        /// Format all children of an <see cref="AggregateException"/> into a single string
        /// </summary>
        /// <param name="aggregateException"></param>
        public static string GetText(this AggregateException aggregateException)
        {
            return string.Join(
                "\r\n" + new string('=', 80) + "\r\n",
                aggregateException.InnerExceptions.Select(ex => ex.GetText()));
        }

        /// <summary>
        /// Formats for human.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>System.String.</returns>
        public static string FormatForHuman(this Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            var properties = exception.GetType().GetProperties();

            if (properties == null)
            {
                return string.Empty;
            }

            var fields = properties.Select(property => new
            {
                property.Name,
                Value = property.GetValue(exception, null)
            }).Select(x => $"{x.Name} = {x.Value?.ToString() ?? string.Empty}");

            return string.Join("\n", fields as string[]);
        }
    }
}
