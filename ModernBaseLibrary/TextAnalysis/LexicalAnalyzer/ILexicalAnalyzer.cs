namespace ModernBaseLibrary.Text
{
    using System.Collections.Generic;

    public interface ILexicalAnalyzer
    {
        bool TryAnalyze(string codeToScan, out List<LAToken> tokens, out List<string> errors); 
    }
}
