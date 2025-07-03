using Knight.ParserCore.Parser.Node;

namespace Knight.ParserCore.Parser;



public class AstVisitor
{

    public static void VisitAst(TemplateNode templateNode)
    {
        Console.WriteLine("STARTING AST VISIT::\n\n");
        Console.WriteLine("");
        Console.WriteLine("Template Node: ");
        var body = templateNode.Body;
        if (body is null) throw new ArgumentNullException("Body of the template node found empty!");

        VisitBody(body.ToList(), 1);

    }

    public static void PrintTemplateBody(TemplateNode templateNode, bool recurse = false)
    {
        if (recurse) { PrintTemplateRecursively(templateNode, 1); }
        else { PrintBody(templateNode, 1); }
    }


    private static void PrintTemplateRecursively(BodyNode bodyNode, int depth)
    {
        if (depth == 1)
        {
            Console.WriteLine("Recursive Template node print: ");
        }

        foreach (var node in bodyNode.Body)
        {
            if (node is not BlockStatement)
            {
                Space(depth);
                Console.WriteLine(node.GetType());
            }
            if (node is BlockStatement blockStatement)
            {
                Space(depth);
                Console.WriteLine(node.GetType());
                Space(depth + 1);
                Console.WriteLine("Consequent Body: ");
                PrintTemplateRecursively(blockStatement.Consequent, depth + 2);
                if (blockStatement.Alternate is not null)
                {
                    Space(depth + 1);
                    Console.WriteLine("Alternate Body: ");
                    PrintTemplateRecursively(blockStatement.Alternate, depth + 2);

                }
            }
        }
    }


    private static void PrintBody(TemplateNode templateNode, int depth)
    {
        Console.WriteLine("Body are: ");

        foreach (var node in templateNode.Body)
        {
            Space(depth);
            Console.WriteLine(node.GetType());
        }
    }

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
