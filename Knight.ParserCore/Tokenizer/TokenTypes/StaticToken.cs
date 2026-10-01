namespace Knight.ParserCore.Tokenizer;


internal class StaticToken : Token
{
    public override TokenType Type => TokenType.Static;
    public override string Value { get; }

    public StaticToken(string value, IReaderContext context) : base(context)
    {
        Value = value;
    }

}
