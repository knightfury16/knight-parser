using Knight.ParserCore.Utils;

namespace Knight.ParserCore.Tokenizer.TokenTypesTokenizer;


internal class EndTokenTokenizer : ITokenTypeTokenizer
{
    public Token? Tokenzie(int node, ExtendedStringReader sourceReader)
    {
        ArgumentNullException.ThrowIfNull(sourceReader);

        if (node <= 0)
        {
            return null;
        }

        if (node.ToChar() != '}')
        {
            return null;
        }

        if (node.ToChar() == '}' && sourceReader.Peek().ToChar() == '}')
        {
            sourceReader.Read(); //pointer at second }
            sourceReader.Read();// moving past the second }
            return Token.EndExpression(sourceReader.GetContext());
        }

        return null;
    }
}
