//-----------------------------------------------------------------------
// <copyright file="ICollectionViewGeneric_Test.cs" company="Lifeprojects.de">
//     Class: ICollectionViewGeneric_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>15.04.2025 08:31:24</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Data;

    using global::ModernBaseLibrary.Collection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class ICollectionViewGeneric_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ICollectionViewGeneric_Test"/> class.
        /// </summary>
        public ICollectionViewGeneric_Test()
        {
        }

        [TestMethod]
        public void CreateCollectionViewGeneric()
        {
            ICollectionView<Person> view = null;
            view = new CollectionViewGeneric<Person>(CollectionViewSource.GetDefaultView(this.CreateDemoData()));

            Assert.AreEqual(view.CountRow, 4);
        }

        [TestMethod]
        public void FilterCollectionViewGenericEmpty()
        {
            ICollectionView<Person> view = null;
            string textsearch = string.Empty;

            view = new CollectionViewGeneric<Person>(CollectionViewSource.GetDefaultView(this.CreateDemoData()));
            view.Filter += (object row) => 
                { 
                    if (textsearch == null)
                    {
                        return true;
                    }

                    Person item = (Person)row;
                    return item.ToString().Contains(textsearch);
                };

            Assert.AreEqual(view.CountRow, 4);
        }

        [DataRow("Female", 2)]
        [DataRow("Neuhofen", 1)]
        [TestMethod]
        public void DataRowInputTest(string input, int expected)
        {
            ICollectionView<Person> view = null;
            string textsearch = input;

            view = new CollectionViewGeneric<Person>(CollectionViewSource.GetDefaultView(this.CreateDemoData()));
            view.Filter += (object row) =>
            {
                if (textsearch == null)
                {
                    return true;
                }

                Person item = (Person)row;
                return item.ToString().Contains(textsearch);
            };

            Assert.AreEqual(view.CountRow, 4);
            Assert.AreEqual(view.CountFilter, expected);
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

        private List<Person> CreateDemoData()
        {
            List<Person> personen = new List<Person>
            {
                new Person { Name = "Vernon", Gender = "Male", City = "Mannheim"},
                new Person { Name = "Carrie", Gender = "Female", City = "Mannheim" },
                new Person { Name = "Joanna", Gender = "Female", City = "Ludwigshafen" },
                new Person { Name = "Gerhard", Gender = "Male", City = "Neuhofen" }
            };

            return personen;
        }

        [DebuggerDisplay("Name:{this.Name};Gender:{this.Gender};{this.City}")]
        private class Person
        {
            public string Name { get; set; }
            public string Gender { get; set; }
            public string City { get; set; }

            public override string ToString()
            {
                return $"{this.Name}|{this.Gender}|{this.City}";
            }
        }
    }
}
