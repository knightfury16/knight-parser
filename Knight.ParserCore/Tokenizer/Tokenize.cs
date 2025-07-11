using System.Text;

namespace Knight.ParserCore.Tokenizer;


internal class Tokenizer
{
    public const char Space = ' ';
    //Hello, I'm {name}.

    // {#   if Ismale}
    //     Im a male.
    // {#else}
    //     Im a female.
    // {#endif}
    //
    // And my age is {age}

    public IEnumerable<Token> Tokenize(ExtendedStringReader sourceReader)
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
                if ((char)node == Space)
                {
                    while ((char)node == Space)
                    {
                        node = sourceReader.Read();

                        if (node == -1)
                        {
                            throw new Exception("Expression close not found while parsing space");
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
