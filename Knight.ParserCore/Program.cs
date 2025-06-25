using System.Text;

namespace Knight.ParserCore;

class Program
{
    static void Main(string[] args)
    {
        //this is the expression i want to Parse
        // var myExpression = "(2 * ( 4 - 2 ))";
        // var myExpression = "(123 + (6 / 2) * (321) * ( 4 - 2 ))";
        // var myExpression = "((6 / 2) * (321) * ( 4 - 2 ))";
        var allTextContent = File.ReadAllText("./test.text");
        var stringReader = new StringReader(allTextContent);
        var esr = new ExtendedStringReader(stringReader);

        while (true)
        {
            Console.WriteLine(esr.GetContext().ToString());
            var input = esr.Read();
            if (input == -1) break;
            Console.WriteLine($"Character: {(char)input}");

        }

        // if (Tokenizer.ValidateExpression(stringReader))
        // {
        //
        //     var tokens = Tokenizer.Tokenize(new StringReader(myExpression));
        //
        //     var parser = new Parser(tokens);
        //     // the parent ast node
        //     AstNode ast = parser.ParseExpression();
        //     AstVisitor.Visit(ast, 0);
        //     Console.WriteLine("Start evaluating ast...");
        //     Console.WriteLine(Evaluator.Evaluate(ast));
        // }
        // else
        // {
        //     Console.WriteLine("Your provided expression is not valid");
        // }


    }
}

public class Evaluator
{
    public static int Evaluate(AstNode node)
    {
        if (node is NumberNode numberNode)
        {
            return int.Parse(numberNode.Value);
        }
        else if (node is BinaryOpNode binaryOpNode)
        {
            var left = binaryOpNode.Left;
            var right = binaryOpNode.Right;
            var operation = binaryOpNode.Operation;

            switch (operation)
            {
                case "addition":
                    return Evaluate(left) + Evaluate(right);
                case "sub":
                    return Evaluate(left) - Evaluate(right);
                case "multiply":
                    return Evaluate(left) * Evaluate(right);
                case "divide":
                    var rightValue = Evaluate(right);
                    if (rightValue == 0) throw new DivideByZeroException();
                    return Evaluate(left) / rightValue;
                default:
                    throw new Exception("No operation match to evaluate");
            }

        }

        throw new Exception("Ast node is not recognized");
    }
}


public abstract class AstNode { }

public class NumberNode(string value) : AstNode
{
    public string Value { get; set; } = value;

    public override string ToString()
    {
        return $"Number: {Value} ";
    }
}

public class BinaryOpNode(string operation, AstNode left, AstNode right) : AstNode
{
    public string Operation { get; set; } = operation;
    public AstNode Left { get; set; } = left;
    public AstNode Right { get; set; } = right;
}

// Defined Grammer for this expression evaluation
// expression ::= term ( ('+' | '-') term)*
// term ::= factor ( ( '*' | '/') factor)*
// factor ::= number | expression

public class AstVisitor
{
    private static int _depth = 0;
    public static void Visit(AstNode node)
    {
        PutSpacer();
        if (node is NumberNode numberNode)
        {
            Console.WriteLine($"Number: {numberNode.Value}");
        }
        else if (node is BinaryOpNode binaryOp)
        {
            Console.WriteLine($"Expression: {binaryOp.Operation}");
            var leftNode = binaryOp.Left;
            var righNode = binaryOp.Right;
            _depth++;
            Visit(leftNode);
            Visit(righNode);
        }
    }

    public static void Visit(AstNode node, int depth)
    {
        PutSpacer(depth);

        if (node is NumberNode numberNode)
        {
            Console.WriteLine($"Number: {numberNode.Value}");
        }
        else if (node is BinaryOpNode binaryOp)
        {
            Console.WriteLine($"Expression: {binaryOp.Operation}");
            var leftNode = binaryOp.Left;
            var righNode = binaryOp.Right;
            var leftDepth = depth + 1;
            var rightDepth = depth + 1;
            Visit(leftNode, leftDepth);
            Visit(righNode, rightDepth);
        }
    }

    private static void PutSpacer()
    {
        for (int i = 0; i < _depth; i++)
        {
            Console.Write(" ");
        }
    }
    private static void PutSpacer(int depth)
    {
        for (int i = 0; i < depth; i++)
        {
            Console.Write(" ");
        }
    }
}


public class Parser
{
    private List<Token> _tokens;
    private int _position = 0;

    public Parser(IEnumerable<Token> tokens)
    {
        _tokens = tokens.ToList();
        _position = 0;
    }

    private Token? Peek()
    {
        return _position < _tokens.Count ? _tokens[_position] : null;
    }

    private Token Consume()
    {
        return _tokens[_position++];
    }

    private bool Match(string operand)
    {
        var token = Peek();

        if (token == null) return false;

        return token.TokenType == operand;
    }

    //I need method for each grammer term
    //(2 + ( 4 -2 ))
    public AstNode ParseExpression()
    {
        //(6 / 2) * (321) * ( 4 - 2 )
        var left = ParseTerm();
        while (Match("addition") || Match("sub"))
        {
            var token = Consume();
            var right = ParseTerm();
            left = new BinaryOpNode(token.TokenType, left, right); // will return a BinaryOpNode
        }
        return left; // like the expression (2) , will return a NumberNode
    }

    public AstNode ParseTerm()
    {
        //(6 / 2) * (321) * ( 4 - 2 )
        var left = ParseFactor();
        while (Match("multiply") || Match("divide"))
        {
            var token = Consume();
            var right = ParseFactor();
            left = new BinaryOpNode(token.TokenType, left, right);
        }

        return left;
    }

    public AstNode ParseFactor()
    {
        var token = Peek();

        if (token == null) throw new ArgumentNullException();

        if (token.TokenType == "number")
        {
            //move the cursor position
            Consume();
            return new NumberNode(token.Value);
        }
        else if (token.TokenType == "paren" && token.Value == "(")
        {
            //move the cursor
            //example: (2)
            Consume();
            // so if its not a number then it can only be an expression according to grammar
            //(6 / 2) * (321) * ( 4 - 2 )
            var expr = ParseExpression(); // after this the cursor will be moved to the last place

            token = Peek();
            ArgumentNullException.ThrowIfNull(token);

            //recursin is killing my brain cell and i like it.
            Consume(); // consume ')'
            return expr;
        }

        throw new Exception($"Unexpected Token: {token.ToString()}");
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

    public static bool ValidateExpression(TextReader reader)
    {
        var parenStack = new Stack<char>();

        int input;

        while (true)
        {
            input = reader.Read();
            if (input == -1) break;

            if ((char)input == '(')
            {
                parenStack.Push('(');
                continue;
            }

            if ((char)input == ')')
            {
                //i have a close bracket but stack is empty, then the exression is not valid
                if (parenStack.Count == 0) return false;
                parenStack.Pop();
                continue;
            }
        }

        //out the looop i still have open paren, expression is not valid
        if (parenStack.Count > 0) return false;

        return true;
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

