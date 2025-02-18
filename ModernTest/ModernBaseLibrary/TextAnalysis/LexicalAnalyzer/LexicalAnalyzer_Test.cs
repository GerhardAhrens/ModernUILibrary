namespace ModernTest.ModernBaseLibrary.Text
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using global::ModernBaseLibrary.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LexicalAnalyzer
    {
        [TestInitialize]
        public void TestSetUp()
        {
        }

        [TestMethod]
        public void LexicalAnalyzer_SQL_A()
        {
            var sql = "SELECT * FROM Customers";

            var sqlAnalyzer = new SqlLexicalAnalyzer();
            bool res = sqlAnalyzer.TryAnalyze(sql, out var tokens, out var errors);
            if (res == true)
            {
                // Parse with tokens
                string resultAnalyzer = tokens.Select(x => x.ToString()).Aggregate((x1, x2) => $"{x1}{Environment.NewLine}{x2}");
            }
            else
            {
                // Result Analyzer error
            }
        }

        [TestMethod]
        public void LexicalAnalyzer_SQL_B()
        {
            var sql = "SELECT c.Name, c.Age FROM Customers c WHERE c.Id LIKE '%john%'";

            var sqlAnalyzer = new SqlLexicalAnalyzer();
            bool res = sqlAnalyzer.TryAnalyze(sql, out var tokens, out var errors);
            if (res == true)
            {
                // Parse with tokens
                string resultAnalyzer = tokens.Select(x => x.ToString()).Aggregate((x1, x2) => $"{x1}{Environment.NewLine}{x2}");
            }
            else
            {
                // Result Analyzer error
            }
        }

        [TestMethod]
        public void LexicalAnalyzer_SQL_C()
        {
            string sql = "SELECT c.Name, c.Age FROM Customers c WHERE c.Id LIKE '%john%' order by c.Age";

            var sqlAnalyzer = new SqlLexicalAnalyzer();
            bool res = sqlAnalyzer.TryAnalyze(sql, out var tokens, out var errors);
            if (res == true)
            {
                // Parse with tokens
                string resultAnalyzer = tokens.Select(x => x.ToString()).Aggregate((x1, x2) => $"{x1}{Environment.NewLine}{x2}");
            }
            else
            {
                // Result Analyzer error
            }
        }
    }
}