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

internal class Parser
{

    private List<Token> _tokens;
    private int _position = 0;
    private Stack<string> _blockStack = new();


    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    private TokenType? Peek()
    {
        return _position < _tokens.Count ? _tokens[_position].Type : null;
    }

    private Token Consume()
    {
        return _tokens[_position++];
    }


    public TemplateNode ParseTemplate()
    {
        var parsedBlock = ParseBlock();

        if (parsedBlock is BodyNode bodyNode)
        {
            return new TemplateNode(bodyNode.Body.ToList());
        }

        throw new ParserException($"In {nameof(ParseTemplate)}, parsed body is not a body node");
    }

    private RootNode ParseBlock()
    {
        var blockNode = new BlockNode();
        var reachedEndExpression = false;

        while (_position < _tokens.Count && !reachedEndExpression)
        {
            switch (Peek())
            {
                case TokenType.Static:
                    blockNode.Body.Add(ParseTextNode());
                    break;
                case TokenType.StartExpression:
                    var parsedExpression = ParseExpression();
                    if (parsedExpression is null) reachedEndExpression = true;
                    else
                    {
                        blockNode.Body.Add(parsedExpression);
                    }
                    break;
                default:
                    Console.WriteLine($"In {nameof(ParseBlock)}, expected token type static or StartExpression. Found {Peek().ToString()}");
                    break;
            }
        }

        return blockNode;
    }

    private RootNode? ParseExpression()
    {
        var startExpressionToken = Consume(); //consuming start expression
        RootNode? parsedExpressionNode = null; // doing this instead of directly returning is for consuming the end expression here also

        if (startExpressionToken is not StartExpressionToken)
        {
            throw new ParserException($"In {nameof(ParseExpression)}, expected StartExpression Token found {startExpressionToken.Type.ToString()}");
        }

        if (Peek() == TokenType.Variable)
        {
            parsedExpressionNode = ParseKnightstatement();
        }
        else if (Peek() == TokenType.BlockWord)
        {
            parsedExpressionNode = ParseBlockstatement();
        }

        if (Peek() == TokenType.EndExpression) Consume(); // consuming the end expression

        return parsedExpressionNode;

    }

    //i need a BlockStatmentParser
    private RootNode? ParseBlockstatement()
    {

        var token = Consume();

        switch (token.Value)
        {
            case "if":
                var param = Consume();
                Consume(); //consume the end expression
                _blockStack.Push("if");
                var consequent = ParseBlock();
                RootNode? alternate = null;

                var a = _blockStack.Pop();

                if (a == "else")
                {
                    alternate = ParseBlock();
                    a = _blockStack.Pop();
                }

                if (a == "endif")
                {
                    var blockStatement = new BlockStatement("if", param.Value) { Consequent = (BlockNode)consequent };
                    blockStatement.Alternate = (BlockNode?)alternate;
                    return blockStatement;
                }
                throw new ParserException($"Expected else or endif found {a}");
            case "else":
                Consume();
                _blockStack.Push("else");
                return null;
            case "endif":
                Consume(); //consume the end expression
                _blockStack.Push("endif");
                return null;
            case "for":
                param = Consume();
                Consume(); //consume the end expression
                _blockStack.Push("for");
                consequent = ParseBlock();

                a = _blockStack.Pop();

                if (a == "endfor")
                {
                    var blockStatement = new BlockStatement("for", param.Value) { Consequent = (BlockNode)consequent };
                    return blockStatement;
                }

                throw new ParserException($"Expected endfor found {a}");
            case "endfor":
                Consume();
                _blockStack.Push("endfor");
                return null;
            default:
                throw new ParserException("Invalid block word");

        }

    }

    //a knightStatementParser
    private RootNode ParseKnightstatement()
    {
        var token = Consume();

        if (token is not VariableToken)
        {
            throw new ParserException($"In {nameof(ParseKnightstatement)}, expected Variable token found {token.Type.ToString()}");
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

        throw new ParserException($"In ParseTextNode expected static token found {token.Type.ToString()}");

    }
}

