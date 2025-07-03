using Knight.ParserCore.Parser.Node;

namespace Knight.ParserCore.Parser;



public class AstVisitor
{
    private static int _spacer = 0;

    public static void VisitAst(TemplateNode templateNode)
    {
        Console.WriteLine("STARTING AST VISIT::\n\n");
        Console.WriteLine("");
        Console.WriteLine("Template Node: ");
        var body = templateNode.Body;
        if (body is null) throw new ArgumentNullException("Body of the template node found empty!");

        VisitBody(body.ToList());

    }

    private static void VisitBody(List<RootNode> body)
    {

        foreach (var node in body)
        {
            switch (node)
            {
                case TextNode text:
                    VisitTextNode(text);
                    break;
                case BlockStatement blockStatement:
                    VisitBlockStatement(blockStatement);
                    break;
                case KnightStatement knight:
                    VisitKnightNode(knight);
                    break;
                default:
                    throw new Exception("Unexpected type found");
            }
        }
    }

    private static void VisitTextNode(TextNode textNode)
    {
        for (int i = 0; i < _spacer; i++)
        {
            Console.WriteLine("     ");
        }
        Console.WriteLine("Text Node: ");
        Console.WriteLine(textNode.Text);
    }

    private static void VisitBlockStatement(BlockStatement blockStatement)
    {


        for (int i = 0; i < _spacer; i++)
        {
            Console.WriteLine("     ");
        }
        Console.WriteLine("Block Statement: ");
        Console.WriteLine($"Evaluator Name: {blockStatement.EvaluatorVariable.Name}");
        Console.WriteLine($"Params: {blockStatement.Parameter?.FirstOrDefault()?.Name}");
        Console.WriteLine("Consequent code: ");
        _spacer++;
        VisitBody(blockStatement.Consequent.Body.ToList());

        if (blockStatement.Alternate is not null)
        {
            Console.WriteLine("Alternate code: ");
            VisitBody(blockStatement.Alternate.Body.ToList());
        }
    }

    private static void VisitKnightNode(KnightStatement knightStatement)
    {

        for (int i = 0; i < _spacer; i++)
        {
            Console.WriteLine("     ");
        }

        Console.WriteLine("Knight Statement: ");
        Console.WriteLine($"Evaluator Name: {knightStatement.EvaluatorVariable.Name}");
    }



}
