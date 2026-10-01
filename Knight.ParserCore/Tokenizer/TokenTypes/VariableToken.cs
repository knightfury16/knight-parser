namespace Knight.ParserCore.Tokenizer;


internal class VariableToken : Token
{
    public override TokenType Type => TokenType.Variable;
    public override string Value { get; }


    public VariableToken(string value, IReaderContext context) : base(context)
    {
        Value = value;
    }
}
