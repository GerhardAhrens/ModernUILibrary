//-----------------------------------------------------------------------
// <copyright file="CollectionLookup.cs" company="Lifeprojects.de">
//     Class: CollectionLookup
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>17.03.2025 13:51:18</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Core.Collection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CollectionLookup : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionLookup"/> class.
        /// </summary>
        public CollectionLookup()
        {
        }

        /// <summary>
        /// https://linqexamples.com/collections/tolookup.html#basic-example
        /// </summary>
        [TestMethod]
        public void ILookUp_Test()
        {
            ILookup<string, string> lookup = this.CreateDemoData().ToLookup(arg => arg.City, arg => arg.Name);

            /*
            EnvDTE80.DTE2 ide = (EnvDTE80.DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE");
            ide.ExecuteCommand("Edit.ClearOutputWindow", ""); System.Runtime.InteropServices.Marshal.ReleaseComObject(ide);
            */

            IEnumerable<string> personenInMannheim = lookup["Mannheim"];
            personenInMannheim.ForEach<string>(p => Trace(p));
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

        private class Person
        {
            public string Name { get; set; }
            public string Gender { get; set; }
            public string City { get; set; }
        }
    }
}
