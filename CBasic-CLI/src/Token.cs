namespace CBasicCLI;

public enum TokenType
{
    Identifier,
    Number,
    String,
    Operator,
    Keyword,
    Equals,
    NewLine,
    EndOfFile
}

public sealed class Token
{
    public TokenType Type;
    public string Value;
    public int Line;
    public int Column;

    public Token(TokenType type, string value, int line, int column)
    {
        Type = type;
        Value = value;
        Line = line;
        Column = column;
    }

    public override string ToString()
    {
        return $"{Type}('{Value}') at {Line} : {Column}";
    }
}
