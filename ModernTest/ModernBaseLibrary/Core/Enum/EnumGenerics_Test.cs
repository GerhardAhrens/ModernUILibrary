using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ModernBaseLibrary.Core;
using ModernBaseLibrary.Extension;

using ModernTest.ModernBaseLibrary;

namespace EasyPrototypingTest
{
    [TestClass]
    public class EnumGenerics_Test
    {
        const string ValidName = "Nine";
        const int ValidInteger = 9;
        const NumberedEnum ValidEnum = NumberedEnum.Nine;

        const string InvalidName = "Ninety";
        const int InvalidInteger = 800;

        [TestMethod]
        public void CastOrNullCastsWhenValidInteger()
        {
            var actual = Enum<NumberedEnum>.CastOrNull(ValidInteger);

            Assert.AreEqual(ValidEnum, actual);
        }

        [TestMethod]
        public void CastOrNullNullsWhenInvalidInteger()
        {
            var actual = Enum<NumberedEnum>.CastOrNull(InvalidInteger);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void GetFlagsHandlesNoFlagsSet()
        {
            var actual = Enum<FlagEnum>.GetFlags(default(FlagEnum));

            Assert.That.IsEmpty(actual);
        }

        [TestMethod]
        public void GetFlagsReturnsTheMultipleFlagsSet()
        {
            const FlagEnum multipleFlags = FlagEnum.BitOne | FlagEnum.BitFive | FlagEnum.BitThree;
            var actual = Enum<FlagEnum>.GetFlags(multipleFlags);

            Assert.AreEqual(new[] { FlagEnum.BitOne, FlagEnum.BitThree, FlagEnum.BitFive }, actual.ToArray());
        }

        [TestMethod]
        public void GetFlagsReturnsTheMultipleFlagsSetIgnoringUndefined()
        {
            const FlagEnum multipleFlags = (FlagEnum)255;
            var actual = Enum<FlagEnum>.GetFlags(multipleFlags);

            Assert.AreEqual(new[] { FlagEnum.BitOne, FlagEnum.BitTwo, FlagEnum.BitThree, FlagEnum.BitFour, FlagEnum.BitsTwoAndFour, FlagEnum.BitFive, FlagEnum.BitSix }, actual.ToArray());
        }

        [TestMethod]
        public void GetFlagsReturnsTheMultipleFlagsSetIncludingCombinedMasks()
        {
            const FlagEnum combinedFlags = FlagEnum.BitsTwoAndFour;
            var actual = Enum<FlagEnum>.GetFlags(combinedFlags);

            Assert.AreEqual(new[] { FlagEnum.BitTwo, FlagEnum.BitFour, FlagEnum.BitsTwoAndFour }, actual.ToArray());
        }

        [TestMethod]
        public void GetFlags_Returns_The_One_Flag_Set()
        {
            var actual = Enum<FlagEnum>.GetFlags(FlagEnum.BitOne);

            Assert.AreEqual(new[] { FlagEnum.BitOne }, actual.ToArray());
        }

        [TestMethod]
        public void GetNameBehavesSame()
        {
            var expected = Enum.GetName(typeof(NumberedEnum), NumberedEnum.Five);
            var actual = Enum<NumberedEnum>.GetName(NumberedEnum.Five);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetNamesBehavesSame()
        {
            var expected = Enum.GetNames(typeof(NumberedEnum));
            var actual = Enum<NumberedEnum>.GetNames();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetValuesBehavesSame()
        {
            var expected = Enum.GetValues(typeof(NumberedEnum));
            var actual = Enum<NumberedEnum>.GetValues().ToArray();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsDefinedWithInvalidIntegerBehavesSame()
        {
            var expected = Enum.IsDefined(typeof(NumberedEnum), InvalidInteger);
            var actual = Enum<NumberedEnum>.IsDefined(InvalidInteger);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsDefinedWithValidIntegerBehavesSame()
        {
            var expected = Enum.IsDefined(typeof(NumberedEnum), ValidInteger);
            var actual = Enum<NumberedEnum>.IsDefined(ValidInteger);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseOrNullInsensitiveReturnsCorrectValueWhenValidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName, true, out parsedValue);

            Assert.IsTrue(didParse);
            Assert.AreEqual(ValidEnum, parsedValue);
        }

        [TestMethod]
        public void ParseOrNullInsensitiveReturnsCorrectValueWhenValidNameWrongCase()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName.ToUpper(), true, out parsedValue);

            Assert.IsTrue(didParse);
            Assert.AreEqual(ValidEnum, parsedValue);
        }

        [TestMethod]
        public void ParseOrNullInsensitiveReturnsNullWhenInvalidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(InvalidName, true, out parsedValue);

            Assert.IsFalse(didParse);
        }

        [TestMethod]
        public void ParseOrNullReturnsCorrectValueWhenValidName()
        {
            var actual = Enum<NumberedEnum>.ParseOrNull(ValidName);

            Assert.AreEqual(ValidEnum, actual);
        }

        [TestMethod]
        public void ParseOrNullReturnsNullValueWhenInvalidName()
        {
            var actual = Enum<NumberedEnum>.ParseOrNull(InvalidName);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ParseOrNullReturnsNullValueWhenValidNameWrongCase()
        {
            var actual = Enum<NumberedEnum>.ParseOrNull(ValidName.ToUpper());

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ParseOrNullSensitiveReturnsCorrectValueWhenValidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName, false, out parsedValue);

            Assert.IsTrue(didParse);
            Assert.AreEqual(ValidEnum, parsedValue);
        }

        [TestMethod]
        public void ParseOrNullSensitiveReturnsFalseWhenValidNameWrongCase()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName.ToUpper(), false, out parsedValue);

