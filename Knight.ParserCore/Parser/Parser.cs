using Knight.ParserCore.Parser.Node;
using Knight.ParserCore.Tokenizer;

namespace Knight.ParserCore.Parser;


//lets define our grammar
//
//Template ::= (TextNode | KnightStatement | BlockStatement)*
//KnightStatement ::= value
//BlockStatement ::= Block
//Block ::= (TextNode | KnightStatement | BlockStatement)*
//TextNode ::= value

public class Parser
{

    private List<Token> _tokens;
    private int _position = 0;


    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    private TokenType Peek()
    {
        return _tokens[_position].Type;
    }

    private Token Consume()
    {
        return _tokens[_position++];
    }


    public BodyNode ParseTemplate()
    {
        var parsedBlock = ParseBlock();

        if (parsedBlock is BodyNode bodyNode)
        {
            return new TemplateNode(bodyNode.Body.ToList());
        }

        throw new Exception($"In {nameof(ParseTemplate)}, parsed body is not a body node");
    }

    private RootNode ParseBlock()
    {
        var blockNode = new BlockNode();

        while (_position < _tokens.Count)
        {
            switch (Peek())
            {
                case TokenType.Static:
                    blockNode.Body.Append(ParseTextNode());
                    break;
                case TokenType.StartExpression:
                    blockNode.Body.Append(ParseExpression());
                    break;

                default:
                    Console.WriteLine($"In {nameof(ParseBlock)}, expected token type static or StartExpression. Found {Peek().ToString()}");
                    break;
            }
        }

        return blockNode;
    }

    private RootNode ParseExpression()
    {
        var startExpressionToken = Consume(); //consuming start expression
        RootNode? parsedExpressionNode = null; // doing this instead of directly returning is for consuming the end expression here also

        if (startExpressionToken is not StartExpressionToken)
        {
            throw new Exception($"In {nameof(ParseExpression)}, expected StartExpression Token found {startExpressionToken.Type.ToString()}");
        }

        if (Peek() == TokenType.Variable)
        {
            parsedExpressionNode = ParseKnightstatement();
        }
        else if (Peek() == TokenType.BlockWord)
        {
            parsedExpressionNode = ParseBlockstatement();

        }

        if (parsedExpressionNode is null)
        {
            throw new Exception($"In {nameof(ParseExpression)}, expected variable token or block word token found {Peek().ToString()}");
        }

        if (Peek() == TokenType.EndExpression) Consume(); // consuming the end expression

        return parsedExpressionNode;

    }

    //i need a BlockStatmentParser
    private RootNode ParseBlockstatement()
    {

    }

    //a knightStatementParser
    private RootNode ParseKnightstatement()
    {
        var token = Consume();

        if (token is not VariableToken)
        {
            throw new Exception($"In {nameof(ParseKnightstatement)}, expected Variable token found {token.Type.ToString()}");
        }

        return new KnightStatement(token.Value);
    }


    // a TextNodeParser
    private RootNode ParseTextNode()
    {
        var token = Consume();

        if (token is StaticToken staticToken)
        {
            return new TextNode(token.Value);
        }

        throw new Exception($"In ParseTextNode expected static token found {token.Type.ToString()}");

    }
}

