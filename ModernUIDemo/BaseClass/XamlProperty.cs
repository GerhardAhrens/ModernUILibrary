//-----------------------------------------------------------------------
// <copyright file="XamlProperty.cs" company="Lifeprojects.de">
//     Class: XamlProperty
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>06.11.2023 15:21:45</date>
//
// <summary>
// Klasse zum Binding von C# Properties in XAML Code
// </summary>
//-----------------------------------------------------------------------

namespace ModernUIDemo.Core
{
    using System;
    using System.Runtime.CompilerServices;

    public static class XamlProperty
    {
        public static XamlProperty<T> Set<T>(Action<T, string> actionMethod, T value = default, [CallerMemberName] string propertyName = "")
        {
            var property = new XamlProperty<T>(default(T), actionMethod, propertyName);
            property.Value = value;
            return property;
        }

        public static XamlProperty<T> Set<T>(Action<T> actionMethod, T value = default, [CallerMemberName] string propertyName = "")
        {
            var property = new XamlProperty<T>(default(T), actionMethod, propertyName);
            property.Value = value;
            return property;
        }

        public static XamlProperty<T> Set<T>(T value = default, [CallerMemberName] string propertyName = "")
        {
            var property = new XamlProperty<T>(default(T), propertyName);
            property.Value = value;
            return property;
        }

        internal static XamlProperty<T1> Set<T1, T2>()
        {
            throw new NotImplementedException();
        }
    }

    public class XamlProperty<T> : XamlPropertyBase
    {
        private T value;

        public XamlProperty(T value, Action<T, string> actionMethod, [CallerMemberName] string propertyName = "")
        {
            this.ActionMethodB = actionMethod;
            this.value = value;
            this.Name = propertyName;

            if (actionMethod != null)
            {
                actionMethod(value, propertyName);
            }
        }

        public XamlProperty(T value, Action<T> actionMethod, [CallerMemberName] string propertyName = "")
        {
            this.ActionMethodA = actionMethod;
            this.value = value;
            this.Name = propertyName;
        }

        public XamlProperty(T value, [CallerMemberName] string propertyName = "")
        {
            this.value = value;
            this.Name = propertyName;
        }

        public XamlProperty([CallerMemberName] string propertyName = "")
        {
            this.value = default(T);
            this.Name = propertyName;
        }

        public T Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.SetProperty(ref this.value, value);

                if (this.value != null)
                {
                    if (this.ActionMethodA != null)
                    {
                        this.ActionMethodA(value);
                    }

                    if (this.ActionMethodB != null)
                    {
                        this.ActionMethodB(value, this.Name);
                    }
                }
            }
        }

        public string Name { get; private set; }

        private Action<T> ActionMethodA { get; set; }

        private Action<T, string> ActionMethodB { get; set; }

        public static XamlProperty<T> Set(T value, [CallerMemberName] string propertyName = "")
        {
            return new XamlProperty<T>(value, propertyName);
        }

        public static XamlProperty<T> Set([CallerMemberName] string propertyName = "")
        {
            return new XamlProperty<T>(default(T), propertyName);
        }
    }
}
