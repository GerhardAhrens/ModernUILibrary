/*
 * <copyright file="Model_Test.cs" company="Lifeprojects.de">
 *     Class: Model_Test
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>25.04.2025 11:00:57</date>
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

namespace ModernTest.ModernInsideVM
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernUILibrary.MVVM.Base;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Model_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Model_Test"/> class.
        /// </summary>
        public Model_Test()
        {
        }

        [TestMethod]
        public void CreateModel()
        {
            Contact contact = new Contact("Gerhard","Ahrens");
            contact.Birthday = new DateTime(1960, 6, 28);
            contact.Status = true;

            Assert.IsNotNull(contact);
            Assert.That.StringEquals(contact.FirstName, "Gerhard");
            Assert.That.StringEquals(contact.LastName, "Ahrens");
            Assert.That.DateEquals(contact.Birthday, new DateTime(1960,6,28));
            Assert.AreEqual(contact.Status, true);
        }

        [TestMethod]
        public void CloneModel()
        {
            Contact contact = new Contact("Gerhard", "Ahrens");
            contact.Birthday = new DateTime(1960, 6, 28);
            contact.Status = true;
            contact.Phones = new Dictionary<string, string>() { { "A", "4711" },{"B","4712" } };

            Assert.IsNotNull(contact);

            Contact contactClone = contact.ToClone<Contact>();
            Assert.IsNotNull(contactClone);
            Assert.That.StringEquals(contactClone.FirstName, "Gerhard");
            Assert.That.StringEquals(contactClone.LastName, "Ahrens");
            Assert.That.DateEquals(contactClone.Birthday, new DateTime(1960, 6, 28));
            Assert.AreEqual(contactClone.Status, true);
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

        private class Contact : ModelBase<Contact>
        {
            public Contact(string firstName, string lastName)
            {
                this.FirstName = firstName;
                this.LastName = lastName;
            }

            public string FirstName
            {
                get => base.GetValue<string>();
                set => base.SetValue(value);
            }

            public string LastName
            {
                get => base.GetValue<string>();
                set => base.SetValue(value);
            }

            public DateTime? Birthday
            {
                get => base.GetValue<DateTime?>();
                set => base.SetValue(value);
            }

            public bool Status
            {
                get => base.GetValue<bool>();
                set => base.SetValue(value);
            }

            public Dictionary<string,string> Phones
            {
                get => base.GetValue<Dictionary<string, string>>();
                set => base.SetValue(value);
            }
        }
    }
}
