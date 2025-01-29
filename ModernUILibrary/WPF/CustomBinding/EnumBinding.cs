//-----------------------------------------------------------------------
// <copyright file="EnumBinding.cs" company="Lifeprojects.de">
//     Class: EnumBinding
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>07.09.2020</date>
//
// <summary>
// Die Klasse stellt ein eigenes WPF Binding (für Enums) zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.WPF.Base
{ 
    using System;

    public class EnumBinding : BindingDecoratorBase
    {
        private Type enumType;
        public Type EnumType
        {
            get { return this.enumType; }
            set
            {
                if (value != this.enumType)
                {
                    if (null != value)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;

                        if (enumType.IsEnum == false)
                        {
                            throw new ArgumentException("Type must be for an Enum.");
                        }
                    }

                    this.enumType = value;
                }
            }
        }

        public EnumBinding() { }

        public EnumBinding(Type enumType)
        {
            this.EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == this.enumType)
            {
                throw new InvalidOperationException("The EnumType must be specified.");
            }

            Type actualEnumType = Nullable.GetUnderlyingType(this.enumType) ?? this.enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == this.enumType)
            {
                return enumValues;
            }

            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}
