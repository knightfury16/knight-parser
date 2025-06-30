using Knight.ParserCore.Parser.Node;

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


    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }


    public BodyNode Parse()
    {

        //i need a BlockStatmentParser
        // a TextNodeParser
        //a knightStatementParser

    }
}

