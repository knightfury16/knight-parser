using Knight.ParserCore.Utils;

namespace Knight.ParserCore.Tokenizer.TokenTypesTokenizer;


internal class StartTokenTokenizer : ITokenTypeTokenizer
{
    public Token? Tokenzie(int node, ExtendedStringReader sourceReader)
    {
        ArgumentNullException.ThrowIfNull(sourceReader);

        if (node <= 0)
        {
            return null;
        }

        if (node.ToChar() != '{')
        {
            return null;
        }

        if (node.ToChar() == '{' && sourceReader.Peek().ToChar() == '{')
        {
            //consuming the second '{'
            sourceReader.Read();
            return Token.StartExpression(sourceReader.GetContext());
        }

        return null;
    }
}
