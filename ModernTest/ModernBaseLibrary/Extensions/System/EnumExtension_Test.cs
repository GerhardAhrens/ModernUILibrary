namespace ModernTest.ModernBaseLibrary
{
    using System.ComponentModel;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Zusammenfassungsbeschreibung für EnumExtension_Test
    /// </summary>
    [TestClass]
    public class EnumExtension_Test
    {
        public EnumExtension_Test()
        {
            //
            // TODO: Konstruktorlogik hier hinzufügen
            //
        }

        [TestMethod]
        public void EnumDescription_Test()
        {
            Coolness coolType1 = Coolness.Cool;

            Coolness coolType2 = Coolness.NotSoCool;

            Assert.IsTrue(coolType1.ToDescription() == "Cool");

            Assert.IsTrue(coolType2.ToDescription() == "Not so cool");

            Assert.IsTrue(coolType2.ToDescription<Coolness>() == "Not so cool");
        }

        [TestMethod]
        public void KeyToEnumWithoutDefault_Test()
        {
            Coolness coolType1 = "Cool".ToEnum<Coolness>();
            Assert.IsTrue(coolType1 == Coolness.Cool);

        }

        [TestMethod]
        public void GetEnumDefault_Test()
        {
            Coolness coolType1 = "AAAA".ToEnum<Coolness>();
            Assert.IsTrue(coolType1 == Coolness.None);

            Coolness coolType2 = "Cool".ToEnum<Coolness>();
            Assert.IsTrue(coolType2 == Coolness.Cool);
        }

        [TestMethod]
        public void KeyToEnumWithDefault_Test()
        {
            Coolness coolType1 = "CoolDefault".ToEnum<Coolness>(Coolness.VeryCool);
            Assert.IsTrue(coolType1 == Coolness.VeryCool);

            Coolness coolType2 = "Cool".ToEnum<Coolness>(Coolness.VeryCool);
            Assert.IsTrue(coolType2 == Coolness.Cool);
            Assert.IsTrue(coolType2 != Coolness.VeryCool);
        }

        [TestMethod]
        public void TryConvertIntToEnum_Test()
        {
            int none = 0;
            Coolness outValue;
            bool isEnum = none.TryConvertToEnum<Coolness>(out outValue);
            Assert.IsTrue(isEnum == true);
            Assert.IsTrue(outValue == Coolness.None);
        }

        [TestMethod]
        public void IntToEnum_Test()
        {
            int none = 0;
            Coolness outValue = none.ToEnum<Coolness>();
            Assert.IsTrue(outValue == Coolness.None);
        }

        [TestMethod]
        public void FromEnumDescription()
        {
            Coolness enumItems = "Nix".FromEnumDescription<Coolness>();
            Assert.IsTrue(Coolness.None == enumItems);
        }

        [TestMethod]
        public void FromEnumDescriptionNotFound()
        {
            Coolness enumItems = "Nixx".FromEnumDescription<Coolness>();
            Assert.IsTrue(Coolness.None == enumItems);
        }

        [TestMethod]
        public void FromEnumDescriptionWithDefault()
        {
            Coolness enumItems = "Nixx".FromEnumDescription<Coolness>(Coolness.None);
            Assert.IsTrue(Coolness.None == enumItems);
        }

        [TestMethod]
        public void EnumCount()
        {
            var enums = Coolness.None;
            var count = enums.Count();
            Assert.IsTrue(count == 5);
        }

        [TestMethod]
        public void ValueInEnumA()
        {
            var enums = Coolness.None;
            bool isIn = enums.In(Coolness.Cool,Coolness.None);
            Assert.IsTrue(isIn);
        }

        [TestMethod]
        public void ValueInEnumB()
        {
            var enums = Coolness.None;
            bool isIn = enums.In(Coolness.Cool, Coolness.NotSoCool);
            Assert.IsFalse(isIn);
        }

        [TestMethod]
        public void ValueNotInEnumA()
        {
            var enums = Coolness.None;
            bool isIn = enums.In(Coolness.Cool, Coolness.None);
            Assert.IsTrue(isIn);
        }

        [TestMethod]
        public void ValueNotInEnumB()
        {
            var enums = Coolness.None;
            bool isIn = enums.In(Coolness.Cool, Coolness.NotSoCool);
            Assert.IsFalse(isIn);
        }

        [TestMethod]
        public void EnumValueToInt()
        {
            var enums = Coolness.None;
            int result = enums.ToInt();
            Assert.IsTrue(result == 0);
        }

        [DefaultValue(None)]
        private enum Coolness

        {
            [System.ComponentModel.Description("Nix")]
            None = 0,

            [System.ComponentModel.Description("Not so cool")]
            NotSoCool = 5,

            Cool,

            [System.ComponentModel.Description("Very cool")]
            VeryCool = NotSoCool + 7,

            [System.ComponentModel.Description("Super cool")]
            SuperCool
        }
    }
}