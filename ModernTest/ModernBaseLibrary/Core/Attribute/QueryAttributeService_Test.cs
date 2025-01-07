
namespace EasyPrototypingNET.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernBaseLibrary.Core;

    [TestClass]
    public class QueryAttributeService_Test 
    {
        [TestMethod]
        public void Read_NoAttribute()
        {
            IDictionary<string, object> attrSource = null;

            using (IQueryAttributeService qas = new QueryAttributeService())
            {
                attrSource = qas.GetExternAttributes<ExportFieldAttribute>(typeof(PersonModel));
            }

            Assert.IsNotNull(attrSource);
            Assert.IsTrue(attrSource.Count == 0);
        }

        [TestMethod]
        public void Read_SearchFilterAttribute()
        {
            IDictionary<string, object> attrSource = null;

            using (IQueryAttributeService qas = new QueryAttributeService())
            {
                attrSource = qas.GetExternAttributes<SearchFilterAttribute>(typeof(PersonModel));
            }

            Assert.IsNotNull(attrSource);
            Assert.IsTrue(attrSource.Count > 0);

            SearchFilterAttribute value = attrSource.Values.First() as SearchFilterAttribute;
            Assert.IsTrue(value.FieldName == "Name");
        }

        [TestMethod]
        public void Read_RequirementAttribute()
        {
            IDictionary<string, object> attrSource = null;

            using (IQueryAttributeService qas = new QueryAttributeService())
            {
                attrSource = qas.GetExternAttributes<RequirementAttribute>(typeof(PersonModel));
            }

            Assert.IsNotNull(attrSource);
            Assert.IsTrue(attrSource.Count == 1);

            RequirementAttribute value = attrSource.Values.First() as RequirementAttribute;
            Assert.IsTrue(value.Id == "R1002");
        }



        [Requirement("R1001", Status = RequirementStatus.Deferred)]
        private class PersonModel
        {
            [SearchFilter]
            [Requirement("R1002", Status = RequirementStatus.Deferred, Comment = "Nachträglich eingeführt")]
            public string Name { get; set; }

            public int Age { get; set; }

            [SearchFilter]
            public string LastName { get; set; }

            [SearchFilter]
            public int City { get; set; }

            public void Test()
            {
            }

            public void Test(string name)
            {
            }
        }
    }
}