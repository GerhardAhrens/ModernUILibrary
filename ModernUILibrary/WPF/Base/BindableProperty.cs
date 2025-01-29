//-----------------------------------------------------------------------
// <copyright file="BindableProperty.cs" company="Lifeprojects.de">
//     Class: BindableProperty
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>06.11.2023 15:21:45</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.WPF.Base
{
    using System;
    using System.Runtime.CompilerServices;

    public static class BindableProperty
    {
        public static BindableProperty<T> Set<T>(Action<T, string> actionMethod, T value = default, [CallerMemberName] string propertyName = "")
        {
            var property = new BindableProperty<T>(default(T), actionMethod, propertyName);
            property.Value = value;
            return property;
        }

        public static BindableProperty<T> Set<T>(Action<T> actionMethod, T value = default, [CallerMemberName] string propertyName = "")
        {
            var property = new BindableProperty<T>(default(T), actionMethod, propertyName);
            property.Value = value;
            return property;
        }

        public static BindableProperty<T> Set<T>(T value = default, [CallerMemberName] string propertyName = "")
        {
            var property = new BindableProperty<T>(default(T), propertyName);
            property.Value = value;
            return property;
        }
    }

    public class BindableProperty<T> : BindableBase
    {
        private T value;

        public BindableProperty(T value, Action<T, string> actionMethod, [CallerMemberName] string propertyName = "")
        {
            this.ActionMethodB = actionMethod;
            this.value = value;
            this.Name = propertyName;

            if (actionMethod != null)
            {
                actionMethod(value, propertyName);
            }
        }

        public BindableProperty(T value, Action<T> actionMethod, [CallerMemberName] string propertyName = "")
        {
            this.ActionMethodA = actionMethod;
            this.value = value;
            this.Name = propertyName;
        }

        public BindableProperty(T value, [CallerMemberName] string propertyName = "")
        {
            this.value = value;
            this.Name = propertyName;
        }

        public BindableProperty([CallerMemberName] string propertyName = "")
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
                        this.ActionMethodB(value,this.Name);
                    }
                }
            }
        }

        public string Name { get; private set; }

        private Action<T> ActionMethodA { get; set; }

        private Action<T,string> ActionMethodB { get; set; }

        public static BindableProperty<T> Set(T value, [CallerMemberName] string propertyName = "")
        {
            return new BindableProperty<T>(value, propertyName);
        }

        public static BindableProperty<T> Set([CallerMemberName] string propertyName = "")
        {
            return new BindableProperty<T>(default(T), propertyName);
        }
    }
}
