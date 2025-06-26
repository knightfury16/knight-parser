using System.Text;

namespace Knight.ParserCore.Tokenizer;


internal class Tokenizer
{
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
            if (node == -1) break;

            if (inExpression)
            {
                //get rid of space
                if ((char)node == ' ')
                {
                    while ((char)node == ' ')
                    {
                        node = sourceReader.Read();

                        if (node == -1)
                        {
                            throw new Exception("Expression close not found while parsing space");
                        }
                    }
                }
                if ((char)node == '{' && (char)sourceReader.Peek() == '{')
                {
                    tokens.Add(Token.StartExpression(sourceReader.GetContext()));
                    node = sourceReader.Read();
                    continue;
                }
                if ((char)node == '}' && (char)node == '}')
                {
                    tokens.Add(Token.EndExpression(sourceReader.GetContext()));
                    node = sourceReader.Read();
                    inExpression = false;
                    continue;
                }

                var token = VariableToken.VariableTokenizer(sourceReader);
                token ??= BlockWordToken.BlockWordTokenizer(sourceReader);


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
                    tokens.Add(Token.Static(sb.ToString(), sourceReader.GetContext()));
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