            Assert.IsFalse(didParse);
        }

        [TestMethod]
        public void ParseOrNullSensitiveReturnsNullWhenInvalidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(InvalidName, false, out parsedValue);

            Assert.IsFalse(didParse);
        }

        [TestMethod]
        public void ParseReturnsValidValueWhenValidName()
        {
            var actual = Enum<NumberedEnum>.Parse(ValidName);

            Assert.AreEqual(ValidEnum, actual);
        }

        [TestMethod]
        public void ParseThrowsWhenInvalidName()
        {
            Assert.ThrowsException<ArgumentException>(() => Enum<NumberedEnum>.Parse(InvalidName));
        }

        [TestMethod]
        public void SetFlagsShouldDefaultWhenGivenNoValues()
        {
            const FlagEnum expected = default(FlagEnum);
            var actual = Enum<FlagEnum>.SetFlags(Enumerable.Empty<FlagEnum>());

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetFlagsShouldReturnCombinationFlagFromIndividualMasks()
        {
            const FlagEnum expected = FlagEnum.BitsTwoAndFour;
            var actual = Enum<FlagEnum>.SetFlags(new[] { FlagEnum.BitFour, FlagEnum.BitTwo });

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TryParseInsensitiveReturnsFalseWhenInvalidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(InvalidName, true, out parsedValue);

            Assert.IsFalse(didParse);
        }

        [TestMethod]
        public void TryParseInsensitiveReturnsTrueAndCorrectOutputValueWhenValidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName, true, out parsedValue);

            Assert.IsTrue(didParse);
            Assert.AreEqual(ValidEnum, parsedValue);
        }

        [TestMethod]
        public void TryParseInsensitiveReturnsTrueAndCorrectOutputValueWhenValidNameWrongCase()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName.ToUpper(), true, out parsedValue);

            Assert.IsTrue(didParse);
            Assert.AreEqual(ValidEnum, parsedValue);
        }

        [TestMethod]
        public void TryParseReturnsFalseWhenInvalidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(InvalidName, out parsedValue);

            Assert.IsFalse(didParse);
        }

        [TestMethod]
        public void TryParseReturnsFalseWhenValidNameWrongCase()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName.ToUpper(), false, out parsedValue);

            Assert.IsFalse(didParse);
        }

        [TestMethod]
        public void TryParseReturnsTrueAndCorrectOutputValueWhenValidName()
        {
            NumberedEnum parsedValue;
            var didParse = Enum<NumberedEnum>.TryParse(ValidName, out parsedValue);

            Assert.IsTrue(didParse);
            Assert.AreEqual(ValidEnum, parsedValue);
        }

        private enum NumberedEnum
        {
            One = 1,
            Two = 2,
            Zero = 0,
            Nine = 9,
            Five = 5
        };

        [Flags]
        private enum FlagEnum
        {
            BitOne = 1,
            BitTwo = 2,
            BitThree = 4,
            BitFour = 8,
            BitFive = 16,
            BitSix = 32,
            BitsTwoAndFour = 10
        };
    }
}