namespace Knight.ParserCore.Tokenizer;

public class BlockAliasToken : Token
{
    public IReaderContext Context { get; }
    public override TokenType Type => TokenType.BlockAlias;
    public override string Value { get; }

    public BlockAliasToken(string value, IReaderContext context)
    {
        Value = value;
        Context = context;
    }
}
