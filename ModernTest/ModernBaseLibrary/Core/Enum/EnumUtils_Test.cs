using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ModernBaseLibrary.Core;
using ModernBaseLibrary.Extension;

namespace ModernTest.ModernBaseLibrary
{
    [TestClass]
    public class EnumUtils_Test
    {
        [TestMethod]
        public void IsReturnValueTypeIsEnum()
        {
            Weekday weekday = Weekday.Mon;
            bool isEnum = EnumUtils.IsEnum(weekday);
            Assert.IsTrue(isEnum);
        }

        [TestMethod]
        public void IsReturnValueTypeIsEnum_ExtensionMethod()
        {
            var weekday = Weekday.Mon;
            bool isEnum = weekday.IsEnum();
            Assert.IsTrue(isEnum);
        }

        [TestMethod]
        public void IsReturnValueTypeIsEnum_Generic()
        {
            bool isEnum = EnumUtils.IsEnum<Weekday>();
            Assert.IsTrue(isEnum);
        }

        [TestMethod]
        public void IsReturnValueTypeIsEnum_Generic_ExtensionMethod()
        {
            object weekday = Weekday.Mon;
            bool isEnum = weekday.IsEnum();
            Assert.IsTrue(isEnum);
        }

        [TestMethod]
        public void ShouldReturnFalseIfTypeIsNotAnEnum()
        {
            bool isEnum = EnumUtils.IsEnum<string>();
            Assert.IsFalse(isEnum);
        }

        [TestMethod]
        public void ShouldGetValuesFromEnum()
        {
            IEnumerable<Weekday> weekdays = EnumUtils.GetValues<Weekday>();
            Assert.IsNotNull(weekdays);
            Assert.That.HaveCount<Weekday>(weekdays, 8);
        }

        [TestMethod]
        public void ShouldGetNameFromEnum()
        {
            string weekday = EnumUtils.GetName(Weekday.Tue);
            Assert.IsNotNull(weekday);
            Assert.That.StringEquals(weekday, "Tue");
        }

        [TestMethod]
        public void ShouldGetNamesFromEnum()
        {
            IEnumerable<string> weekdays = EnumUtils.GetNames<Weekday>();
            Assert.IsNotNull(weekdays);
            Assert.That.HaveCount<string>(weekdays, 8);
        }

        [TestMethod]
        public void ShouldGetValuesFromNullableEnum()
        {
            IEnumerable<Weekday?> weekdays = EnumUtils.GetValues<Weekday?>();
            Assert.IsNotNull(weekdays);
            Assert.That.HaveCount<Weekday?>(weekdays, 8);
        }

        [TestMethod]
        public void ShouldGetRandom()
        {
            var randomWeekdays = new List<Weekday>
            {
                EnumUtils.GetRandom<Weekday>(),
                EnumUtils.GetRandom<Weekday>(),
                EnumUtils.GetRandom<Weekday>(),
                EnumUtils.GetRandom<Weekday>(),
                EnumUtils.GetRandom<Weekday>(),
                EnumUtils.GetRandom<Weekday>(),
                EnumUtils.GetRandom<Weekday>(),
            };


            var weekdayGroups = randomWeekdays.GroupBy(s => s).Select(g => new
            {
                Weekday = g.Key,
                Count = g.Count()
            });

            Assert.IsNotNull(weekdayGroups);

            foreach (var group in weekdayGroups)
            {
                Assert.IsTrue(group.Count > 0);
            }
        }

        [TestMethod]
        public void ShouldParseWithSuccess()
        {
            string enumString = "Thu";
            var parsed = EnumUtils.Parse<Weekday>(enumString);

            Assert.IsNotNull(parsed);
            Assert.IsTrue(parsed == Weekday.Thu);
        }

        [TestMethod]
        public void ShouldParseWithFailure()
        {
            string enumString = "non-existent-weekday";
            Action action = () => EnumUtils.Parse<Weekday>(enumString);

            Assert.ThrowsException<ArgumentException>(action).Message.Contains(enumString);
        }

        [TestMethod]
        public void ShouldTryParseWithSuccess()
        {
            string enumString = "Fri";

            var parsed = EnumUtils.TryParse<Weekday>(enumString);

            Assert.IsNotNull(parsed);
            Assert.IsTrue(parsed == Weekday.Fri);
        }

        [TestMethod]
        public void ShouldTryParseWithFailureAndGetDefault()
        {
            string enumString = "non-existent-weekday";
            var parsed = EnumUtils.TryParse<Weekday>(enumString);

            Assert.IsNotNull(parsed);
            Assert.IsTrue(parsed == default(Weekday));
        }

        [TestMethod]
        public void ShouldCastToEnum()
        {
            int enumValue = 3;
            var casted = EnumUtils.Cast<Weekday>(enumValue);

            Assert.IsNotNull(casted);
            Assert.IsTrue(casted == Weekday.Tue);
        }

        [TestMethod]
        public void ShouldCastToEnumWithDefaultValue()
        {
            int enumValue = 33;
            var casted = EnumUtils.Cast(enumValue, defaultValue: Weekday.Mon);

            Assert.IsNotNull(casted);
            Assert.IsTrue(casted == Weekday.Mon);
        }

        [TestMethod]
        public void ShouldReturnRandomEnum()
        {
            const int count = 1000;
            List<Weekday> randomWeekdays = Enumerable.Range(0, count).Select(x => EnumUtils.GetRandom<Weekday>()).ToList();

            Assert.IsNotNull(randomWeekdays);
            Assert.That.HaveCount<Weekday>(randomWeekdays, count);
        }

        [TestMethod]
        public void ShouldCountEnums()
        {
            int count = EnumUtils.Count<Weekday>();

            Assert.IsTrue(count == 8);
        }

        [TestMethod]
        public void ShouldGetDescriptions()
        {
            IDictionary<Weekday,string> weekdayDescriptions = EnumUtils.GetDescriptions<Weekday>();
            Assert.IsNotNull(weekdayDescriptions);
            Assert.That.HaveCount<IDictionary<Weekday, string>>(weekdayDescriptions, EnumUtils.Count<Weekday>());
        }

        [TestMethod]
        public void EnumToList()
        {
            List<DayOfWeek> weekdays = EnumUtils.EnumToList<DayOfWeek>();
            Assert.IsTrue(weekdays.Count == 7);
        }

        [TestMethod]
        public void EnumToList_WithDelegate()
        {
            List<DayOfWeek> weekdays = EnumUtils.EnumToList<DayOfWeek>().FindAll(
            delegate (DayOfWeek x)
            {
                return x != DayOfWeek.Sunday && x != DayOfWeek.Saturday;
            });
            Assert.IsTrue(weekdays.Count == 5);
        }

        [DefaultValue(None)]
        private enum Weekday
        {
            None,
            Sun,
            Mon,
            Tue,
            Wed,
            Thu,

            [System.ComponentModel.Description("Friday")]
            Fri,

            [System.ComponentModel.Description("Saturday")]
            Sat
        };
    }
}