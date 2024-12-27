/*
 * <copyright file="Object_Coalesce_Test.cs" company="Lifeprojects.de">
 *     Class: Object_Coalesce_Test
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.05.2023</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernTest.ModernBaseLibrary
{
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Object_Coalesce_Test
    {

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Object_Coalesce_Test"/> class.
        /// </summary>
        public Object_Coalesce_Test()
        {
        }

        [TestMethod]
        public void CoalesceFromObject()
        {
            object thisNull = null;
            object thisNotNull = "Fizz";

            Assert.AreEqual(thisNull.Coalesce(null, null, "Fizz", "Buzz"), "Fizz");
            Assert.AreEqual(thisNull.Coalesce(null, "Fizz", null, "Buzz"), "Fizz");
            Assert.AreEqual(thisNotNull.Coalesce(null, null, null, "Buzz"), "Fizz");
        }

        [TestMethod]
        public void CoalesceOrDefault()
        {
            // Varable
            object nullObject = null;

            // Type
            object @thisNull = null;
            object @thisNotNull = "Fizz";

            // Exemples
            object result1 = @thisNull.CoalesceOrDefault(nullObject, nullObject, "Buzz");
            object result2 = @thisNull.CoalesceOrDefault(() => "Buzz", null, null); 
            object result3 = @thisNull.CoalesceOrDefault((x) => "Buzz", null, null); 
            object result4 = @thisNotNull.CoalesceOrDefault(nullObject, nullObject, "Buzz");

            // Unit Test
            Assert.AreEqual("Buzz", result1);
            Assert.AreEqual("Buzz", result2);
            Assert.AreEqual("Buzz", result3);
            Assert.AreEqual("Fizz", result4);
        }
    }
}
