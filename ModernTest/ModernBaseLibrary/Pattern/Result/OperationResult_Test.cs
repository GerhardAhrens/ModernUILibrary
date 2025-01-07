namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OperationResult_Test
    {
        [TestMethod]
        public void OperationResultSuccessTrue()
        {
            OperationResult<List<string>> textList = GenerateListWithResultTrue();
            List<string> liste = textList.Result;

            Assert.IsNotNull(textList.Result);
            Assert.IsNotNull(liste);
            Assert.IsTrue(liste.Count > 0);
            Assert.IsTrue((bool)textList.OperationResultState);
        }

        [TestMethod]
        public void OperationResultSuccessFalse()
        {
            OperationResult<List<string>> textList = GenerateListWithResultFalse();
            List<string> liste = textList.Result;

            Assert.IsNotNull(textList.Result);
            Assert.IsNotNull(liste);
            Assert.IsTrue(liste.Count > 0);
            Assert.IsFalse((bool)textList.OperationResultState);
        }

        [TestMethod]
        public void OperationResultSuccessNull()
        {
            OperationResult<List<string>> textList = GenerateListWithResultNull();
            List<string> liste = textList.Result;

            Assert.IsNotNull(textList.Result);
            Assert.IsNotNull(liste);
            Assert.IsTrue(liste.Count > 0);
            Assert.IsTrue(textList.OperationResultState == null);
        }

        [TestMethod]
        public void OperationResultSuccessTime()
        {
            OperationResult<List<string>> textList = GenerateListWithResultTime();
            List<string> liste = textList.Result;

            Assert.IsNotNull(textList.Result);
            Assert.IsNotNull(liste);
            Assert.IsTrue(liste.Count > 0);
            Assert.IsTrue(textList.OperationResultTime < DateTime.Now);
        }

        [TestMethod]
        public void OperationResultFailure()
        {
            OperationResult<List<string>> textList = GenerateListWithFailure();
            List<string> liste = textList.Result;

            Assert.IsFalse(textList.Success);

            if (textList.Success == false)
            {
                Exception ex = textList.Exception;
                if (ex is NullReferenceException)
                {
                    Assert.IsTrue(ex is NullReferenceException);
                }
            }

            Assert.IsNull(liste);
        }

        private static OperationResult<List<string>> GenerateListWithResultFalse(int count = 10)
        {
            List<string> result = new List<string>();

            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                result.Add(GenerateCoupon(10, rnd));
            }

            return OperationResult<List<string>>.SuccessResult(result, false);
        }

        private static OperationResult<List<string>> GenerateListWithResultTrue(int count = 10)
        {
            List<string> result = new List<string>();

            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                result.Add(GenerateCoupon(10, rnd));
            }

            return OperationResult<List<string>>.SuccessResult(result, true);
        }

        private static OperationResult<List<string>> GenerateListWithResultNull(int count = 10)
        {
            List<string> result = new List<string>();

            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                result.Add(GenerateCoupon(10, rnd));
            }

            return OperationResult<List<string>>.SuccessResult(result, null);
        }

        private static OperationResult<List<string>> GenerateListWithResultTime(int count = 10)
        {
            List<string> result = new List<string>();

            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                result.Add(GenerateCoupon(10, rnd));
            }

            return OperationResult<List<string>>.SuccessResult(result, new DateTime(2019,8,19,9,20,0));
        }

        private static OperationResult<List<string>> GenerateListWithFailure(int count = 10)
        {
            List<string> result = new List<string>();
            Random rnd = new Random();

            try
            {
                result = null;
                for (int i = 0; i < count; i++)
                {
                    result.Add(GenerateCoupon(10, rnd));
                }
            }
            catch (Exception ex)
            {
                return OperationResult<List<string>>.Failure(ex, false);
            }

            return OperationResult<List<string>>.SuccessResult(result, new DateTime(2019, 8, 19, 9, 20, 0));
        }

        private static string GenerateCoupon(int length, Random random)
        {
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

    }
}
