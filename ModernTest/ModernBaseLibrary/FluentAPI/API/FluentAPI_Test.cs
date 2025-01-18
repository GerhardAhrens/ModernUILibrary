namespace ModernTest.ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Text;

    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.FluentAPI;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Zusammenfassungsbeschreibung für DataRepository_Test
    /// </summary>
    [TestClass]
    public class FluentAPI_Test
    {
        public FluentAPI_Test()
        {
            //
            // TODO: Konstruktorlogik hier hinzufügen
            //
        }

        [TestMethod]
        public void FluentAPICreateObject()
        {
            ContactBuilder contactBuilder = ContactBuilder.NewContact
                .SetName("Gerhard", "Ahrens")
                .SetTitle("Herr")
                .AtPosition("Softwarearchitekt");

            Assert.IsNotNull(contactBuilder);
            Assert.IsTrue(contactBuilder.GetType() == typeof(ContactBuilder));
        }

        [TestMethod]
        public void FluentAPICreateContentObject()
        {
            ContactBuilder contactBuilder = ContactBuilder.NewContact
                .SetName("Gerhard", "Ahrens")
                .SetTitle("Herr")
                .AtPosition("Softwarearchitekt");

            Contact contact = contactBuilder.Get();

            Assert.IsNotNull(contact);
            Assert.IsTrue(contact.GetType() == typeof(Contact));
        }

        [TestMethod]
        public void FluentAPIBaseCloneAndCompare()
        {
            var contactBuilder = ContactBuilder.NewContact
                .SetName("Gerhard", "Ahrens")
                .SetTitle("Herr")
                .AtPosition("Softwarearchitekt");

            Contact contact1 = contactBuilder.Get();
            Contact contact2 = ContactBuilder.ToClone(contact1);
            Assert.That.AreEqualValue<Contact>(contact1,contact2);

            bool isCompare1 = ContactBuilder.ToCompare(contact1, contact2);
            Assert.IsTrue(isCompare1);

            Contact contact3 = contactBuilder.WithSalary(5000).Get();
            bool isCompare2 = ContactBuilder.ToCompare(contact2, contact3);
            Assert.IsFalse(isCompare2);
        }

        private sealed class ContactBuilder : FluentAPIBase<Contact>
        {
            private readonly Contact contact;

            public ContactBuilder()
            {
                this.contact = new Contact();
            }

            public static ContactBuilder NewContact => new ContactBuilder();

            public ContactBuilder SetName(string firstname, string lastName)
            {
                this.contact.FirstName = firstname;
                this.contact.LastName = lastName;

                return this;
            }

            public ContactBuilder SetSolutation(string solutation)
            {
                this.contact.Solutation = solutation;

                return this;
            }

            public ContactBuilder SetTitle(string title)
            {
                this.contact.Title = title;

                return this;
            }

            public ContactBuilder SetBirthday(DateTime birthday)
            {
                this.contact.Birthday = birthday.DateOrDefault();

                return this;
            }

            public ContactBuilder AtPosition(string position)
            {
                this.contact.Position = position;

                return this;
            }

            public ContactBuilder WithSalary(decimal salary)
            {
                this.contact.Salary = salary;

                return this;
            }

            public override Contact Get()
            {
                return this.contact;
            }

            public override string ToString()
            {
                StringBuilder fullname = new StringBuilder();

                if (string.IsNullOrEmpty(this.contact.Title) == false)
                {
                    fullname.Append(this.contact.Title).Append(" ");
                }

                if (string.IsNullOrEmpty(this.contact.Solutation) == false)
                {
                    fullname.Append(this.contact.Solutation).Append(" ");
                }

                if (string.IsNullOrEmpty(this.contact.FirstName) == false)
                {
                    fullname.Append(this.contact.FirstName).Append(" ");
                }

                if (string.IsNullOrEmpty(this.contact.LastName) == false)
                {
                    fullname.Append(this.contact.LastName).Append(" ");
                }

                return fullname.ToString();
            }
        }

        private class Contact
        {
            public string Solutation { get; set; }

            public string Title { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }


            public DateTime Birthday { get; set; }

            public string Position { get; set; }

            public decimal Salary { get; set; }

            public override string ToString()
            {
                return $"Name: {this.FirstName} {this.LastName}, Position: {this.Position}, Salary: {this.Salary}";
            }
        }
    }
}
