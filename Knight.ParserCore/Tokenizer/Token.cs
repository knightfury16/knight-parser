namespace Knight.ParserCore.Tokenizer;


public abstract class Token
{
    //I could have context on the base class,
    //but do i need context in all the tokne

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
}
