namespace Knight.ParserCore.Tokenizer;

internal class EndExpressionToken : Token
{
    public IReaderContext Context { get; }
    public override TokenType Type => TokenType.EndExpression;
    public override string Value { get; }

    public EndExpressionToken(string value, IReaderContext context)
    {
        Value = value;
        Context = context;
    }
}
