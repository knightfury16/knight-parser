namespace Knight.ParserCore.Tokenizer;


internal class VariableToken : Token
{
    public IReaderContext Context { get; }
    public override TokenType Type => TokenType.Variable;
    public override string Value { get; }


    public VariableToken(string value, IReaderContext context)
    {
        Value = value;
        Context = context;
    }
}
