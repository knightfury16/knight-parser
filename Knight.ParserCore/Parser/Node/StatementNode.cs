namespace Knight.ParserCore.Parser.Node;

public abstract class StatementNode : RootNode
{
    public EvaluatorVariable EvaluatorVariable { get; set; } // path exprssion here is variable name  or if or each

    public StatementNode(string value)
    {
        EvaluatorVariable = new EvaluatorVariable(value);
    }
}
