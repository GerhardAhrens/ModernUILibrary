namespace ModernInsideVM.Core
{
    using System;
    using System.Linq.Expressions;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Core.LINQ;
    using ModernBaseLibrary.Extension;

    public class InputValidation<TViewModel> where TViewModel : class
    {
        private static InputValidation<TViewModel> validation;

        private TViewModel ThisObject { get; set; }

        public static InputValidation<TViewModel> This(TViewModel thisObject)
        {
            validation = new InputValidation<TViewModel>();
            validation.ThisObject = thisObject;
            return validation;
        }

        public Result<string> NotEmpty(Expression<Func<TViewModel, object>> expression, string fieldName)
        {
            string result = string.Empty;
            bool resultValidError = false;
            string propertyName = ExpressionPropertyName.For<TViewModel>(expression);
            string propertyValue = (string)validation.ThisObject.GetType().GetProperty(propertyName).GetValue(validation.ThisObject);

            if (string.IsNullOrEmpty(propertyValue) == true)
            {
                result = $"Das Feld '{fieldName}' darf nicht leer sein.";
                resultValidError = true;
            }

            return Result<string>.SuccessResult(result, resultValidError);
        }

        public Result<string> NotEmptyAndMinChar(Expression<Func<TViewModel, object>> expression, string fieldName, int minChar = 5)
        {
            string result = string.Empty;
            bool resultValidError = false;
            string propertyName = ExpressionPropertyName.For<TViewModel>(expression);
            string propertyValue = (string)validation.ThisObject.GetType().GetProperty(propertyName).GetValue(validation.ThisObject);

            if (string.IsNullOrEmpty(propertyValue) == true)
            {
                result = $"Das Feld '{fieldName}' darf nicht leer sein.";
                resultValidError = true;
            }
            else
            {
                if (propertyValue.Length < minChar)
                {
                    result = $"Das Feld '{fieldName}' muß mind. {minChar} Zeichen lang sein.";
                    resultValidError = true;
                }
            }

            return Result<string>.SuccessResult(result, resultValidError);
        }

        public Result<string> InRange(Expression<Func<TViewModel, object>> expression, int min, int max, string fieldName)
        {
            string result = string.Empty;
            bool resultValidError = false;
            string propertyName = ExpressionPropertyName.For<TViewModel>(expression);
            object propertyValue = (object)validation.ThisObject.GetType().GetProperty(propertyName).GetValue(validation.ThisObject, null);

            if (propertyValue == null)
            {
                return Result<string>.SuccessResult(result, resultValidError);
            }

            if (propertyValue.IsEmpty() == false)
            {
                if (propertyValue.ToInt().InRange(min, max) == false)
                {
                    result = $"Das Feld '{fieldName}' muß zwischen {min} und {max} liegen";
                    resultValidError = true;
                }
            }
            else
            {
                result = $"Das Feld '{fieldName}' darf nicht leer sein.";
                resultValidError = true;
            }

            return Result<string>.SuccessResult(result, resultValidError);
        }

        public Result<string> GreaterThanZero(Expression<Func<TViewModel, object>> expression, string fieldName)
        {
            string result = string.Empty;
            bool resultValidError = false;
            string propertyName = ExpressionPropertyName.For<TViewModel>(expression);
            object propertyValue = (object)validation.ThisObject.GetType().GetProperty(propertyName).GetValue(validation.ThisObject, null);

            if (propertyValue == null)
            {
                return Result<string>.SuccessResult(result, resultValidError);
            }

            if (propertyValue.IsEmpty() == false)
            {
                double testDouble;
                if (double.TryParse(propertyValue.ToString(), out testDouble) == true)
                {
                    if (testDouble <= 0)
                    {
                        result = $"Der Feld '{fieldName}' muß gößer 0 sein";
                        resultValidError = true;
                    }
                }
                else
                {
                    result = $"Das Feld '{fieldName}' nicht leer sein.";
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