namespace Knight.ParserCore.Parser.Node;

public class BlockStatement : StatementNode
{
    public List<EvaluatorVariable>? Prameter { get; set; }

    public required BlockNode Consequent { get; set; }

    public BlockNode? Alternate { get; set; }
}


