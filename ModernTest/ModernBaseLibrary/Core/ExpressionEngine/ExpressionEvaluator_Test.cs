/*
 * <copyright file="ExpressionEvaluator_Test.cs" company="Lifeprojects.de">
 *     Class: ExpressionEvaluator_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>15.10.2022 20:08:00</date>
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
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.ExpressionEngine;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExpressionEvaluator_Test
    {
        private ExpressionEvaluator engine;
        private Random generator;

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            engine = new ExpressionEvaluator();
            generator = new Random();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionEvaluator_Test"/> class.
        /// </summary>
        public ExpressionEvaluator_Test()
        {
        }

        [TestMethod]
        public void Empty_String_Is_Zero()
        {
            Assert.AreEqual(engine.Evaluate(string.Empty), 0);
        }

        [TestMethod]
        public void Decimal_Is_Treated_As_Decimal()
        {
            var left = generator.Next(1, 100);

            Assert.AreEqual(engine.Evaluate(left.ToString()), left);
        }

        [TestMethod]
        public void Two_Plus_Two_Is_Four()
        {
            Assert.AreEqual(engine.Evaluate("2+2"), 4);
        }

        [TestMethod]
        public void Can_Add_Two_Decimal_Numbers()
        {
            Assert.AreEqual(engine.Evaluate("2.7+3.2"), 2.7m + 3.2m);
        }

        [TestMethod]
        public void Can_Add_Many_Numbers()
        {
            Assert.AreEqual(engine.Evaluate("1.2+3.4+5.6+7.8"), 1.2m + 3.4m + 5.6m + 7.8m);
            Assert.AreEqual(engine.Evaluate("1.7+2.9+14.24+6.58"), 1.7m + 2.9m + 14.24m + 6.58m);
        }

        [TestMethod]
        public void Can_Subtract_Two_Numbers()
        {
            Assert.AreEqual(engine.Evaluate("5-2"), 5 - 2);
        }

        [TestMethod]
        public void Can_Subtract_Multiple_Numbers()
        {
            Assert.AreEqual(engine.Evaluate("15.2-2.3-4.8-0.58"), 15.2m - 2.3m - 4.8m - 0.58m);
        }

        [TestMethod]
        public void Can_Add_And_Subtract_Multiple_Numbers()
        {
            Assert.AreEqual(engine.Evaluate("15+8-4-2+7"), 15 + 8 - 4 - 2 + 7);
            Assert.AreEqual(engine.Evaluate("17.89-2.47+7.16"), 17.89m - 2.47m + 7.16m);

        }

        [TestMethod]
        public void Can_Add_Subtract_Multiply_Divide_Multiple_Numbers()
        {
            Assert.AreEqual(engine.Evaluate("50-5*3*2+7"), 50 - 5 * 3 * 2 + 7);
            Assert.AreEqual(engine.Evaluate("84+15+4-4*3*9+24+4-54/3-5-7+47"), 84 + 15 + 4 - 4 * 3 * 9 + 24 + 4 - 54 / 3 - 5 - 7 + 47);
            Assert.AreEqual(engine.Evaluate("50-48/4/3+7*2*4+2+5+8"), 50 - 48 / 4 / 3 + 7 * 2 * 4 + 2 + 5 + 8);
            Assert.AreEqual(engine.Evaluate("5/2/2+1.5*3+4.58"), 5 / 2m / 2m + 1.5m * 3m + 4.58m);
            Assert.AreEqual(engine.Evaluate("25/3+1.34*2.56+1.49+2.36/1.48"), 25 / 3m + 1.34m * 2.56m + 1.49m + 2.36m / 1.48m);
            Assert.AreEqual(engine.Evaluate("2*3+5-4-2*5+7"), 2 * 3 + 5 - 4 - 2 * 5 + 7);
        }

        [TestMethod]
        public void Supports_Parentheses()
        {
            Assert.AreEqual(engine.Evaluate("2*(5+3)"), 2 * (5 + 3));
            Assert.AreEqual(engine.Evaluate("(5+3)*2"), (5 + 3) * 2);
            Assert.AreEqual(engine.Evaluate("(5+3)*5-2"), (5 + 3) * 5 - 2);
            Assert.AreEqual(engine.Evaluate("(5+3)*(5-2)"), (5 + 3) * (5 - 2));
            Assert.AreEqual(engine.Evaluate("((5+3)*3-(8-2)/2)/2"), (((5 + 3) * 3 - (8 - 2) / 2) / 2m));
            Assert.AreEqual(engine.Evaluate("(4*(3+5)-4-8/2-(6-4)/2)*((2+4)*4-(8-5)/3)-5"), (4 * (3 + 5) - 4 - 8 / 2 - (6 - 4) / 2) * ((2 + 4) * 4 - (8 - 5) / 3) - 5);
            Assert.AreEqual(engine.Evaluate("(((9-6/2)*2-4)/2-6-1)/(2+24/(2+4))"), (((9 - 6 / 2) * 2 - 4) / 2m - 6 - 1) / (2 + 24 / (2 + 4)));
        }

        [TestMethod]
        public void Can_Process_Simple_Variables()
        {
            decimal a = 2.6m;
            decimal b = 5.7m;

            Assert.AreEqual(engine.Evaluate("a", new { a }), a);
            Assert.AreEqual(engine.Evaluate("a+a", new { a }), (a + a));
            Assert.AreEqual(engine.Evaluate("a+b", new { a, b }),( a + b));
        }

        [TestMethod]
        public void Can_Process_Multiple_Variables()
        {
            var a = 6;
            var b = 4.5m;
            var c = 2.6m;
            Assert.AreEqual(engine.Evaluate("(((9-a/2)*2-b)/2-a-1)/(2+c/(2+4))", new { a, b, c }), (((9 - a / 2) * 2 - b) / 2 - a - 1) / (2 + c / (2 + 4)));
            Assert.AreEqual(engine.Evaluate("(c+b)*a", new { a, b, c }), (c + b) * a);
        }

        [TestMethod]
        public void Can_Pass_Named_Variables()
        {
            dynamic dynamicEngine = new ExpressionEvaluator();

            var a = 6;
            var b = 4.5m;
            var c = 2.6m;

            Assert.AreEqual(dynamicEngine.Evaluate("(c+b)*a", a: 6, b: 4.5, c: 2.6), (c + b) * a);
        }

        [TestMethod]
        public void Can_Invoke_Expression_Multiple_Times()
        {
            var a = 6m;
            var b = 3.9m;
            var c = 4.9m;

            var compiled = engine.Compile("(a+b)/(a+c)");
            Assert.AreEqual(compiled(new { a, b, c }), (a + b) / (a + c));

            a = 5.4m;
            b = -2.4m;
            c = 7.5m;

            Assert.AreEqual(compiled(new { a, b, c }), (a + b) / (a + c));
        }

        [TestMethod]
        public void NegativeNumber_Parsed_AsNegativeNumber()
        {
            var left = -generator.Next(1, 100);

            Assert.AreEqual(engine.Evaluate(left.ToString()), left);
        }

        [TestMethod]
        public void Can_EvaluateExpression_WithNegativeNumbers()
        {
            Assert.AreEqual(engine.Evaluate("-5 + 3"), -5 + 3);
            Assert.AreEqual(engine.Evaluate("5 + (-3)"), 5 + (-3));

            Assert.AreEqual(engine.Evaluate("5 + (4-3)"), 5 + (4 - 3));
            Assert.AreEqual(engine.Evaluate("5 + (-(4-3))"), 5 + (-(4 - 3)));
        }

        [TestMethod]
        public void Can_ParseNumbers_InDifferentCulture()
        {
            var clone = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            clone.NumberFormat.NumberDecimalSeparator = ",";
            clone.NumberFormat.NumberGroupSeparator = " ";

            var engineWithCulture = new ExpressionEvaluator(clone);

            Assert.AreEqual(engineWithCulture.Evaluate("5,67"), 5.67M);
            Assert.AreEqual(engineWithCulture.Evaluate("5 000"), 5000);

            Assert.AreEqual(engineWithCulture.Evaluate("5 000,67"), 5000.67M);
        }

        [TestMethod]
        public void Can_Invoke_Two_Distinct_Expressions_With_Different_Parameters_Count()
        {
            var a = 6m;
            var b = 3.9m;
            var c = 4.9m;

            var compiled = engine.Compile("(a+b)/(a+c)");
            var compiled2 = engine.Compile("(a+b)/a");
            Assert.AreEqual(compiled(new { a, b, c }), (a + b) / (a + c));

            a = 5.4m;
            b = -2.4m;

            Assert.AreEqual(compiled2(new { a, b }), (a + b) / a);
        }

        [TestMethod]
        public void CalcWithConstantValue()
        {
            Assert.AreEqual(engine.Evaluate("2+{pi}"), 4);
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void CalcNumberAndLetterForException()
        {
            try
            {
                Assert.AreEqual(engine.Evaluate("2+test"), 4);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentException));
            }
        }
    }
}
