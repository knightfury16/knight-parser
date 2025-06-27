namespace Knight.ParserCore.Tokenizer;

internal class BlockWordToken : Token
{
    public IReaderContext Context { get; }
    public override TokenType Type => TokenType.BlockWord;
    public override string Value { get; }

    public BlockWordToken(string value, IReaderContext context)
    {
        Value = value;
        Context = context;
    }

}
