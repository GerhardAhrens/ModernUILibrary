//-----------------------------------------------------------------------
// <copyright file="TextBoxInputMode.cs" company="Lifeprojects.de">
//     Class: TextBoxInputMode
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>09.06.2020</date>
//
// <summary>Definition of Enum Class for TextBoxInput Behavior</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Behaviors
{
    public enum TextBoxInputMode
    {
        None,
        DecimalInput,
        DigitInput,
        PercentInput,
        Date,
        CurrencyInput,
        LetterOrDigit,
        Letter
    }
}
