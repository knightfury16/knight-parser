
namespace Knight.ParserCore.Tokenizer;

public class StartExpressionToken : Token
{
    public IReaderContext Context { get; }
    public override TokenType Type => TokenType.StartExpression;
    public override string Value { get; }

    public StartExpressionToken(string value, IReaderContext context)
    {
        Value = value;
        Context = context;
    }
}
