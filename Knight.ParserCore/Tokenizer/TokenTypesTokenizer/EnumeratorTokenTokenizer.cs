using System.Text;
using Knight.ParserCore.Utils;

namespace Knight.ParserCore.Tokenizer.TokenTypesTokenizer;


internal class EnumeratorTokenTokenizer : ITokenTypeTokenizer
{
    private static HashSet<char> ValidEnumeratorCharaters = "~".ToHashSet();

    public Token? Tokenzie(int node, ExtendedStringReader sourceReader)
    {
        var context = sourceReader.GetContext();

        if (!IsValid(node.ToChar()))
        {
            return null;
        }

        var accumulator = new StringBuilder();

        node = sourceReader.Read(); //skip the ~

        //I need to parse till after the 'in'
        //TODO: Refactor this
        while (true)
        {
            if (node == -1) throw new TokenizerException("Reach end of the file while tokenizing variable name");

            accumulator.Append(node.ToChar());

            // am expecting 'in' not the end
            if (sourceReader.Peek().ToChar() == '}')
            {
                return null;
            }

            if (sourceReader.Peek().ToChar() == ' ')
            {
                //check for in
                node = sourceReader.Read();

                while (char.IsWhiteSpace(node.ToChar()))
                {
                    node = sourceReader.Read();
                }

                CheckForIn(node, sourceReader);
                break;
            }

            node = sourceReader.Read();
        }

        return new VariableEnumeratorToken(accumulator.ToString(), context);
    }

    private static void CheckForIn(int node, ExtendedStringReader source)
    {
        if (node == -1 || source.Peek() == -1)
        {
            throw new TokenizerException($"Reach end without tokenzing enumerator value. Value: {node.ToChar()} Context: {source.GetContext()}");
        }
        var i = node.ToChar();
        var n = source.Peek().ToChar();

        if (i != 'i' && n != 'n')
        {
            throw new TokenizerException($"Enumerator syntax error. Expected 'in' found {i}{n}. Value: {node.ToChar()} Context: {source.GetContext()}");
        }

        source.Read(); // Pointer on n
        source.Read(); // pointer skiping n
    }

    private static bool IsValid(char node)
    {
        // Console.WriteLine($"Checking if valid iterator {node}");
        return ValidEnumeratorCharaters.Contains(node);
    }
}
