namespace Knight.ParserCore.Tokenizer;

internal class VariableEnumeratorToken : Token
{
    public override TokenType Type => TokenType.VaribaleEnumerator;
    public override string Value { get; }

    public VariableEnumeratorToken(string value, IReaderContext context) : base(context)
    {
        Value = value;
    }
}
