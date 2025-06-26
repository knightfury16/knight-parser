namespace Knight.ParserCore.Tokenizer;


public class StaticToken : Token
{
    public IReaderContext Context { get; set; }
    public override TokenType Type => TokenType.Static;
    public override string Value { get; }

    public StaticToken(string value, IReaderContext context)
    {
        Context = context;
        Value = value;
    }

}
