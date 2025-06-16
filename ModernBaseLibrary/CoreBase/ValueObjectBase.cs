namespace ModernBaseLibrary.CoreBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ModernBaseLibrary.Core;

    public abstract class ValueObjectBase : IEquatable<ValueObjectBase>
    {
        private List<PropertyInfo> properties;
        private List<FieldInfo> fields;

        public static bool operator ==(ValueObjectBase left, ValueObjectBase right)
        {
            if (object.Equals(left, null))
            {
                if (object.Equals(right, null))
                {
                    return true;
                }
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(ValueObjectBase left, ValueObjectBase right)
        {
            return !(left == right);
        }

        public bool Equals(ValueObjectBase value)
        {
            return Equals(value as object);
        }

        public override bool Equals(object value)
        {
            if (value == null || GetType() != value.GetType()) return false;
            
            return GetProperties().All(p => PropertiesAreEqual(value, p))
                && GetFields().All(f => FieldsAreEqual(value, f));
        }

        public TType CopyTo<TType>()
        {
            Type t = GetType();
            object coptyTo = null;
            Type tt = typeof(TType);
            ConstructorInfo ctor = tt.GetConstructor(new[] { typeof(TType) });
            coptyTo = (TType)Activator.CreateInstance(typeof(TType), ctor);

            PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo property in properties)
            {
                var valueToCopy = property.GetValue(this, null);
                if (valueToCopy == null)
                {
                    continue;
                }

                if (property.CanWrite)
                {
                    property.SetValue(coptyTo, valueToCopy, null);
                }
            }

            return (TType)coptyTo;
        }

        private bool PropertiesAreEqual(object value, PropertyInfo p)
        {       
            return object.Equals(p.GetValue(this, null), p.GetValue(value, null));
        }

        private bool FieldsAreEqual(object value, FieldInfo f)
        {
            return object.Equals(f.GetValue(this), f.GetValue(value));
        }

        private IEnumerable<PropertyInfo> GetProperties()
        {
            if (this.properties == null)
            {
                this.properties = GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => !Attribute.IsDefined(p, typeof(IgnoreMemberAttribute))).ToList();
            }

            return this.properties;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            if (this.fields == null)
            {
                this.fields = GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .Where(f => !Attribute.IsDefined(f, typeof(IgnoreMemberAttribute))).ToList();
            }

            return this.fields;
        }

        public override int GetHashCode()
        {
            unchecked   //allow overflow
            {
                int hash = 17;
                foreach (var prop in GetProperties())
                {   
                    var value = prop.GetValue(this, null);
                    hash = HashValue(hash, value);
                }

                foreach(var field in GetFields())
                {
                    var value = field.GetValue(this);
                    hash = HashValue(hash, value);
                }

                return hash;
            }
        }

        private int HashValue(int seed, object value)
        {
            var currentHash = value != null ? value.GetHashCode() : 0;

            return seed * 23 + currentHash;
        }
    }
}