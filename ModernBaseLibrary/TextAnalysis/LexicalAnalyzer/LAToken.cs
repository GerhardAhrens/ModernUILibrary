namespace ModernBaseLibrary.Text
{
    public enum Category { OPERATOR, KEYWORD, IDENTIFIER, PUNCTUATION, NUMBER, LITERAL, ALL }

    public class LAToken
    {
        public string Lexeme { get; set; }

        public Category Category { get; set; }

        public override string ToString()
        {
            return $"({Category.ToString()}|{Lexeme})";
        }
    }
}
