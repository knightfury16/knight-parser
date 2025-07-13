namespace Knight.ParserCore.Tokenizer.TokenTypesTokenizer;


//I can not adhere the static token tokenizer to the generic type
//design problem
internal class StaticTokenTokenizer : ITokenTypeTokenizer
{
    public Token? Tokenzie(int node, ExtendedStringReader sourceReader)
    {
        throw new NotImplementedException();
    }
}
