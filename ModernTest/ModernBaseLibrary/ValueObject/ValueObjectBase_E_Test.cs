namespace ModernTest.ModernBaseLibrary.CoreBase
{
    using System;

    using global::ModernBaseLibrary.CoreBase;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ValueObjectBase_E_Test
    {
        [TestMethod]
        public void User_CreateNull()
        {
            var user = new User(string.Empty).Create();
            Assert.IsNull(user);
        }

        [TestMethod]
        public void User_CreateWithValid()
        {
            var user = new User("ahrens").TryValidate();
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void User_CreateWithAllArgs()
        {
            var user = new User("ahrens","gerhard.ahrens@lifeprojects.de").TryValidate();
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void User_CreateWithAllArgsWrongEMail()
        {
            var user = new User("ahrens", "gerhard.ahrens@Lifeprojects.de-de").TryValidate();
            Assert.IsNull(user);
        }
    }
}