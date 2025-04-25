namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    using global::ModernBaseLibrary.Extension;

    [DebuggerStepThrough]
    [DebuggerNonUserCode]
    public static partial class AssertExtension
    {
        public static void IsDateToday(this Assert assert, DateTime today)
        {
            if (today.Date != DateTime.Now.Date)
            {
                throw new AssertFailedException($"IsDateToday: today {today} != {DateTime.Now.Date}");
            }
        }

        public static bool IsEmpty<T>(this Assert @this, IEnumerable<T> data)
        {
            return data != null && data.Any();
        }

        public static bool AreEqualValue<TTyp>(this Assert @this,TTyp object1, TTyp object2)
        {
            Type type = typeof(TTyp);

            if (object1 == null || object2 == null)
            {
                return false;
            }

            foreach (PropertyInfo property in type.GetProperties())
            {
                if (property.Name != "ExtensionData")
                {
                    string object1Value = string.Empty;
                    string object2Value = string.Empty;

                    if (type.GetProperty(property.Name).GetValue(object1, null) != null)
                    {
                        object1Value = type.GetProperty(property.Name).GetValue(object1, null).ToString();
                    }

                    if (type.GetProperty(property.Name).GetValue(object2, null) != null)
                    {
                        object2Value = type.GetProperty(property.Name).GetValue(object2, null).ToString();
                    }

                    if (object1Value.Trim() != object2Value.Trim())
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void CountGreaterZero<TTyp>(this Assert @this, object input) where TTyp:class
        {
            bool result = false;
            string typeAsText = string.Empty;

            if (input == null)
            {
                return;
            }

            Type testedTyp = input.GetType();

            if (input is IEnumerable<TTyp>)
            {
                typeAsText = input.GetType().ToFriendlyName();
                var collection = input as IEnumerable<TTyp>;
                if (collection.Count() > 0)
                {
                    result = true;
                }
            }
            else if (testedTyp.IsArray == true)
            {
                typeAsText = input.GetType().ToFriendlyName();
                TTyp res = input as TTyp;
                if (res != null)
                {

                }
            }

            if (result == true)
            {
                return;
            }

            throw new AssertFailedException($"{typeAsText }<{typeof(TTyp).Name}> == 0");
        }

        public static void CountGreaterZero<TTyp>(this Assert @this, TTyp[] input) where TTyp : class
        {
            bool result = false;
            string typeAsText = string.Empty;

            if (input == null)
            {
                return;
            }

            Type testedTyp = input.GetType();

            if (testedTyp.IsArray == true)
            {
                typeAsText = input.GetType().ToFriendlyName();
                TTyp[] res = input as TTyp[];
                if (res != null && res.Length > 0)
                {
                    result = true;
                }
            }

            if (result == true)
            {
                return;
            }

            throw new AssertFailedException($"{typeAsText }<{typeof(TTyp).Name}> == 0");
        }

        public static void CountGreaterZero<TTyp>(this Assert @this, TTyp[,] input) where TTyp : class
        {
            bool result = false;
            string typeAsText = string.Empty;

            if (input == null)
            {
                return;
            }

            Type testedTyp = input.GetType();

            if (testedTyp.IsArray == true)
            {
                typeAsText = input.GetType().ToFriendlyName();
                TTyp[,] res = input as TTyp[,];
                if (res != null && res.Length > 0)
                {
                    result = true;
                }
            }

            if (result == true)
            {
                return;
            }

            throw new AssertFailedException($"{typeAsText }<{typeof(TTyp).Name}> == 0");
        }

        public static void CountGreaterZero<TTyp1, TTyp2>(this Assert @this, object input)
        {
            bool result = false;
            string typeAsText = string.Empty;

            if (input == null)
            {
                return;
            }

            Type testedTyp = input.GetType();

            if (testedTyp.IsGenericType && testedTyp.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                typeAsText = input.GetType().ToFriendlyName();
                var collection = input as Dictionary<TTyp1, TTyp2>;
                if (collection.Count() > 0)
                {
                    result = true;
                }
            }

            if (result == true)
            {
                return;
            }

            throw new AssertFailedException($"{typeAsText } == 0");
        }

        public static void DateEquals(this Assert @this, DateTime expected, DateTime actual)
        {
            if (expected.Equals(actual) == true)
            {
                return;
            }

            throw new AssertFailedException(GetMessage(expected.ToShortDateString(), actual.ToShortDateString()));
        }

        public static void DateEquals(this Assert @this, DateTime? expected, DateTime? actual)
        {
            if (expected == null || actual == null)
            {
                return;
            }

            if (expected.Equals(actual) == true)
            {
                return;
            }

            throw new AssertFailedException(GetMessage(((DateTime)expected).ToShortDateString(), ((DateTime)actual).ToShortDateString()));
        }

        public static void StringEquals(this Assert @this, string expected, string actual)
        {
            if (expected.Equals(actual) == true)
            {
                return;
            }

            throw new AssertFailedException(GetMessage(expected, actual));
        }

        public static void StringContains(this Assert @this, string expected, string actual)
        {
            if (expected.Contains(actual) == true)
            {
                return;
            }

            throw new AssertFailedException(GetMessage(expected, actual));
        }

        public static void StringNotEquals(this Assert @this, string expected, string actual)
        {
            if (expected.Equals(actual) == false)
            {
                return;
            }

            throw new AssertFailedException(GetMessage(expected, actual));
        }

        private static string GetMessage(string expected, string actual)
        {
            var expectedFormat = ReplaceInvisibleCharacters(expected);
            var actualFormat = ReplaceInvisibleCharacters(actual);

            // Get the index of the first different character
            var index = expectedFormat.Zip(actualFormat, (c1, c2) => c1 == c2).TakeWhile(b => b).Count();
            var caret = new string(' ', index) + "^";

            return $@"Strings are differents.
                        Expect: <{expectedFormat}>
                        Actual: <{actualFormat}>
                                 {caret}";
        }

        private static string ReplaceInvisibleCharacters(string value)
        {
            return value
                .Replace(' ', '·')
                .Replace('\t', '→')
                .Replace("\r", "\\r")
                .Replace("\n", "\\n");
        }
    }
}