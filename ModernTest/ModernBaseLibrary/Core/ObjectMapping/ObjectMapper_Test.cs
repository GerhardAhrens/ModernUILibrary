//-----------------------------------------------------------------------
// <copyright file="ObjectMapper_Test.cs" company="Lifeprojects.de">
//     Class: ObjectMapper_Test
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.04.2022 06:58:07</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using EasyPrototypingNET.Core;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ObjectMapper_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMapper_Test"/> class.
        /// </summary>
        public ObjectMapper_Test()
        {
        }

        [TestMethod]
        public void MapToObject_Dictionary()
        {
            var item1 = new Dictionary<string, string>();
            item1.Add("FirstName", "Tim");
            item1.Add("LastName", "Corey");

            var item2 = new Dictionary<string, string>();
            item2.Add("FirstName", "Nick");
            item2.Add("LastName", "Chapsas");

            var dictList = new List<Dictionary<string, string>>();
            dictList.Add(item1);
            dictList.Add(item2);

            var result = ObjectMapper.MapToObject<PersonModel>(dictList);
            string dump = ObjectDump.Dump(result);

            var result2 = ObjectMapper.MapToObject<Person2Model>(dictList);
            string dump2 = ObjectDump.Dump(result2);
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

        private class PersonModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private class Person2Model
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string SomeOther { get; set; }
        }
    }
}
