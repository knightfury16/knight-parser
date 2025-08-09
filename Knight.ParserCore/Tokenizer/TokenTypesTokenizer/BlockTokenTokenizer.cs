using System.Text;
using Knight.ParserCore.Utils;

namespace Knight.ParserCore.Tokenizer.TokenTypesTokenizer;


internal class BlockTokenTokenizer : ITokenTypeTokenizer
{

    private static readonly HashSet<char> BlockWordIdentifiers = "#".ToHashSet();

    public Token? Tokenzie(int node, ExtendedStringReader sourceReader)
    {

        var context = sourceReader.GetContext();

        if (!IsValidBlockWord(node.ToChar()))
        {
            return null;
        }

        //get rid of the space after # until the first character
        CleanSpaceInBetween(sourceReader);

        var accumulator = new StringBuilder();


        while (true)
        {
            //lets not include the # or white spcae in the token value
            node = sourceReader.Read();

            if (node == -1) throw new Exception("Reach end of the file while tokenizing blockword name");

            accumulator.Append(node.ToChar());

            if (sourceReader.Peek().ToChar() == '}' || char.IsWhiteSpace(sourceReader.Peek().ToChar()))
            {
                break;
            }
        }

        return new BlockWordToken(accumulator.ToString(), context);
    }

    private static void CleanSpaceInBetween(ExtendedStringReader sourceReader)
    {
        while (true)
        {
            if (char.IsWhiteSpace(sourceReader.Peek().ToChar())) sourceReader.Read();
            else break;
        }
    }

    private static bool IsValidBlockWord(char node) => BlockWordIdentifiers.Contains(node);
}
