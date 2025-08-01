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
        //theoretically this if will not catch 
        if (_position >= _tokens.Count) throw new InvalidOperationException("Attempted to consume token beyond end of input.");

        return _tokens[_position++];
    }

    private bool IsAtEnd => _position >= _tokens.Count;


    // this is the entry point to this class
    public TemplateNode ParseTemplate()
    {
        var parsedBlock = ParseBlock();
        return new TemplateNode(parsedBlock.Body);
    }

    private BodyNode ParseBlock()
    {
        var blockNode = new BlockNode();

        while (!IsAtEnd)
        {
            var node = ParseBlockElement();

            if (node is null)
            {
                break;
            }

            blockNode.Body.Add(node);
        }

        return blockNode;
    }

    private RootNode? ParseBlockElement()
    {
        return Peek() switch
        {
            TokenType.Static => ParseTextNode(),
            TokenType.StartExpression => ParseExpression(),
            _ => throw new ParserException($"Unexpected token type: {Peek()}. Expected Static  or StartExpression token.")
        };
    }

    // a TextNodeParser
    private RootNode ParseTextNode()
    {
        ExpectToken(TokenType.Static, out var token);

        return new TextNode(token.Value);
    }

    private RootNode? ParseExpression()
    {
        ExpectToken(TokenType.StartExpression, out var startExpressionToken); // consuming the start token

        RootNode? parsedExpressionNode = null; // doing this instead of directly returning is for consuming the end expression here also

        if (Peek() == TokenType.Variable)
        {
            parsedExpressionNode = ParseKnightstatement();
        }
        else if (Peek() == TokenType.BlockWord)
        {
            parsedExpressionNode = ParseBlockstatement();
        }

        //this might not be always end expression
        if (Peek() == TokenType.EndExpression) ExpectToken(TokenType.EndExpression, out var _);

        return parsedExpressionNode;
    }

    //a knightStatementParser
    private RootNode ParseKnightstatement()
    {
        ExpectToken(TokenType.Variable, out var token);
        return new KnightStatement(token.Value);
    }

    //i need a BlockStatmentParser
    private RootNode? ParseBlockstatement()
    {

        if (!ExpectToken(TokenType.BlockWord, out var blockToken))
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


    private RootNode? ParseIfStatement()
    {
        ExpectToken(TokenType.Variable, out var variableToken);
        // TODO: Might not have an en expression, might be another variable 
        ExpectToken(TokenType.EndExpression, out var _); //consuming the end the token

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
            var blockStatement = new BlockStatement(TemplateKeywords.If, variableToken.Value) { Consequent = (BlockNode)consequent };
            blockStatement.Alternate = (BlockNode?)alternate;
            return blockStatement;
        }
        throw new ParserException($"Expected {TemplateKeywords.Else} or {TemplateKeywords.EndIf}, found {a}");
    }


    private RootNode? ParseElseStatement()
    {
        ExpectToken(TokenType.EndExpression, out var _);//consume the end expression
        _blockStack.Push(TemplateKeywords.Else);
        return null;
    }

    private RootNode? ParseEndIfStatement()
    {
        ExpectToken(TokenType.EndExpression, out var _);//consume the end expression
        _blockStack.Push(TemplateKeywords.EndIf);
        return null;
    }

    private RootNode? ParseForStatement()
    {

        ExpectToken(TokenType.Variable, out var variableToken);
        ExpectToken(TokenType.EndExpression, out var _); // consuming the end expression

        _blockStack.Push(TemplateKeywords.For);
        var consequent = ParseBlock();

        var a = _blockStack.Pop();

        if (a == TemplateKeywords.EndFor)
        {
            var blockStatement = new BlockStatement(TemplateKeywords.For, variableToken.Value) { Consequent = (BlockNode)consequent };
            return blockStatement;
        }

        throw new ParserException($"Expected {TemplateKeywords.EndFor}, found {a}");
    }

    private RootNode? ParseEndForStatement()
    {
        ExpectToken(TokenType.EndExpression, out var _); // consuming the end expression
        _blockStack.Push(TemplateKeywords.EndFor);
        return null;
    }

    private bool ExpectToken(TokenType expectedTokenType, out Token token)
    {
        token = null!;

        if (IsAtEnd) throw new Exception($"Expected {expectedTokenType}, no token found. At the end of the file.");

        if (Peek() != expectedTokenType)
            throw new ParserException($"Expected {expectedTokenType}, found {Peek()}");

        token = Consume();
        return true;
    }
}

