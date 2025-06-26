namespace Knight.ParserCore.Tokenizer;


public abstract class Token
{
    public abstract TokenType Type { get; }

    public abstract string Value { get; }

    public sealed override string ToString()
    {
        return $"{Type.ToString()}: {Value}";
    }
}
