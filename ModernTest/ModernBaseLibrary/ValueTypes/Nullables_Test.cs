/*
 * <copyright file="Nullables_Test.cs" company="Lifeprojects.de">
 *     Class: Nullables_Test
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>02.03.2025 20:37:02</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
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

namespace ModernTest.ModernBaseLibrary.ValueTypes
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.ValueTypes;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Nullables_Test
    {
        private Word Hello => new("Hello");
        private Word There => new("there");
        private Word Friend => new("friend.");

        private readonly EqualityComparer<Statement> _comparer = EqualityComparer<Statement>.Default;

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Nullables_Test"/> class.
        /// </summary>
        public Nullables_Test()
        {
        }

        [TestMethod]
        public void SameStatements_WithEmptyWordGroups_AreEqual()
        {
            var statement1 = new Statement(new[] { Hello, There }, new Word[] { });
            var statement2 = new Statement(new[] { Hello, There }, new Word[] { });

            Assert.AreEqual(statement1, statement2);
            Assert.IsTrue(_comparer.Equals(statement1, statement2));
            Assert.IsTrue(statement1.Equals(statement2));
            Assert.IsTrue(statement2.Equals(statement1));
            Assert.IsTrue(statement1 == statement2);
            Assert.IsTrue(statement2 == statement1);
            Assert.IsFalse(statement1 != statement2);
            Assert.IsFalse(statement2 != statement1);
        }

        [TestMethod]
        public void SameStatements_InSameOrder_AreEqual()
        {
            var statement1 = new Statement(new[] { Hello, There }, new[] { Friend });
            var statement2 = new Statement(new[] { Hello, There }, new[] { Friend });

            Assert.AreEqual(statement1, statement2);
            Assert.IsTrue(_comparer.Equals(statement1, statement2));
            Assert.IsTrue(statement1.Equals(statement2));
            Assert.IsTrue(statement2.Equals(statement1));
            Assert.IsTrue(statement1 == statement2);
            Assert.IsTrue(statement2 == statement1);
            Assert.IsFalse(statement1 != statement2);
            Assert.IsFalse(statement2 != statement1);
        }

        [TestMethod]
        public void MultipleStatements_WithSameOrdering_AreDifferent()
        {
            var statement1 = new Statement(new[] { Hello, There }, new[] { Friend });
            var statement2 = new Statement(new[] { Hello }, new[] { There, Friend });

            Assert.AreNotEqual(statement1, statement2);
            Assert.IsFalse(_comparer.Equals(statement1, statement2));
            Assert.IsFalse(statement1.Equals(statement2));
            Assert.IsFalse(statement2.Equals(statement1));
            Assert.IsFalse(statement1 == statement2);
            Assert.IsFalse(statement2 == statement1);
            Assert.IsTrue(statement1 != statement2);
            Assert.IsTrue(statement2 != statement1);
        }

        [TestMethod]
        public void DifferentStatements_AreDifferent()
        {
            var statement1 = new Statement(new[] { Hello, There }, new Word[] { });
            var statement2 = new Statement(new[] { Hello, Friend }, new Word[] { });

            Assert.AreNotEqual(statement1, statement2);
            Assert.IsFalse(_comparer.Equals(statement1, statement2));
            Assert.IsFalse(statement1.Equals(statement2));
            Assert.IsFalse(statement2.Equals(statement1));
            Assert.IsFalse(statement1 == statement2);
            Assert.IsFalse(statement2 == statement1);
            Assert.IsTrue(statement1 != statement2);
            Assert.IsTrue(statement2 != statement1);
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }
    }

    class Word : Value
    {
        private readonly string _word;
        public Word(string word) => _word = word;
        protected override IEnumerable<ValueBase> GetValues() => Yield(_word);
    }

    class Statement : Value
    {
        private readonly Word[] _first;
        private readonly Word[] _second;

        public Statement(Word[] first, Word[] second) => (_first, _second) = (first, second);

        protected override IEnumerable<ValueBase> GetValues() => Yield(_first.AsValues(), _second.AsValues());
    }
}
