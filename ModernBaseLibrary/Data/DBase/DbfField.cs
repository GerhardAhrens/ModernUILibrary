﻿namespace ModernBaseLibrary.DBase
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Encapsulates a field descriptor in a .dbf file.
    /// </summary>
    public class DbfField : IEquatable<DbfField>
    {
        private string defaultValue;

        /// <summary>
        /// Field name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Field type
        /// </summary>
        public DbfFieldType Type { get; set; }

        /// <summary>
        /// Length of field in bytes
        /// </summary>
        public byte Length { get; set; }

        public byte Precision { get; set; }

        public byte WorkAreaID { get; set; }

        public byte Flags { get; set; }

        /// <summary>
        /// Default value to write.
        /// </summary>
        public string DefaultValue => defaultValue ?? (defaultValue = new string(' ', Length));

        public DbfField(string name, DbfFieldType type, byte length, byte precision = 0)
        {
            this.Name = name;
            this.Type = type;
            this.Length = length;
            this.Precision = precision;
            this.WorkAreaID = 0;
            this.Flags = 0;
        }

        /// <inheritdoc />
        public bool Equals(DbfField other)
        {
            return other != null
                   && this.Name == other.Name
                   && this.Type == other.Type
                   && this.Length == other.Length
                   && this.Precision == other.Precision
                   && this.WorkAreaID == other.WorkAreaID
                   && this.Flags == other.Flags;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is DbfField other && Equals(other);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Name = `{this.Name}`, Type = `{(char)this.Type}`, Length = `{this.Length}`, Precision = `{this.Precision}`";
        }

        internal DbfField(BinaryReader reader, Encoding encoding)
        {
            // Some field name maybe like `NUM\0\0?B\0\0\0\0`, so we should split by `\0` instead of end trimming.
            string rawName = encoding.GetString(reader.ReadBytes(11));
            Name = rawName.Split((char)0)[0];
            Type = (DbfFieldType)reader.ReadByte();
            reader.ReadBytes(4); // reserved: Field data address in memory.
            Length = reader.ReadByte();
            Precision = reader.ReadByte();
            reader.ReadBytes(2); // reserved.
            WorkAreaID = reader.ReadByte();
            reader.ReadBytes(2); // reserved.
            Flags = reader.ReadByte();
            reader.ReadBytes(8);
        }

        internal void Write(BinaryWriter writer, Encoding encoding)
        {
            // Pad field name with 0-bytes, then save it.
            string name = this.Name;

            // consider multibyte should truncate or padding after GetBytes (11 bytes)
            byte[] nameBytes = encoding.GetBytes(name);
            if (nameBytes.Length < 11)
            {
                writer.Write(nameBytes);

                var padlen = 11 - nameBytes.Length;
                writer.Write(new byte[padlen]);
            }
            else
            {
                writer.Write(nameBytes, 0, 11);
            }

            writer.Write((char)Type);
            writer.Write((uint)0); // 4 reserved bytes: Field data address in memory.
            writer.Write(Length);
            writer.Write(Precision);
            writer.Write((ushort)0); // 2 reserved byte.
            writer.Write(WorkAreaID);
            writer.Write((ushort)0); // 2 reserved byte.
            writer.Write(Flags);

            for (int i = 0; i < 8; i++) writer.Write((byte)0); // 8 reserved bytes.
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + this.Name.GetHashCode();
            hash = hash * 31 + this.Type.GetHashCode();
            hash = hash * 31 + this.Length.GetHashCode();
            hash = hash * 31 + this.Precision.GetHashCode();
            hash = hash * 31 + this.WorkAreaID.GetHashCode();
            hash = hash * 31 + this.Flags.GetHashCode();
            return hash;
        }
    }
}
