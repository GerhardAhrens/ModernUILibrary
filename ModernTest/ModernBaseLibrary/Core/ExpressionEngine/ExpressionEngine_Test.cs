/*
 * <copyright file="ExpressionEngine_Test.cs" company="Lifeprojects.de">
 *     Class: ExpressionEngine_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>17.10.2022 20:08:00</date>
 * <Project>Git-Projekt</Project>
 * <FrameworkVersion>6.0</FrameworkVersion>
 *
 * <summary>
 * Unit-Test Klasse für
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernTest.ModernBaseLibrary.ExpressionEngine
{
    using System;
    using System.IO;

    using global::ModernBaseLibrary.ExpressionEngine;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExpressionEngine_Test
    {
        [TestMethod]
        public void TokenizerTest()
        {
            var testString = "10 + 20 - 30.123";
            var t = new Tokenizer(new StringReader(testString));

            // "10"
            Assert.AreEqual(t.Token, Token.Number);
            Assert.AreEqual(t.Number, 10);
            t.NextToken();

            // "+"
            Assert.AreEqual(t.Token, Token.Add);
            t.NextToken();

            // "20"
            Assert.AreEqual(t.Token, Token.Number);
            Assert.AreEqual(t.Number, 20);
            t.NextToken();

            // "-"
            Assert.AreEqual(t.Token, Token.Subtract);
            t.NextToken();

            // "30.123"
            Assert.AreEqual(t.Token, Token.Number);
            Assert.AreEqual(t.Number, 30.123M);
            t.NextToken();
        }

        [TestMethod]
        public void TokenizerWithParenthesesTest()
        {
            var testString = "(10 + 20) - 30.123";
            var t = new Tokenizer(new StringReader(testString));

            // (
            Assert.AreEqual(t.Token, Token.OpenParens);
            t.NextToken();

            // "10"
            Assert.AreEqual(t.Token, Token.Number);
            Assert.AreEqual(t.Number, 10);
            t.NextToken();

            // "+"
            Assert.AreEqual(t.Token, Token.Add);
            t.NextToken();

            // "20"
            Assert.AreEqual(t.Token, Token.Number);
            Assert.AreEqual(t.Number, 20);
            t.NextToken();

            // )
            Assert.AreEqual(t.Token, Token.CloseParens);
            t.NextToken();

            // "-"
            Assert.AreEqual(t.Token, Token.Subtract);
            t.NextToken();

            // "30.123"
            Assert.AreEqual(t.Token, Token.Number);
            Assert.AreEqual(t.Number.ToString(), 30.123.ToString());
            t.NextToken();

            Assert.AreEqual(t.Token, Token.EOF);
            t.NextToken();
        }

        [TestMethod]
        public void AddSubtractTest()
        {
            // Add 
            Assert.AreEqual(Parser.Parse("10 + 20").Eval(), 30);

            // Subtract 
            Assert.AreEqual(Parser.Parse("10 - 20").Eval(), -10);

            // Sequence
            Assert.AreEqual(Parser.Parse("10 + 20 - 40 + 100").Eval(), 90);
        }

        [TestMethod]
        public void UnaryTest()
        {
            // Negative
            Assert.AreEqual(Parser.Parse("-10").Eval(), -10);

            // Positive
            Assert.AreEqual(Parser.Parse("+10").Eval(), 10);

            // Negative of a negative
            Assert.AreEqual(Parser.Parse("--10").Eval(), 10);

            // Woah!
            Assert.AreEqual(Parser.Parse("--++-+-10").Eval(), 10);

            // All together now
            Assert.AreEqual(Parser.Parse("10 + -20 - +30").Eval(), -40);
        }

        [TestMethod]
        public void MultiplyDivideTest()
        {
            // Add 
            Assert.AreEqual(Parser.Parse("10 * 20").Eval(), 200);

            // Subtract 
            Assert.AreEqual(Parser.Parse("10 / 20").Eval(), 0.5M);

            // Sequence
            Assert.AreEqual(Parser.Parse("10 * 20 / 50").Eval(), 4);
        }

        [TestMethod]
        public void OrderOfOperation()
        {
            // No parens
            Assert.AreEqual(Parser.Parse("10 + 20 * 30").Eval(), 610);

            // Parens
            Assert.AreEqual(Parser.Parse("(10 + 20) * 30").Eval(), 900);

            // Parens and negative
            Assert.AreEqual(Parser.Parse("-(10 + 20) * 30").Eval(), -900);

            // Nested
            Assert.AreEqual(Parser.Parse("-((10 + 20) * 5) * 30").Eval(), -4500);
        }

        private class CircleCalculation : ICalculationContext
        {
            public CircleCalculation(decimal r)
            {
                _r = r;
            }

            decimal _r;

            public decimal ResolveVariable(string name)
            {
                switch (name)
                {
                    case "pi": return (decimal)Math.PI;
                    case "r": return _r;
                }

                throw new InvalidDataException($"Unknown variable: '{name}'");
            }

            public decimal CallFunction(string name, decimal[] arguments)
            {
                return 0;
            }
        }

        [TestMethod]
        public void Variables1()
        {
            var ctx = new CircleCalculation(10);

            var result = Parser.Parse("2 * pi * r").Eval(ctx);

            Assert.AreEqual(result, 2 * (decimal)Math.PI * 10);
        }

        private class SqrtCalculation : ICalculationContext
        {
            public decimal ResolveVariable(string name)
            {
                throw new InvalidDataException($"Unknown variable: '{name}'");
            }

            public decimal CallFunction(string name, decimal[] arguments)
            {
                if (name.ToLower() == "sqrt")
                {
                    return (decimal)Math.Sqrt((double)arguments[0]);
                }

                return 1;
            }
        }

        [TestMethod]
        public void Variables2()
        {
            var ctx = new SqrtCalculation();

            var result = Parser.Parse("1 + sqrt(2)").Eval(ctx);

            Assert.AreEqual(result, 1 + (decimal)Math.Sqrt(2));
        }

        private class CustomFunctionContext : ICalculationContext
        {
            public decimal ResolveVariable(string name)
            {
                throw new InvalidDataException($"Unknown variable: '{name}'");
            }

            public decimal CallFunction(string name, decimal[] arguments)
            {
                if (name == "rectArea")
                {
                    return arguments[0] * arguments[1];
                }

                if (name == "rectPerimeter")
                {
                    return (arguments[0] + arguments[1]) * 2;
                }

                throw new InvalidDataException($"Unknown function: '{name}'");
            }
        }

        [TestMethod]
        public void Functions()
        {
            var ctx = new CustomFunctionContext();
            Assert.AreEqual(Parser.Parse("rectArea(10,20)").Eval(ctx), 200);
            Assert.AreEqual(Parser.Parse("rectPerimeter(10,20)").Eval(ctx), 60);
        }

        private class MathLibrary
        {
            public MathLibrary()
            {
                pi = (decimal)Math.PI;
            }

            public decimal pi { get; private set; }
            public decimal r { get; set; }

            public decimal rectArea(decimal width, decimal height)
            {
                return width * height;
            }

            public decimal rectPerimeter(decimal width, decimal height)
            {
                return (width + height) * 2;
            }
            public decimal SQRT(decimal value)
            {
                return (decimal)Math.Sqrt((double)value);
            }
        }

        [TestMethod]
        public void ReflectioWithLibrary1()
        {
            // Create a library of helper function
            var lib = new MathLibrary();
            lib.r = 10;

            // Create a context that uses the library
            var ctx = new ReflectionContext(lib);

            // Test
            Assert.AreEqual(Parser.Parse("1^100").Eval(), 1^100);
            Assert.AreEqual(Parser.Parse("10 + 20 * 30").Eval(), 610);
            Assert.AreEqual(Parser.Parse("rectArea(10,20)").Eval(ctx), 200);
            Assert.AreEqual(Parser.Parse("rectPerimeter(10,20)").Eval(ctx), 60);
            Assert.AreEqual(Parser.Parse("2 * pi * r").Eval(ctx), 2 * (decimal)Math.PI * 10);
            Assert.AreEqual(Parser.Parse("1 + sqrt(2)").Eval(ctx), 1+ (decimal)Math.Sqrt(2));
        }

        private class FinanceLibrary
        {
            public FinanceLibrary(decimal tax = 19)
            {
                this.Tax = tax;
            }

            public decimal Tax { get; set; }

            public decimal ToNetto(decimal valueBrutto)
            {
                decimal taxValue = TruncatePrecisions(valueBrutto / (1 + (decimal)this.Tax / 100) * (decimal)this.Tax / 100,2);
                return valueBrutto - taxValue;
            }

            public decimal ToBrutto(decimal valueNetto)
            {
                decimal taxValue = TruncatePrecisions(valueNetto * (decimal)this.Tax / 100,2);
                return valueNetto + taxValue;
            }

            private decimal TruncatePrecisions(decimal @this, int precision)
            {
                if (precision < 0)
                {
                    throw new ArgumentException($"Number of decimal places ({precision} is invalid!)");
                }

                if (precision < 0)
                {
                    return @this;
                }

                var multiplied = @this * (decimal)(Math.Pow(10, precision));

                decimal skippedValue;
                if (@this >= 0)
                {
                    skippedValue = Math.Floor(multiplied);
                }
                else
                {
                    skippedValue = Math.Ceiling(multiplied);
                }

                return skippedValue / (decimal)(Math.Pow(10, precision));
            }
        }

        [TestMethod]
        public void ReflectioWithLibrary2()
        {
            // Create a library of helper function
            var lib = new FinanceLibrary();
            lib.Tax = 19;

            // Create a context that uses the library
            var ctx = new ReflectionContext(lib);

            // Test
            Assert.AreEqual(Parser.Parse("ToNetto(100)").Eval(ctx).ToString(), 84.04.ToString());
            Assert.AreEqual(Parser.Parse("ToBrutto(84.04)").Eval(ctx).ToString(), 100.ToString());
        }
    }
}