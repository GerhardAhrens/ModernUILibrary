namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Result_Test
    {

        [TestMethod]
        public void ResultSuccessNull()
        {
            Result<List<string>> textList = GenerateListWithResultNull();
            List<string> liste = textList.Value;

            Assert.IsNull(textList.Value);
            Assert.IsNull(liste);
            Assert.IsTrue(textList.ResultState == null);
        }

        [TestMethod]
        public void ResultSuccessNotNull()
        {
            Result<List<string>> textList = GenerateListWithResultNotNull();
            List<string> liste = textList.Value;

            Assert.IsNotNull(textList.Value);
            Assert.IsNotNull(liste);
            Assert.IsTrue(liste.Count > 0);
            Assert.IsTrue(textList.ResultState == true);
        }

        [TestMethod]
        public void ResultFail()
        {
            Result<List<string>> textList = GenerateListWithResultFail();
            List<string> liste = textList.Value;

            Assert.IsNull(textList.Value);
            Assert.IsNull(liste);
            Assert.IsTrue(textList.Exception is Exception);
            Assert.IsTrue(textList.Success == false);
            Assert.IsTrue(textList.ResultState == null);
        }

        private static Result<List<string>> GenerateListWithResultNull()
        {
            return Result<List<string>>.SuccessResult(null, null,0);
        }

        private static Result<List<string>> GenerateListWithResultNotNull(int count = 10)
        {
            List<string> result = new List<string>();

            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                result.Add(GenerateCoupon(10, rnd));
            }

            return Result<List<string>>.SuccessResult(result,"Mit Werten", true,1);
        }

        private static Result<List<string>> GenerateListWithResultFail(int count = 10)
        {
            List<string> result = null;

            try
            {
                result = new List<string>();
                result = null;
                Random rnd = new Random();
                for (int i = 0; i < count; i++)
                {
                    result.Add(GenerateCoupon(10, rnd));
                }
            }
            catch (Exception ex)
            {
                return Result<List<string>>.FailureResult(ex);
            }

            return Result<List<string>>.SuccessResult(result, "Mit Werten", true, 1);
        }

        private static Result<List<string>> GenerateListWithResultFalse(int count = 10)
        {
            List<string> result = new List<string>();

            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                result.Add(GenerateCoupon(10, rnd));
            }

            return Result<List<string>>.SuccessResult(result, false);
        }

        private static Result<List<string>> GenerateListWithResultTrue(int count = 10)
        {
            List<string> result = new List<string>();

            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                result.Add(GenerateCoupon(10, rnd));
            }

            return Result<List<string>>.SuccessResult(result, true);
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
