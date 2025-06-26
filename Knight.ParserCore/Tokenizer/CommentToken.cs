
namespace Knight.ParserCore.Tokenizer;

public class CommentToken : Token
{
    public IReaderContext Context { get; }
    public override TokenType Type => TokenType.Comment;
    public override string Value { get; }

    public CommentToken(string value, IReaderContext context)
    {
        Value = value;
        Context = context;
    }
}
