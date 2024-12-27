/*
 * <copyright file="ObjectExtension_Test.cs" company="Lifeprojects.de">
 *     Class: ObjectExtension_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 15:10:18</date>
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

namespace ModernTest.ModernBaseLibrary
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ObjectExtension_Test
    {

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectToExtensions_Test"/> class.
        /// </summary>
        public ObjectExtension_Test()
        {
        }

        [TestMethod]
        public void DeepClone()
        {
            Person personA = new Person
            {
                Name = "A",
                Status = Status.An,
                Friends = new List<Person> { new Person { Name = "A1", DataTag = new Tag(1, "T1") } },
                DataTag = new Tag(11, "T11")
            };

            var personB = personA.DeepClone();

            Assert.IsFalse(object.ReferenceEquals(personA, personB));
            Assert.IsFalse(object.ReferenceEquals(personA.Friends, personB.Friends));
            Assert.IsFalse(object.ReferenceEquals(personA.Friends.First(), personB.Friends.First()));
            Assert.IsFalse(object.ReferenceEquals(personA.DataTag, personB.DataTag));
            Assert.IsTrue(personA.Status == personB.Status);

            Assert.AreEqual(personA.Name, personB.Name);
            Assert.AreEqual(personA.Status.ToString(), personB.Status.ToString());
            Assert.AreEqual(personA.DataTag.Id, personB.DataTag.Id);
            Assert.AreEqual(personA.DataTag.Value, personB.DataTag.Value);
            Assert.AreEqual(personA.Friends.Count, personB.Friends.Count);
            Assert.AreEqual(personA.Friends[0].Name, personB.Friends[0].Name);
        }
        private enum Status
        {
            An = 0,
            Aus = 1
        }

        private class Person
        {
            public string Name { get; set; }
            public Status Status { get; set; }
            public List<Person> Friends { get; set; }
            public Tag DataTag { get; set; }
        }

        private class Tag
        {
            public int Id { get; set; }
            public string Value { get; set; }

            public Tag(int id, string value)
            {
                Id = id;
                Value = value;
            }
        }
    }
}
