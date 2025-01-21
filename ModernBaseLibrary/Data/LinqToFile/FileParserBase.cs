//-----------------------------------------------------------------------
// <copyright file="FileParserBase.cs" company="Lifeprojects.de">
//     Class: FileParserBase
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>08.12.2017</date>
//
// <summary>
// Base Class to Parse File
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.LinqToFile
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;

    public abstract class FileParserBase<T, M> : IFileParser<T> where T : new() where M : FileMetadata
    {
        private readonly List<M> metadata;

        protected FileParserBase(List<M> metadata)
        {
            this.metadata = metadata;
        }

        protected abstract bool End { get; }

        public virtual IEnumerator<T> GetEnumerator()
        {
            while (!this.End)
            {
                yield return this.Deserialize(this.ReadRow(), this.metadata);
            }
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public virtual void Dispose()
        {
        }

        protected virtual T Deserialize(string line, List<M> metadata)
        {
            T obj = new T();
            foreach (MetadataValue<M> metadataValue in this.Split(line, metadata))
            {
                metadataValue.FieldMetadata.PropertyInfo.SetValue(obj, this.ParseValue(metadataValue), null);
            }

            return obj;
        }

        protected abstract IEnumerable<MetadataValue<M>> Split(string line, List<M> metadata);

        protected object ParseValue(MetadataValue<M> metadataValue)
        {
            string item = metadataValue.Item.Trim();
            if (string.IsNullOrWhiteSpace(item))
            {
                return null;
            }

            M fieldMetadata = metadataValue.FieldMetadata;
            switch (fieldMetadata.Type)
            {
                case TypeCode.Boolean:
                    if (item == "0")
                    {
                        return false;
                    }

                    if (item == "1")
                    {
                        return true;
                    }

                    return bool.Parse(item);

                case TypeCode.Byte:
                    return byte.Parse(item);

                case TypeCode.Char:
                    return item[0];

                case TypeCode.DateTime:
                    return DateTime.ParseExact(item.Trim(), fieldMetadata.DateTimeFormat, CultureInfo.InvariantCulture);

                case TypeCode.Decimal:
                    return decimal.Parse(item.Replace(" ", string.Empty), fieldMetadata.NumberFormatProvider);

                case TypeCode.Double:
                    return double.Parse(item.Replace(" ", string.Empty), fieldMetadata.NumberFormatProvider);

                case TypeCode.Int16:
                    return short.Parse(item.Replace(" ", string.Empty), fieldMetadata.NumberFormatProvider);

                case TypeCode.Int32:
                    return int.Parse(item.Replace(" ", string.Empty), fieldMetadata.NumberFormatProvider);

                case TypeCode.Int64:
                    return long.Parse(item.Replace(" ", string.Empty), fieldMetadata.NumberFormatProvider);

                case TypeCode.SByte:
                    return sbyte.Parse(item.Replace(" ", string.Empty), fieldMetadata.NumberFormatProvider);

                case TypeCode.Single:
                    return float.Parse(item.Replace(" ", string.Empty), fieldMetadata.NumberFormatProvider);

                case TypeCode.String:
                    return item.TrimEnd();

                case TypeCode.UInt16:
                    return ushort.Parse(item.Replace(" ", string.Empty), fieldMetadata.NumberFormatProvider);

                case TypeCode.UInt32:
                    return uint.Parse(item.Replace(" ", string.Empty), fieldMetadata.NumberFormatProvider);

                case TypeCode.UInt64:
                    return ulong.Parse(item.Replace(" ", string.Empty), fieldMetadata.NumberFormatProvider);

                default:
                    return null;
            }
        }

        protected abstract string ReadRow();
    }
}