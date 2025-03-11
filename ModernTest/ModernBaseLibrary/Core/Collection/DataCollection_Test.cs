/*
 * <copyright file="DataCollection_Test.cs" company="Lifeprojects.de">
 *     Class: DataCollection_Test
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>11.03.2025 20:14:30</date>
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

namespace ModernTest.ModernBaseLibrary.Collection
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Globalization;
    using System.Diagnostics;
    using global::ModernBaseLibrary.Collection;
    using System.Windows.Data;

    [TestClass]
    public class DataCollection_Test : BaseTest
    {
        private static Stopwatch watchData;

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataCollection_Test"/> class.
        /// </summary>
        public DataCollection_Test()
        {
            watchData = new Stopwatch();
        }

        private static bool DataTextFilterV1(Contact rowItem)
        {
            return true;
        }

        private static bool DataTextFilterV2(Author rowItem)
        {
            return true;
        }

        private static bool DataTextFilterV3(AuthorV0 rowItem)
        {
            return true;
        }

        private static bool DataTextFilterV4(AuthorV1 rowItem)
        {
            return true;
        }

        [TestMethod]
        public void Contact_DataTextFilterV1()
        {
            watchData.Reset();
            watchData.Start();

            List<Contact> contacts = new List<Contact>();
            const int maxCount = 1_000_000;

            for (int i = 0; i < maxCount; i++)
            {
                Contact contact = new Contact();
                contact.Id = Guid.NewGuid();
                contact.IdCounter = i;
                contact.Name = "Gerhard";
                contact.Age = 10;
                contact.Birthday = DateTime.Now;
                contacts.Add(contact);
            }

            DataCollection<Contact> dc = new DataCollection<Contact>(contacts);
            dc.RaiseListChangedEvents = true;
            int index = dc.FindIndex("IdCounter", 10);
            if (index > 0)
            {
                string v1 = dc[index].Name;
                dc[index].Name = "Ahrens";
            }
            dc.ResetModified();

            ListCollectionView view = dc.View(item => DataTextFilterV1(item as Contact));

            watchData.Stop();

            Trace("Fertig V1: {0}mit 'this.Get<Guid>();/this.Set(value);'", watchData.ElapsedMilliseconds);
        }

        [TestMethod]
        public void Author_DataTextFilterV2()
        {
            watchData.Reset();
            watchData.Start();

            IEnumerable<Author> result = Author_Test();

            watchData.Stop();

            Trace("Fertig: V2 {0} mit this.SetProperty(ref this.id, value))", watchData.ElapsedMilliseconds);
        }

        private IEnumerable<Author> Author_Test()
        {
            List<Author> authors = new List<Author>();
            const int maxCount = 1000000;

            for (int i = 0; i < maxCount; i++)
            {
                Author autor = new Author();
                autor.Id = Guid.NewGuid();
                autor.Name = "Gerhard";
                autor.Age = 10;
                autor.Birthday = DateTime.Now;
                authors.Add(autor);
            }

            DataCollection<Author> dc = new DataCollection<Author>(authors);

            ListCollectionView view = dc.View(item => DataTextFilterV2(item as Author));

            return dc;
        }

        [TestMethod]
        public void Author_DataTextFilterV3()
        {
            watchData.Reset();
            watchData.Start();

            IEnumerable<AuthorV0> result = AuthorV0_Test();

            watchData.Stop();

            Trace("Fertig: V3 {0} mit ' get membervaraiablen; set membervaraiablen; '", watchData.ElapsedMilliseconds);
        }

        private IEnumerable<AuthorV0> AuthorV0_Test()
        {
            List<AuthorV0> authors = new List<AuthorV0>();
            const int maxCount = 1000000;

            for (int i = 0; i < maxCount; i++)
            {
                AuthorV0 autor = new AuthorV0();
                autor.Id = Guid.NewGuid();
                autor.Name = "Gerhard";
                autor.Age = 10;
                autor.Birthday = DateTime.Now;
                authors.Add(autor);
            }

            DataCollection<AuthorV0> dc = new DataCollection<AuthorV0>(authors);
            ListCollectionView view = dc.View(item => DataTextFilterV3(item as AuthorV0));

            return dc;
        }

        [TestMethod]
        public void Author_DataTextFilterV4()
        {
            watchData.Reset();
            watchData.Start();

            IEnumerable<AuthorV1> result = AuthorV1_Test();

            watchData.Stop();

            Trace("Fertig: V4 {0} mit ' public string Name {{get; set;}}'", watchData.ElapsedMilliseconds);
        }

        private static IEnumerable<AuthorV1> AuthorV1_Test()
        {
            List<AuthorV1> authors = new List<AuthorV1>();
            const int maxCount = 1000000;

            for (int i = 0; i < maxCount; i++)
            {
                AuthorV1 autor = new AuthorV1();
                autor.Id = Guid.NewGuid();
                autor.Name = "Gerhard";
                autor.Age = 10;
                autor.Birthday = DateTime.Now;
                authors.Add(autor);
            }

            DataCollection<AuthorV1> dc = new DataCollection<AuthorV1>(authors);
            ListCollectionView view = dc.View(item => DataTextFilterV4(item as AuthorV1));

            return dc;
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
}
