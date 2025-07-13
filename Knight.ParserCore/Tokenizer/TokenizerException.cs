namespace Knight.ParserCore.Tokenizer;


public class TokenizerException : Exception
{
    public TokenizerException(string message) : base(message) { }
    public TokenizerException(string message, Exception innerException) : base(message, innerException) { }
}
