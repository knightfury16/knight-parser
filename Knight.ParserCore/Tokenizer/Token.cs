namespace Knight.ParserCore.Tokenizer;


internal abstract class Token
{
    //I could have context on the base class,
    //but do i need context in all the token

    public abstract TokenType Type { get; }

    public abstract string Value { get; }

    public sealed override string ToString()
    {
        return $"{Type.ToString()}: {Value}";
    }


    public static StaticToken Static(string value, IReaderContext context)
    {
        return new StaticToken(value, context);
    }

    public static StartExpressionToken StartExpression(string value, IReaderContext context)
    {
        return new StartExpressionToken(value, context);
    }

    public static StartExpressionToken StartExpression(IReaderContext context)
    {
        return new StartExpressionToken("{{", context);
    }

    public static EndExpressionToken EndExpression(string value, IReaderContext context)
    {
        return new EndExpressionToken(value, context);
    }

    public static EndExpressionToken EndExpression(IReaderContext context)
    {
        return new EndExpressionToken("}}", context);
    }

    public static BlockWordToken BlockWord(string value, IReaderContext context)
    {
        return new BlockWordToken(value, context);
    }
    public static BlockAliasToken BlockAlias(string value, IReaderContext context)
    {
        return new BlockAliasToken(value, context);
    }
    public static VariableToken Variable(string value, IReaderContext context)
    {
        return new VariableToken(value, context);
    }
}
