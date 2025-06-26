using System.Text;
namespace Knight.ParserCore.Tokenizer;


internal class VariableToken : Token
{
    public IReaderContext Context { get; }
    public override TokenType Type => TokenType.Variable;
    public override string Value { get; }

    private static HashSet<char> ValidVariableStartCharacter = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToHashSet();


    public VariableToken(string value, IReaderContext context)
    {
        Value = value;
        Context = context;
    }

    public static Token? VariableTokenizer(int node, ExtendedStringReader sourceReader)
    {

        var context = sourceReader.GetContext();

        if (!IsValid((char)node))
        {
            return null;
        }

        var accumulator = new StringBuilder();

        while (true)
        {
            if (node == -1) throw new Exception("Reach end of the file while tokenizing variable name");

            accumulator.Append((char)node);

            if ((char)sourceReader.Peek() == '{' || (char)sourceReader.Peek() == ' ')
            {
                break;
            }

            node = sourceReader.Read();
        }

        return new VariableToken(accumulator.ToString(), context);


    }

    private static bool IsValid(char node) => ValidVariableStartCharacter.Contains(node);
}
