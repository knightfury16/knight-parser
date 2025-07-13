using System.Text;
using Knight.ParserCore.Tokenizer.TokenTypesTokenizer;

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
                //get rid of space
                if (char.IsWhiteSpace((char)node))
                {
                    while (char.IsWhiteSpace((char)node))
                    {
                        node = sourceReader.Read();

                        if (node == -1)
                        {
                            throw new TokenizerException("Expression close not found while parsing space");
                        }
                    }
                    continue; //get out of space loop
                }
                if ((char)node == '{' && (char)sourceReader.Peek() == '{')
                {
                    tokens.Add(Token.StartExpression(sourceReader.GetContext()));
                    node = sourceReader.Read();
                    node = sourceReader.Read(); //get over the second {
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

                // my source reader poniter might be at '{'
                var token = VariableToken.VariableTokenizer(node, sourceReader);
                token ??= BlockWordToken.BlockWordTokenizer(node, sourceReader);


                if (token is null)
                {
                    throw new Exception("Some thing went wrong. Token is null");
                }

                tokens.Add(token);

                node = sourceReader.Read();
            }
            else
            {
                if ((char)node == '{' && (char)sourceReader.Peek() == '{')
                {
                    inExpression = true;
                    if (sb.Length > 0) tokens.Add(Token.Static(sb.ToString(), sourceReader.GetContext()));
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
