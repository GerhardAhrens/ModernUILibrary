//-----------------------------------------------------------------------
// <copyright file="AssertMyExtensions.cs" company="Lifeprojects.de">
//     Class: AssertMyExtensions
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.09.2022 14:52:20</date>
//
// <summary>
// Extension Class for 
// </summary>
//-----------------------------------------------------------------------

namespace EasyPrototypingTest
{
    using UnitTest = Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;
    using System.Collections;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static partial class AssertExtension
    {
        public static T Throws<T>(Action expressionUnderTest,
                                  string exceptionMessage = "Expected exception has not been thrown by target of invocation."
                                 ) where T : Exception
        {
            try
            {
                expressionUnderTest();
            }
            catch (T exception)
            {
                return exception;
            }

            Assert.Fail(exceptionMessage);
            return null;
        }
    }
}
