
namespace Knight.ParserCore.Tokenizer;

internal class BlockWordToken : Token
{
    public override TokenType Type => TokenType.BlockWord;
    public override string Value { get; }

    public BlockWordToken(string value, IReaderContext context) : base(context)
    {
        Value = value;
    }
}
