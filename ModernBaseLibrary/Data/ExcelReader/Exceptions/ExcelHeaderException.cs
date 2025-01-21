
namespace ModernBaseLibrary.ExcelReader
{
    using System;

    public class ExcelHeaderException : Exception
    {
        public ExcelHeaderException()
        {
        }

        public ExcelHeaderException(string message)
            : base(message)
        {
        }

        public ExcelHeaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
