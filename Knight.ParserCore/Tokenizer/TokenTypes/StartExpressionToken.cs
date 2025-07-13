
namespace Knight.ParserCore.Tokenizer;

internal class StartExpressionToken : Token
{
    public static readonly string StartExpressionString = "{{";
    public IReaderContext Context { get; }
    public override TokenType Type => TokenType.StartExpression;
    public override string Value { get; }

    public StartExpressionToken(IReaderContext context)
    {
        Value = StartExpressionString;
        Context = context;
    }
}
