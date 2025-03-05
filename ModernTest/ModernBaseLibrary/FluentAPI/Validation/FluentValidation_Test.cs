namespace ModernTest.ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Diagnostics;
    using System.Text;

    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.FluentAPI;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FluentValidation_Test
    {
        private readonly Contact cc = null;

        public FluentValidation_Test()
        {
            cc = new Contact();
            cc.LastName = "XX"; // string.Empty;
            cc.FirstName = "Gerhard";
            cc.EMail = "developer@lifeprojects.de";
            cc.Birthday = new DateTime(1, 1, 1);
            cc.AktiveDatum = DateTime.Now;
            cc.Age = 0;
            cc.State = Status.Aktive;
            cc.Gruppe = "X";
        }

        [TestMethod]
        public void ValidationMessageFormatter()
        {
            string result = string.Empty;

            using (ValidationMessageFormatter vmf = new ValidationMessageFormatter())
            {
                vmf.PropertyName = "Age";
                vmf.RawMessage = "Zahl: Der Wert muß zwischen {von} und {bis} liegen!";
                vmf.AppendArgument("von", 18);
                vmf.AppendArgument("bis", 99);
                result = vmf.ToString();
            }

            Assert.That.StringEquals(result, "Zahl: Der Wert muß zwischen 18 und 99 liegen!");
        }

        [TestMethod]
        public void IsEmpty_Result_IsTrue()
        {
            ValidationBuilder validContent = FluentValidation<Contact>.This(cc)
                .RuleFor(x => x.LastName)
                .IsEmpty()
                .Message("Der Nachname darf nicht leer sein!");
            Assert.IsTrue(validContent.ValidResult);
        }

        [DebuggerDisplay("FirstName={FirstName};Birthday={Birthday},PostalCode={PostalCode}")]
        private class Contact 
        {
            public Guid Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public int PostalCode { get; set; }

            public string City { get; set; }

            public DateTime Birthday { get; set; }

            public bool IsPrivate { get; set; }

            public byte[] ContactPhoto { get; set; }

            public int Age { get; set; }

            public string EMail { get; set; }

            public Status State { get; set; }

            public DateTime AktiveDatum { get; set; }

            public string Gruppe { get; set; }
        }

        private enum Status : int
        {
            None = 0,
            Aktive = 1,
            InAktiv = 2
        }
    }
}
