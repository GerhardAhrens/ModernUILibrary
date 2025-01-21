namespace ModernTest.ModernBaseLibrary.Core
{
    using System.IO;

    using global::ModernBaseLibrary.Core.IO;
    using global::ModernBaseLibrary.Cryptography;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;

    [TestClass]
    public class ValidatableObject_Test : BaseTest
    {
        public string TestDirPath => TestContext.TestRunDirectory;

        public string TempDirPath => Path.Combine(TestDirPath, "Temp");

        [TestInitialize]
        public void SetUp()
        {
            Directory.CreateDirectory(TempDirPath);
        }

        [TestCleanup]
        public void Clean()
        {
            if (Directory.Exists(TempDirPath))
            {
                Directory.Delete(TempDirPath, true);
            }
        }

        [TestMethod]
        public void ValidOK()
        {
            var result = new List<ValidationResult>();
            bool validateAllProperties = false;

            var employee = new TestValidEmployee()
            {
                Id = -1,
                Name = "Gerhard Ahrens",
                Status = true,
                DesignationId = 1
            };

            bool isValid = Validator.TryValidateObject(employee, new ValidationContext(employee, null, null), result, validateAllProperties);

            if (isValid == true && result.Count == 0)
            {
                base.Trace("Kein Fehler gefunden!");
            }
            else
            {
                if (result != null && result.Count > 0)
                {
                    foreach (ValidationResult validItem in result)
                    {
                        base.Trace(validItem.ErrorMessage);
                    }
                }
            }
        }

        [TestMethod]
        public void ValidWrongContent()
        {
            var result = new List<ValidationResult>();
            bool validateAllProperties = false;

            var employee = new TestValidEmployee()
            {
                Id = -1,
                Name = string.Empty,
                Status = true,
                DesignationId = 1
            };

            bool isValid = Validator.TryValidateObject(employee, new ValidationContext(employee, null, null), result, validateAllProperties);

            if (isValid == true && result.Count == 0)
            {
                base.Trace("Kein Fehler gefunden!");
            }
            else
            {
                if (result != null && result.Count > 0)
                {
                    foreach (ValidationResult validItem in result)
                    {
                        base.Trace(validItem.ErrorMessage);
                    }
                }
            }
        }

    }

    internal class TestValidEmployee : IValidatableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int DesignationId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Id <= 0)
            {
                yield return new ValidationResult("Employee Id is a required filed!", new[] { "Id" });
            }

            if (string.IsNullOrEmpty(this.Name))
            {
                yield return new ValidationResult("Employee Name is a required field!", new[] { "Name" });
            }
        }
    }
}