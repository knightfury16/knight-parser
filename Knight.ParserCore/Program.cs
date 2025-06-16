using System.Text;

namespace Knight.ParserCore;

class Program
{
    static void Main(string[] args)
    {
        //this is the expression i want to Parse
        //(2 + ( 4 -2 ))
        var myExpression = "(123 + ( 4 - 2 ))";
        var stringReader = new StringReader(myExpression);

        var tokens = Tokenizer.Tokenize(stringReader);

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }

    }
}

public class Tokenizer
{
    public static IEnumerable<Token> Tokenize(TextReader reader)
    {
        var validNumbers = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        var operators = new Dictionary<char, string>
        {
            {'+', "addition"},
            {'-', "sub"},
            {'*', "multiply"},
            {'/', "divide"},
        }
        ;
        var tokens = new List<Token>();

        int input;

        while (true)
        {
            input = reader.Read();

            if (input == -1) break;

            if ((char)input == '(' || (char)input == ')')
            {
                tokens.Add(new Token("paren", (char)input));
                continue;
            }

            if (validNumbers.Contains((char)input))
            {
                var fullNumber = new StringBuilder();
                fullNumber.Append((char)input);

                while (validNumbers.Contains((char)reader.Peek()))
                {
                    input = reader.Read();
                    fullNumber.Append((char)input);
                }

                tokens.Add(new Token("number", fullNumber.ToString()));
                continue;
            }

            if (operators.TryGetValue((char)input, out string? result))
            {
                if (result is not null)
                {
                    tokens.Add(new Token(result, (char)input));
                }
                continue;
            }

            //ignoring the whitespace
            if ((char)input == ' ') continue;

            //if im here no condition got matched. throw some error 
            throw new Exception($"Could not match the character {input} ");
        }

        return tokens;
    }

}

public class Token
{
    public string Value { get; }
    public string TokenType { get; }

    public Token(string tokenType, char value)
    {
        TokenType = tokenType;
        Value = value.ToString();
    }

    public Token(string tokenType, string value)
    {
        TokenType = tokenType;
        Value = value;
    }

    public override string ToString()
    {
        return $"Type: {TokenType}, Value: {Value}";
    }
}

