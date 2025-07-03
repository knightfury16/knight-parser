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

        VisitBody(body.ToList(), 1);

    }

    {
    private static void VisitBody(List<RootNode> body, int depth)
    {


        foreach (var node in body)
        {
            Space(depth);
            switch (node)
            {
                case TextNode text:
                    VisitTextNode(text, depth + 1);
                    break;
                case BlockStatement blockStatement:
                    VisitBlockStatement(blockStatement, depth + 1);
                    break;
                case KnightStatement knight:
                    VisitKnightNode(knight, depth + 1);
                    break;
                default:
                    throw new Exception("Unexpected type found");
            }
        }
    }

    private static void VisitTextNode(TextNode textNode, int depth)
    {
        Console.WriteLine("Text Node: ");
        Space(depth);
        Console.WriteLine(textNode.Text);
    }

    private static void VisitBlockStatement(BlockStatement blockStatement, int depth)
    {
        Console.WriteLine("Block Statement: ");
        Space(depth);
        Console.WriteLine($"Evaluator Name: {blockStatement.EvaluatorVariable.Name}");
        Space(depth);
        Console.WriteLine($"Params: {blockStatement.Parameter?.FirstOrDefault()?.Name}");
        Space(depth);
        Console.WriteLine("Consequent code: ");

        VisitBody(blockStatement.Consequent.Body.ToList(), depth + 1);

        if (blockStatement.Alternate is not null)
        {
            Space(depth);
            Console.WriteLine("Alternate code: ");
            VisitBody(blockStatement.Alternate.Body.ToList(), depth + 1);
        }
    }

    private static void VisitKnightNode(KnightStatement knightStatement, int depth)
    {

        Console.WriteLine("Knight Statement: ");
        Space((depth));
        Console.WriteLine($"Evaluator Name: {knightStatement.EvaluatorVariable.Name}");
    }

    private static void Space(int depth)
    {
        for (int i = 0; i < depth; i++)
        {
            Console.Write(" ");
        }
    }

}
