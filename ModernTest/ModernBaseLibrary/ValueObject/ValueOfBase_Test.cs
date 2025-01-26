namespace ModernTest.ModernBaseLibrary.CoreBase
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.CoreBase;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValueOfBase_Test
    {
        [TestInitialize]
        public void TestSetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void SingleValuedExample()
        {
            ClientRef clientRef1 = ClientRef.From("ASDF12345");
            ClientRef clientRef2 = ClientRef.From("ASDF12345");
            Assert.AreEqual(clientRef1, clientRef2);
            Assert.AreEqual(clientRef1.GetHashCode(), clientRef2.GetHashCode());

            ClientRef clientRef3 = ClientRef.From("QWER98765");
            Assert.AreNotEqual(clientRef1, clientRef3);
            Assert.AreNotEqual(clientRef1.GetHashCode(), clientRef3.GetHashCode());
        }

        [TestMethod]
        public void MultiValuedExample()
        {
            Address address1 = Address.From(("Dorfstrasse 1", "Musterdorf", Postcode.From("67999")));
            Address address2 = Address.From(("Dorfstrasse 1", "Musterdorf", Postcode.From("67999")));
            Assert.AreEqual(address1, address2);
            Assert.AreEqual(address1.GetHashCode(), address2.GetHashCode());

            Address address3 = Address.From(("Hauptstrasse 99", "Musterstadt", Postcode.From("70999")));
            Assert.AreNotEqual(address1, address3);
            Assert.AreNotEqual(address1.GetHashCode(), address3.GetHashCode());
        }

        #region Equals by CaseInsensitive
        [TestMethod]
        public void CaseInsensitiveMultiValuedExample()
        {
            Address address1 = Address.From(("Dorfstrasse 1", "Musterdorf", Postcode.From("67999")));
            Address address2 = Address.From(("DORFSTRASSE 1", "MUSTERDORF", Postcode.From("67999")));
            Assert.AreEqual(address1, address2);
            Assert.AreEqual(address1.GetHashCode(), address2.GetHashCode());
        }

        [TestMethod]
        public void CaseInsensitiveEquals()
        {
            CaseInsensitiveClientRef clientRef1 = CaseInsensitiveClientRef.From("ASDF12345");
            CaseInsensitiveClientRef clientRef2 = CaseInsensitiveClientRef.From("asdf12345");
            Assert.AreEqual(clientRef1, clientRef2);
            Assert.AreEqual(clientRef1.GetHashCode(), clientRef2.GetHashCode());
            Assert.IsTrue(clientRef1 == clientRef2);
            Assert.IsTrue(clientRef1.Value == "ASDF12345");

            CaseInsensitiveClientRef clientRef3 = CaseInsensitiveClientRef.From("QWER98765");
            Assert.AreNotEqual(clientRef1, clientRef3);
            Assert.AreNotEqual(clientRef1.GetHashCode(), clientRef3.GetHashCode());
            Assert.IsFalse(clientRef1 == clientRef3);
        }
        #endregion Equals by CaseInsensitive

        #region ToString Returns ValuedObjects
        [TestMethod]
        public void ToStringReturnsValueFromSingleValuedObjects()
        {
            ClientRef clientRef1 = ClientRef.From("ASDF12345");

            Assert.AreEqual(clientRef1.Value, clientRef1.ToString());
        }

        [TestMethod]
        public void ToStringReturnsValueFromMultiValuedObjects()
        {
            Address address1 = Address.From(("Dorfstrasse 1", "Musterdorf", Postcode.From("67999")));

            Assert.AreEqual(address1.Value.ToString(), address1.ToString());

        }
        #endregion ToString Returns ValuedObjects

        #region CopyToObjects
        [TestMethod]
        public void SingleValuesCopyToObjects()
        {
            ClientRef clientRef1 = ClientRef.From("ASDF12345");
            ClientRef copyFrom = new ClientRef();
            clientRef1.CopyTo(copyFrom);

            Assert.AreEqual<ClientRef>(clientRef1, copyFrom);
        }

        [TestMethod]
        public void MultiValuesCopyToObjects()
        {
            Address address1 = Address.From(("Dorfstrasse 1", "Musterdorf", Postcode.From("67999")));
            Address copyFrom = null; // new Address();
            address1.CopyTo(copyFrom);

            Assert.AreEqual<Address>(address1, copyFrom);
        }
        #endregion CopyToObjects

        #region Valid Single Value
        [TestMethod]
        public void ValidSingleValuedExample()
        {
            try
            {
                ValidatedClientRef.From("");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentException));
                Assert.IsTrue(ex.Message == "Der Wert darf nicht leer sein!");
            }
        }
        #endregion Valid Single Value

        #region TryValidation, Returns Value Object by true
        [TestMethod]
        public void TryValidateReturnsFalse()
        {
            bool isValid = TryValidateClientRef.TryFrom("", out TryValidateClientRef valueObject);

            Assert.IsFalse(isValid);
            Assert.IsNull(valueObject);
        }

        [TestMethod]
        public void TryValidateReturnsTrue()
        {
            bool isValid = TryValidateClientRef.TryFrom("something", out TryValidateClientRef valueObject);

            Assert.IsTrue(isValid);
            Assert.IsNotNull(valueObject);
            Assert.AreEqual("something", valueObject.Value);
        }
        #endregion TryValidation, Returns Value Object by true

        private class ClientRef : ValueOfBase<string, ClientRef> { }

        private class Postcode : ValueOfBase<string, Postcode> { }

        private class Address : ValueOfBase<(string firstLine, string secondLine, Postcode postcode), Address> { }

        private class CaseInsensitiveAddress : ValueOfBase<(string firstLine, string secondLine, Postcode postcode), Address>
        {
            protected override bool Equals(ValueOfBase<(string firstLine, string secondLine, Postcode postcode), Address> other)
            {
                var aa = this.Value.firstLine.ToLower();
                var bb = this.Value.secondLine.ToLower();
                var cc = this.Value.postcode;
                var ddValue = (aa, bb, cc);

                var ee = other.Value.firstLine.ToLower();
                var ff = other.Value.secondLine.ToLower();
                var gg = this.Value.postcode;
                var hhOhter = (aa, bb, cc);

                return EqualityComparer<(string, string, Postcode)>.Default.Equals(ddValue, hhOhter);
            }

            public override int GetHashCode()
            {
                var aa = this.Value.firstLine.ToLower();
                var bb = this.Value.secondLine.ToLower();
                var cc = this.Value.postcode;
                var ddValue = (aa, bb, cc);
                return EqualityComparer<(string, string, Postcode)>.Default.GetHashCode(ddValue);
            }

        }

        public class CaseInsensitiveClientRef : ValueOfBase<string, CaseInsensitiveClientRef>
        {
            protected override bool Equals(ValueOfBase<string, CaseInsensitiveClientRef> other)
            {
                return EqualityComparer<string>.Default.Equals(this.Value.ToLower(), other.Value.ToLower());
            }

            public override int GetHashCode()
            {
                return EqualityComparer<string>.Default.GetHashCode(this.Value.ToLower());
            }
        }

        public class ValidatedClientRef : ValueOfBase<string, ValidatedClientRef>
        {
            protected override void Validate()
            {
                if (string.IsNullOrWhiteSpace(this.Value))
                {
                    throw new ArgumentException("Der Wert darf nicht leer sein!");
                }
            }
        }

        public class TryValidateClientRef : ValueOfBase<string, TryValidateClientRef>
        {
            protected override bool TryValidate()
            {
                return !string.IsNullOrWhiteSpace(this.Value);
            }
        }
    }
}