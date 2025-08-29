//-----------------------------------------------------------------------
// <copyright file="InputValidation.cs" company="company">
//     Class: InputValidation
//     Copyright © company 2025
// </copyright>
//
// <author>Gerhard Ahrens - company</author>
// <email>gerhard.ahrens@company.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Die Klasse beinhaltet verschiedene Methoden um eine Eingabe zu prüfen.
// Über ein Result-Objekt wird der Fehler zurückgegeben. Die Klasse ist speziell auf ein DataRow ausgelegt.
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System;
    using System.Data;
    using System.Globalization;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Extension;

    public class InputValidation<TDataRow> where TDataRow : class
    {
        private static InputValidation<DataRow> validation;

        private DataRow ThisObject { get; set; }

        public static InputValidation<DataRow> This(DataRow thisObject)
        {
            validation = new InputValidation<DataRow>();
            validation.ThisObject = thisObject;
            return validation;
        }

        /// <summary>
        /// Prüft ob der Inhalt leer ist
        /// </summary>
        /// <param name="fieldName">Column im DataRow</param>
        /// <param name="displayName">Anzeigename des Feldes</param>
        /// <returns><Result-Object/returns>
        public Result<string> NotEmpty(string fieldName, string displayName = "")
        {
            string result = string.Empty;
            bool resultValidError = false;
            string propertyValue = (string)((DataRow)validation.ThisObject).GetAs<string>(fieldName);

            displayName = string.IsNullOrEmpty(displayName) == true ? fieldName : displayName;

            if (string.IsNullOrEmpty(propertyValue) == true)
            {
                result = $"Das Feld '{displayName}' darf nicht leer sein.";
                resultValidError = true;
            }

            return Result<string>.SuccessResult(result, resultValidError);
        }

        /// <summary>
        /// Prüft um ein Integer innerhalb eines festgelegten Berecihes liegt.
        /// </summary>
        /// <param name="fieldName">Column im DataRow</param>
        /// <param name="min">Kleinster Wert</param>
        /// <param name="max">Größter Wert</param>
        /// <param name="displayName">Anzeigename des Feldes</param>
        /// <returns><Result-Object/returns>
        public Result<string> InRange(string fieldName, int min, int max, string displayName = "")
        {
            string result = string.Empty;
            bool resultValidError = false;
            object propertyValue = (int)((DataRow)validation.ThisObject).GetAs<int>(fieldName);

            if (propertyValue == null)
            {
                return Result<string>.SuccessResult(result, resultValidError);
            }

            displayName = string.IsNullOrEmpty(displayName) == true ? fieldName : displayName;

            if (string.IsNullOrEmpty(propertyValue.ToString()) == false)
            {
                if ((Enumerable.Range(min, max).Contains(Convert.ToInt32(propertyValue, CultureInfo.CurrentCulture))) == false)
                {
                    result = $"Das Feld '{displayName}' muß zwischen {min} und {max} liegen";
                    resultValidError = true;
                }
            }
            else
            {
                result = $"Das Feld '{displayName}' darf nicht leer sein.";
                resultValidError = true;
            }

            return Result<string>.SuccessResult(result, resultValidError);
        }

        /// <summary>
        /// Prüft ob der Wert größer 0 ist
        /// </summary>
        /// <param name="fieldName">Column im DataRow</param>
        /// <param name="displayName">Anzeigename des Feldes</param>
        /// <returns><Result-Object/returns>
        public Result<string> GreaterThanZero(string fieldName, string displayName = "")
        {
            string result = string.Empty;
            bool resultValidError = false;
            object propertyValue = (double)((DataRow)validation.ThisObject).GetAs<double>(fieldName);

            if (propertyValue == null)
            {
                return Result<string>.SuccessResult(result, resultValidError);
            }

            displayName = string.IsNullOrEmpty(displayName) == true ? fieldName : displayName;

            if (string.IsNullOrEmpty(propertyValue.ToString()) == false)
            {
                double testDouble;
                if (double.TryParse(propertyValue.ToString(), out testDouble) == true)
                {
                    if (testDouble <= 0)
                    {
                        result = $"Der Feld '{displayName}' muß größer 0 sein";
                        resultValidError = true;
                    }
                }
                else
                {
                    result = $"Das Feld '{displayName}' nicht leer sein.";
                    resultValidError = true;
                }
            }
            else
            {
                result = $"Das Feld nicht leer sein.";
                resultValidError = true;
            }

            return Result<string>.SuccessResult(result, resultValidError);
        }

        /// <summary>
        /// Prüft ob ein Wert größer oder gleich 0 ist
        /// </summary>
        /// <param name="fieldName">Column im DataRow</param>
        /// <param name="displayName">Anzeigename des Feldes</param>
        /// <returns><Result-Object/returns>
        public Result<string> GreaterOrZero(string fieldName, string displayName = "")
        {
            string result = string.Empty;
            bool resultValidError = false;
            object propertyValue = (double)((DataRow)validation.ThisObject).GetAs<double>(fieldName);

            if (propertyValue == null)
            {
                return Result<string>.SuccessResult(result, resultValidError);
            }

            displayName = string.IsNullOrEmpty(displayName) == true ? fieldName : displayName;

            if (string.IsNullOrEmpty(propertyValue.ToString()) == false)
            {
                double testDouble;
                if (double.TryParse(propertyValue.ToString(), out testDouble) == true)
                {
                    if (testDouble <= -1)
                    {
                        result = $"Der Feld '{displayName}' muß größer oder gleich 0 sein";
                        resultValidError = true;
                    }
                }
                else
                {
                    result = $"Das Feld '{displayName}' nicht leer sein.";
                    resultValidError = true;
                }
            }
            else
            {
                result = $"Das Feld nicht leer sein.";
                resultValidError = true;
            }

            return Result<string>.SuccessResult(result, resultValidError);
        }
    }
}