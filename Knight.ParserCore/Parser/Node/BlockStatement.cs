namespace Knight.ParserCore.Parser.Node;

public class BlockStatement : StatementNode
{
    public List<EvaluatorVariable> Parameter { get; set; }

    public required BlockNode Consequent { get; set; }

    public BlockNode? Alternate { get; set; }

    public BlockStatement(string value, string param) : base(value)
    {
        Parameter = new() { new(param) };
    }
}


