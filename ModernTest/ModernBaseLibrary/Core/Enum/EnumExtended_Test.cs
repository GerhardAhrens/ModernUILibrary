//-----------------------------------------------------------------------
// <copyright file="EnumExtended_Test.cs" company="Lifeprojects.de">
//     Class: EnumExtended_Test
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.01.2023</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnumExtended_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumExtended_Test"/> class.
        /// </summary>
        public EnumExtended_Test()
        {
        }

        [TestMethod]
        public void SetValue()
        {
            PaymentType p = PaymentType.CreditCard;

            var aa = PaymentType.GetAll<PaymentType>();

            PaymentType outVar;
            var bb = PaymentType.TryGetFromValueOrName("None", out outVar);
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

    public abstract class PaymentType : EnumExtended
    {
        public static readonly PaymentType None = new NoneType();

        public static readonly PaymentType DebitCard = new DebitCardType();

        public static readonly PaymentType CreditCard = new CreditCardType();

        private PaymentType(int value, string name = null) : base(value, name)
        {
        }

        public abstract string Code { get; }

        private class NoneType : PaymentType
        {
            public NoneType() : base(0, "None")
            {
            }

            public override string Code => "NO";
        }

        private class DebitCardType : PaymentType
        {
            public DebitCardType() : base(1, "DebitCard")
            {
            }

            public override string Code => "DC";
        }

        private class CreditCardType : PaymentType
        {
            public CreditCardType() : base(2, "CreditCard")
            {
            }

            public override string Code => "CC";
        }
    }
}
