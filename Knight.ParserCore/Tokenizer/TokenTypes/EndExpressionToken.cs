namespace Knight.ParserCore.Tokenizer;

internal class EndExpressionToken : Token
{
    public static readonly string EndExpressionString = "}}";
    public IReaderContext Context { get; }
    public override TokenType Type => TokenType.EndExpression;
    public override string Value { get; }

    public EndExpressionToken(IReaderContext context)
    {
        Value = EndExpressionString;
        Context = context;
    }
}
