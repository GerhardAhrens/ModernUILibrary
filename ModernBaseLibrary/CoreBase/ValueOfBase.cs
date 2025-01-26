namespace ModernBaseLibrary.CoreBase
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public abstract class ValueOfBase<TValue, TThis> where TThis : ValueOfBase<TValue, TThis>, new()
    {
        private static readonly Func<TThis> Factory;

        /// <summary>
        /// WARNING - THIS FEATURE IS EXPERIMENTAL. I may change it to do
        /// validation in a different way.
        /// Right now, override this method, and throw any exceptions you need to.
        /// Access this.Value to check the value
        /// </summary>
        protected virtual void Validate()
        {
        }

        protected virtual bool TryValidate()
        {
            return true;
        }

        static ValueOfBase()
        {
            ConstructorInfo ctor = typeof(TThis)
                .GetTypeInfo()
                .DeclaredConstructors
                .First();

            var argsExp = new Expression[0];
            NewExpression newExp = Expression.New(ctor, argsExp);
            LambdaExpression lambda = Expression.Lambda(typeof(Func<TThis>), newExp);

            Factory = (Func<TThis>)lambda.Compile();
        }

        public TValue Value { get; protected set; }

        public static TThis From(TValue item)
        {
            TThis x = Factory();
            x.Value = item;
            x.Validate();

            return x;
        }

        public static bool TryFrom(TValue item, out TThis thisValue)
        {
            TThis x = Factory();
            x.Value = item;

            thisValue = x.TryValidate()
               ? x
               : null;

            return thisValue != null;
        }

        protected virtual bool Equals(ValueOfBase<TValue, TThis> other)
        {
            return EqualityComparer<TValue>.Default.Equals(this.Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            return obj.GetType() == GetType() && Equals((ValueOfBase<TValue, TThis>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TValue>.Default.GetHashCode(this.Value);
        }

        public static bool operator ==(ValueOfBase<TValue, TThis> a, ValueOfBase<TValue, TThis> b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ValueOfBase<TValue, TThis> a, ValueOfBase<TValue, TThis> b)
        {
            return !(a == b);
        }

        // Implicit operator removed. See issue #14.

        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Copies the content of public properties to the object passed by parameter.
        /// </summary>
        /// <param name="coptyTo">Object that will contain the copied data.</param>
        public void CopyTo(TThis coptyTo)
        {
            Type t = GetType();

            if (coptyTo == null)
            {
                Type tt = typeof(TValue);
                ConstructorInfo ctor = tt.GetConstructor(new[] { typeof(TValue) });
                coptyTo = (TThis)Activator.CreateInstance(typeof(TThis), ctor);
            }

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
        }
    }
}