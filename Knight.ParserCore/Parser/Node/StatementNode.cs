namespace Knight.ParserCore.Parser.Node;

public abstract class StatementNode : RootNode
{
    public required EvaluatorVariable EvaluatorVariable { get; set; } // path exprssion here is variable name  or if or each
}
