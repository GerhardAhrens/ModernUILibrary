//-----------------------------------------------------------------------
// <copyright file="SmartSettingsCustomClassTest.cs" company="Lifeprojects.de">
//     Class: SmartSettingsCustomClassTest
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
    using System.Threading;

    using global::ModernBaseLibrary.Core.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SmartSettingsSpecialClass_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartSettingsSpecialClass_Test"/> class.
        /// </summary>
        public SmartSettingsSpecialClass_Test()
        {
        }

        [TestMethod]
        public void CreateSaveSettingsWithCustomClass_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoSettingsWithCustomClass(file.Path)
                {
                    CustomClassProperty = new DemoSettingsWithCustomClass.CustomClass
                    {
                        IntProperty = 42,
                        StringProperty = "foo"
                    }
                };

                settings.Save();

                var loadedSettings = new DemoSettingsWithCustomClass(file.Path);
                bool isLoaded = loadedSettings.Load();

                Assert.IsTrue(isLoaded);
                Assert.That.AreEqualValue(settings, loadedSettings);
            }
        }

        [TestMethod]
        public void CreateSaveSettingsWithCustomEnumClass_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoSettingsWithCustomEnum(file.Path)
                {
                    CustomEnumProperty = DemoSettingsWithCustomEnum.CustomEnum.Bar
                };

                settings.Save();

                var loadedSettings = new DemoSettingsWithCustomEnum(file.Path);
                bool isLoaded = loadedSettings.Load();

                Assert.IsTrue(isLoaded);
                Assert.That.AreEqualValue(settings, loadedSettings);
            }
        }

        [TestMethod]
        public void CreateSaveSettingsWithCustomDateOnlyClass_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoSettingsWithDateOnly(file.Path)
                {
                    DateOnlyProperty = new DateOnly(1960, 06, 28)
                };

                settings.Save();

                var loadedSettings = new DemoSettingsWithDateOnly(file.Path);
                bool isLoaded = loadedSettings.Load();

                Assert.IsTrue(isLoaded);
                Assert.That.AreEqualValue(settings, loadedSettings);
            }
        }

        [TestMethod]
        public void CreateSaveSettingsWithCustomTimeOnlyClass_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoSettingsWithTimeOnly(file.Path)
                {
                    TimeOnlyProperty = new TimeOnly(23, 59)
                };

                settings.Save();

                var loadedSettings = new DemoSettingsWithTimeOnly(file.Path);
                bool isLoaded = loadedSettings.Load();

                Assert.IsTrue(isLoaded);
                Assert.That.AreEqualValue(settings, loadedSettings);
            }
        }

        [TestMethod]
        public void CreateSaveSettingsWithCustomTimeSpanClass_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoeSettingsWithTimeSpan(file.Path)
                {
                    TimeSpanProperty = TimeSpan.FromMinutes(3.14)
                };

                settings.Save();

                var loadedSettings = new DemoeSettingsWithTimeSpan(file.Path);
                bool isLoaded = loadedSettings.Load();

                Assert.IsTrue(isLoaded);
                Assert.That.AreEqualValue(settings, loadedSettings);
            }
        }

        [TestMethod]
        public void CreateSaveSettingsWithNamedProperty_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoSettingsWithNamedProperty(file.Path)
                {
                    IntProperty = 42
                };

                settings.Save();

                var loadedSettings = new DemoSettingsWithNamedProperty(file.Path);
                bool isLoaded = loadedSettings.Load();

                Assert.IsTrue(isLoaded);
                Assert.That.AreEqualValue(settings, loadedSettings);
            }
        }

        [TestMethod]
        public void CreateSaveSettingsWithIgnorProperty_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoSettingsWithIgnoredProperty(file.Path)
                {
                    IntProperty = 42,
                    IgnoredProperty = "foo"
                };

                settings.Save();

                var loadedSettings = new DemoSettingsWithIgnoredProperty(file.Path);
                bool isLoaded = loadedSettings.Load();

                Assert.IsTrue(isLoaded);
                Assert.That.AreEqualValue(settings, loadedSettings);
            }
        }

        [TestMethod]
        public void CreateSaveSettingsWithImmutableClass_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoSettingsWithCustomImmutableClass(file.Path)
                {
                    CustomClassProperty = new DemoSettingsWithCustomImmutableClass.CustomClass(42, "foo")

                };

                settings.Save();

                var loadedSettings = new DemoSettingsWithCustomImmutableClass(file.Path);
                bool isLoaded = loadedSettings.Load();

                Assert.IsTrue(isLoaded);
                Assert.That.AreEqualValue(settings, loadedSettings);
            }
        }

        [TestMethod]
        public void CreateSaveSettingsWithImmutableStruct_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoSettingsWithCustomImmutableStruct(file.Path)
                {
                    CustomStructProperty = new DemoSettingsWithCustomImmutableStruct.CustomStruct(42, "foo")

                };

                settings.Save();

                var loadedSettings = new DemoSettingsWithCustomImmutableStruct(file.Path);
                bool isLoaded = loadedSettings.Load();

                Assert.IsTrue(isLoaded);
                Assert.That.AreEqualValue(settings, loadedSettings);
            }
        }

        [TestMethod]
        public void CreateSaveSettingsWithRecordType_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoSettingsWithCustomRecord(file.Path)
                {
                    CustomRecordProperty = new DemoSettingsWithCustomRecord.CustomRecord(42, "foo")
                };

                settings.Save();

                var loadedSettings = new DemoSettingsWithCustomRecord(file.Path);
                bool isLoaded = loadedSettings.Load();

                Assert.IsTrue(isLoaded);
                Assert.That.AreEqualValue(settings, loadedSettings);
            }
        }

        [TestMethod]
        public void CreateSaveSettingsWithRecordStructType_Test()
        {
            using (var file = TempFile.Create())
            {
                var settings = new DemoSettingsWithCustomStructRecord(file.Path)
                {
                    CustomRecordProperty = new DemoSettingsWithCustomStructRecord.CustomRecord(42, "foo")
                };

                settings.Save();

                var loadedSettings = new DemoSettingsWithCustomStructRecord(file.Path);
                bool isLoaded = loadedSettings.Load();

                Assert.IsTrue(isLoaded);
                Assert.That.AreEqualValue(settings, loadedSettings);
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
