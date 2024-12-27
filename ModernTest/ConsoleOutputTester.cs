namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.IO;

    /// <summary>
    /// Ergebnis auf die Console ausgeben
    /// </summary>
    /// <example>
    /// using var consoleTester = new ConsoleOutputTester();
    /// 
    /// Output von einem Consolenprogramm
    /// 
    /// var response = consoleTester.GetOutput();
    /// </example>
    public sealed class ConsoleOutputTester : IDisposable
    {
        private readonly StringWriter _consoleOutput = new();
        private readonly TextWriter _originalOutput;

        public ConsoleOutputTester()
        {
            _originalOutput = System.Console.Out;
            System.Console.SetOut(_consoleOutput);
        }

        public void Dispose()
        {
            System.Console.SetOut(_originalOutput);
            _consoleOutput.Dispose();
        }

        public string GetOutput() => _consoleOutput.ToString();
    }
}
