
namespace Knight.ParserCore.Tokenizer;

internal class CommentToken : Token
{
    public override TokenType Type => TokenType.Comment;
    public override string Value { get; }

    public CommentToken(string value, IReaderContext context) : base(context)
    {
        Value = value;
    }
}
