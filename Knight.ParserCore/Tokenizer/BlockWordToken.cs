
using System.Text;

namespace Knight.ParserCore.Tokenizer;

internal class BlockWordToken : Token
{
    public IReaderContext Context { get; }
    public override TokenType Type => TokenType.BlockWord;
    public override string Value { get; }
    private static readonly HashSet<char> BlockWordIdentifiers = "#".ToHashSet();

    public BlockWordToken(string value, IReaderContext context)
    {
        Value = value;
        Context = context;
    }
    public static Token? BlockWordTokenizer(int node, ExtendedStringReader sourceReader)
    {
        var context = sourceReader.GetContext();

        if (!IsValidBlockWord((char)node))
        {
            return null;
        }

        var accumulator = new StringBuilder();

        //get rid of the space after # until the first character
        CleanSpaceInBetween(sourceReader);

        while (true)
        {
            //lets not include the # in the token value
            node = sourceReader.Read();

            if (node == -1) throw new Exception("Reach end of the file while tokenizing blockword name");

            accumulator.Append((char)node);

            if ((char)sourceReader.Peek() == '}' || (char)sourceReader.Peek() == ' ')
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
            if ((char)sourceReader.Peek() == ' ') sourceReader.Read();
            else break;
        }
    }

    private static bool IsValidBlockWord(char node) => BlockWordIdentifiers.Contains(node);
}
