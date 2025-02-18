namespace ModernBaseLibrary.Text
{
    public class SqlLexicalAnalyzer : ILexicalAnalyzer
    {        
        private Dictionary<string, FSM> _fsms;
        private Dictionary<string, IFSMBuilder> _builders;

        public SqlLexicalAnalyzer()
        {
            _fsms = new Dictionary<string, FSM>();

            _builders = new Dictionary<string, IFSMBuilder>
            {
                { Category.OPERATOR.ToString(), new OperatorFSMBuilder() },
                { Category.KEYWORD.ToString(), new KeywordFSMBuilder() },
                { Category.IDENTIFIER.ToString(), new IdentifierFSMBuilder() },
                { Category.NUMBER.ToString(), new NumberFSMBuilder() },
                { Category.LITERAL.ToString(), new LiteralFSMBuilder() },
                { Category.PUNCTUATION.ToString(), new PunctuationFSMBuilder() },
                { Category.ALL.ToString(), new AllFSMBuilder() }
            };

            foreach (var category in Enum.GetNames(typeof(Category)))
            {
                var fsmBuilder = _builders[category];
                _fsms.Add(category, fsmBuilder.Build());
            }
        }

        public bool TryAnalyze(string codeToScan, out List<LAToken> tokens, out List<string> errors)
        {
            tokens = new List<LAToken>(); errors = new List<string>();

            var partsSql = ParsePart(codeToScan.Trim());
            foreach (var split in partsSql)
            {
                var found = false;
                foreach (var category in _fsms.Keys)
                {
                    var fsm = _fsms[category];
                    if (fsm.Simulate(split))
                    {
                        tokens.Add(new LAToken() { Lexeme = split, Category = (Category)Enum.Parse(typeof(Category), category) });
                        found = true;
                        break;
                    }
                }

                if (!found) errors.Add($"The lexeme {split} is not recognized by the language.");
            }
            
            return !errors.Any();
        }

        private string[] ParsePart(string part)
        {
            var specials = new List<string>()
            {
                " ", ",", ".", "<", ">", "="
            };

            var tempParts = new List<string>() { part };
            foreach (var special in specials)
            {
                var temps = new string[tempParts.Count]; tempParts.CopyTo(temps, 0);
                tempParts = new List<string>();
                foreach (var t in temps)
                {
                    var splits = t.Trim().Split(special, StringSplitOptions.None);
                    if (splits.Length == 1) tempParts.Add(splits[0]);
                    else
                    {
                        foreach (var split in splits)
                        {
                            tempParts.Add(split);
                            tempParts.Add(special);
                        }

                        tempParts.RemoveAt(tempParts.Count - 1);
                    }

                    tempParts = tempParts.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                }
            }            

            return tempParts.ToArray();
        }
    }
}
