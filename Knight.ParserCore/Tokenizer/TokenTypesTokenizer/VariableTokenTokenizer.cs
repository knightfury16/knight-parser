using System.Text;

namespace Knight.ParserCore.Tokenizer.TokenTypesTokenizer;


internal class VariableTokenTokenizer : ITokenTypeTokenizer
{

    private static HashSet<char> ValidVariableStartCharacter = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToHashSet();

    public Token? Tokenzie(int node, ExtendedStringReader sourceReader)
    {
        var context = sourceReader.GetContext();

        if (!IsValid((char)node))
        {
            return null;
        }

        var accumulator = new StringBuilder();

        while (true)
        {
            if (node == -1) throw new TokenizerException("Reach end of the file while tokenizing variable name");

            accumulator.Append((char)node);

            if ((char)sourceReader.Peek() == '}' || (char)sourceReader.Peek() == ' ')
            {
                break;
            }

            node = sourceReader.Read();
        }

        return new VariableToken(accumulator.ToString(), context);
    }

    private static bool IsValid(char node) => ValidVariableStartCharacter.Contains(node);
}

