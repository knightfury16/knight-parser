namespace Knight.ParserCore.Tokenizer;

public class BlockWordToken : Token
{
    public IReaderContext Context { get; }
    public override TokenType Type => TokenType.BlockWord;
    public override string Value { get; }

    public BlockWordToken(string value, IReaderContext context)
    {
        Value = value;
        Context = context;
    }

    public static Token BlockWordTokenizer(ExtendedStringReader sourceReader)
    {

    }
}
