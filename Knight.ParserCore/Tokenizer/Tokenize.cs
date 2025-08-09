using System.Text;
using Knight.ParserCore.Tokenizer.TokenTypesTokenizer;
using Knight.ParserCore.Utils;

namespace Knight.ParserCore.Tokenizer;


internal static class Tokenizer
{

    private static readonly ITokenTypeTokenizer StaticTokenTokenizer = new StaticTokenTokenizer();
    private static readonly ITokenTypeTokenizer StartTokenTokenizer = new StartTokenTokenizer();
    private static readonly ITokenTypeTokenizer EndTokenTokenizer = new EndTokenTokenizer();
    private static readonly ITokenTypeTokenizer VariableTokenizer = new VariableTokenTokenizer();
    private static readonly ITokenTypeTokenizer BlockTokenTokenizer = new BlockTokenTokenizer();
    private static readonly ITokenTypeTokenizer EnumeratorTokenTokenizer = new EnumeratorTokenTokenizer();
    private static readonly ITokenTypeTokenizer CommentTokenTokenizer = new CommentTokenTokenizer();
    //Hello, I'm {name}.

    // {#   if Ismale}
    //     Im a male.
    // {#else}
    //     Im a female.
    // {#endif}
    //
    // And my age is {age}

    public static IEnumerable<Token> Tokenize(ExtendedStringReader sourceReader)
    {
        try
        {
            return TokenizeImpl(sourceReader);
        }
        catch (System.Exception ex)
        {
            throw new TokenizerException("An Exception occured while trying to tokenize the template: ", ex);
        }
    }


    private static IEnumerable<Token> TokenizeImpl(ExtendedStringReader sourceReader)
    {
        var tokens = new List<Token>();
        var sb = new StringBuilder();
        var node = sourceReader.Read();
        var inExpression = false;

        while (true)
        {
            if (node == -1)
            {
                if (sb.Length > 0)
                {
                    Console.WriteLine($"At the end ");
                    var charArray = sb.ToString().ToCharArray();
                    foreach (var cha in charArray)
                    {
                        Console.WriteLine((int)cha);
                    }
                    tokens.Add(Token.Static(sb.ToString(), sourceReader.GetContext()));
                }

                break;
            }

            if (inExpression)
            {
                // when i am in expression i will be already inside the {{
                //get rid of space
                //TODO: can get rid of this if
                if (char.IsWhiteSpace(node.ToChar()))
                {
                    while (char.IsWhiteSpace(node.ToChar()))
                    {
                        node = sourceReader.Read();

                        if (node == -1)
                        {
                            throw new TokenizerException("Expression close not found while parsing space");
                        }
                    }
                }

                // my source reader poniter now is inside the {{ and handled the white space
                var token = BlockTokenTokenizer.Tokenzie(node, sourceReader);
                token ??= EnumeratorTokenTokenizer.Tokenzie(node, sourceReader);
                token ??= VariableTokenizer.Tokenzie(node, sourceReader);
                token ??= CommentTokenTokenizer.Tokenzie(node, sourceReader);

                if (token is not null)
                {
                    tokens.Add(token);
                    node = sourceReader.Read();
                    continue;
                }

                if ((char)node == '}' && (char)sourceReader.Peek() == '}')
                {
                    tokens.Add(Token.EndExpression(sourceReader.GetContext()));
                    node = sourceReader.Read();
                    node = sourceReader.Read();
                    inExpression = false;
                    continue;
                }

                throw new Exception($"Some thing went wrong. Token is null. Node: {node.ToChar()}, Context: {sourceReader.GetContext()}");
            }
            else
            {
                if ((char)node == '{' && (char)sourceReader.Peek() == '{')
                {
                    inExpression = true;
                    //Tokenize the Static Expression
                    if (sb.Length > 0) tokens.Add(Token.Static(sb.ToString(), sourceReader.GetContext()));

                    //Tokenize the Start Expression 
                    var startExpressionToken = StartTokenTokenizer.Tokenzie(node, sourceReader);
                    if (startExpressionToken is null) throw new TokenizerException($"Could not tokenize start expression. Node:{node.ToChar()}, Context:{sourceReader.GetContext()}");
                    tokens.Add(startExpressionToken);
                    node = sourceReader.Read(); // moving one pointer to get inside the expression

                    sb.Clear();
                    continue;
                }

                sb.Append((char)node);

                node = sourceReader.Read();
            }

        }

        return tokens;
    }
}
