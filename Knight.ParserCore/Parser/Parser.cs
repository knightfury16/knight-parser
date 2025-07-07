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
    private readonly List<Token> _tokens;
    private int _position = 0;
    private Stack<string> _blockStack = new();


    public Parser(List<Token> tokens)
    {
        _tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
    }

    private TokenType? Peek() => _position < _tokens.Count ? _tokens[_position].Type : null;

    private Token Consume()
    {
        return _tokens[_position++];
    }


    public TemplateNode ParseTemplate()
    {
        var parsedBlock = ParseBlock();
        return new TemplateNode(parsedBlock.Body);
    }

    private BodyNode ParseBlock()
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

        if (!ExceptToken(TokenType.BlockWord, out var blockToken))
            return null;

        return blockToken.Value switch
        {
            TemplateKeywords.If => ParseIfStatement(),
            TemplateKeywords.Else => ParseElseStatement(),
            TemplateKeywords.EndIf => ParseEndIfStatement(),
            TemplateKeywords.For => ParseForStatement(),
            TemplateKeywords.EndFor => ParseEndForStatement(),
            _ => throw new ParserException($"Unknown block word: {blockToken.Value}")
        };
    }

    private RootNode? ParseEndForStatement()
    {

        Consume();
        _blockStack.Push(TemplateKeywords.EndFor);
        return null;
    }

    private RootNode? ParseForStatement()
    {

        var param = Consume();
        Consume(); //consume the end expression
        _blockStack.Push(TemplateKeywords.For);
        var consequent = ParseBlock();

        var a = _blockStack.Pop();

        if (a == TemplateKeywords.EndFor)
        {
            var blockStatement = new BlockStatement(TemplateKeywords.For, param.Value) { Consequent = (BlockNode)consequent };
            return blockStatement;
        }

        throw new ParserException($"Expected endfor found {a}");
    }

    private RootNode? ParseEndIfStatement()
    {
        Consume(); //consume the end expression
        _blockStack.Push(TemplateKeywords.EndIf);
        return null;
    }

    private RootNode? ParseElseStatement()
    {
        Consume(); // consume the end expression
        _blockStack.Push(TemplateKeywords.Else);
        return null;
    }

    private RootNode? ParseIfStatement()
    {
        var param = Consume();

        Consume(); //consume the end expression

        _blockStack.Push(TemplateKeywords.If);

        // the consequent of the if statement
        var consequent = ParseBlock();

        RootNode? alternate = null;

        var a = _blockStack.Pop();

        if (a == TemplateKeywords.Else)
        {
            // this is the alternate statement if else exists
            alternate = ParseBlock();

            a = _blockStack.Pop();
        }

        if (a == TemplateKeywords.EndIf)
        {
            var blockStatement = new BlockStatement(TemplateKeywords.If, param.Value) { Consequent = (BlockNode)consequent };
            blockStatement.Alternate = (BlockNode?)alternate;
            return blockStatement;
        }
        throw new ParserException($"Expected else or endif found {a}");
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

    private bool ExceptToken(TokenType expectedTokenType, out Token token)
    {
        token = null!;
        // if(im at the end throw exception)

        if (Peek() != expectedTokenType)
            throw new ParserException($"Expected {expectedTokenType}, found {Peek()}");

        token = Consume();
        return true;
    }
}

