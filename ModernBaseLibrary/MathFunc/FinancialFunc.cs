//-----------------------------------------------------------------------
// <copyright file="FinancialFunc.cs" company="Lifeprojects.de">
//     Class: FinancialFunc
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>27.06.2025 13:23:14</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.MathFunc
{
    public static class FinancialFunc
    {
        /// <summary>
        /// Ermittelt die prozentuale Differenz zwischen zwei Werten
        /// </summary>
        /// <param name="netSales"></param>
        /// <param name="totalAssets"></param>
        /// <returns></returns>
        public static double CalcAssetTurnover(double netSales, double totalAssets)
        {
            return netSales / totalAssets;
        }

        public static double CalcAverageCollectionPeriod(double accountsReceivable, double annualCreditSales)
        {
            return accountsReceivable / (annualCreditSales / 365);
        }
    }
}
