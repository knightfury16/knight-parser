namespace Knight.ParserCore.Tokenizer;

internal class VariableEnumeratorToken : Token
{
    public IReaderContext Context { get; }
    public override TokenType Type => TokenType.VaribaleEnumerator;
    public override string Value { get; }

    public VariableEnumeratorToken(string value, IReaderContext context)
    {
        Value = value;
        Context = context;
    }
}
