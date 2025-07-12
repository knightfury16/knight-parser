namespace Knight.ParserCore.Tokenizer.TokenTypesTokenizer;


internal interface ITokenTypeTokenizer
{
    public Token Tokenzie(int node, ExtendedStringReader sourceReader);
}
