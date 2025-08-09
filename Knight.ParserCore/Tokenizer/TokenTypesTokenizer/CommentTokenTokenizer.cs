
using System.Text;
using Knight.ParserCore.Utils;

namespace Knight.ParserCore.Tokenizer.TokenTypesTokenizer;


internal class CommentTokenTokenizer : ITokenTypeTokenizer
{
    private static HashSet<char> ValidCommentStartCharacters = "!".ToHashSet();

    public Token? Tokenzie(int node, ExtendedStringReader sourceReader)
    {
        var context = sourceReader.GetContext();

        if (!IsValid(node.ToChar()))
        {
            return null;
        }

        var accumulator = new StringBuilder();

        while (true)
        {
            if (node == -1) throw new TokenizerException("Reach end of the file while tokenizing comment");

            accumulator.Append((char)node);

            if ((char)sourceReader.Peek() == '}')
            {
                break;
            }

            node = sourceReader.Read();
        }

        return new CommentToken(accumulator.ToString(), context);
    }

    private bool IsValid(char node) => ValidCommentStartCharacters.Contains(node);
}
