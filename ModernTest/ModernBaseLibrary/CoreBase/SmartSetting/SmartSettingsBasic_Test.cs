//-----------------------------------------------------------------------
// <copyright file="SmartSettingsBasic_Test.cs" company="Lifeprojects.de">
//     Class: SmartSettingsBasic_Test
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.10.2022 11:58:29</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.SmartSettings
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading;

    using global::ModernBaseLibrary.Core.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SmartSettingsBasic_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartSettingsBasic_Test"/> class.
        /// </summary>
        public SmartSettingsBasic_Test()
        {
        }

        [TestMethod]
        public void SmartSettings_Base_Test()
        {
            using var file = TempFile.Create();
            var settings = new DemoSettings(file.Path);

            settings.Save();

            string settingsName = settings.Filename;

            bool isExist = File.Exists(file.Path);
            Assert.IsTrue(isExist);

            var properties = settings.GetProperties;

            int countProperties = settings.Count;
            Assert.AreEqual(countProperties, 4);
        }

        [TestMethod]
        public void CreateSaveSettings_Empty_Test()
        {
            using var file = TempFile.Create();
            var settings = new DemoSettings(file.Path);

            settings.Save();

            bool isExist = File.Exists(file.Path);
            Assert.IsTrue(isExist);

        }

        [TestMethod]
        public void LoadSavedSettings_Test()
        {
            using var file = TempFile.Create();
            var settings = new DemoSettings(file.Path)
            {
                IntProperty = 42,
                BoolProperty = true,
                StringProperty = "foo",
                StringPropertyWithDefaultValue = "bar"
            };

            settings.Save();

            DemoSettings loadedSettings = new DemoSettings(file.Path);
            var isLoaded = loadedSettings.Load();

            Assert.IsTrue(isLoaded);
            Assert.That.AreEqualValue(settings, loadedSettings);
        }

        [TestMethod]
        public void LoadSaveResetSettings_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoSettings(file.Path)
                {
                    IntProperty = 42,
                    BoolProperty = true,
                    StringProperty = "foo",
                    StringPropertyWithDefaultValue = "bar"
                };

                settings.Reset();

                DemoSettings loadedSettings = new DemoSettings(file.Path);

                Assert.That.AreEqualValue(settings, loadedSettings);
            }
        }

        [TestMethod]
        public void DeleteSettings_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoSettings(file.Path);

                settings.Save();

                var wasDeleted = settings.Delete();

                Assert.IsTrue(wasDeleted);

                bool isExist = File.Exists(file.Path);
                Assert.IsFalse(isExist);
            }
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
