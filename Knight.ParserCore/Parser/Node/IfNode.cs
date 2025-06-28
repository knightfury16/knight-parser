namespace Knight.ParserCore.Parser.Node;

public class IfNode : TemplateNode
{
    public required string ConditionVariableName { get; set; }

    public required List<TemplateNode> TrueBranch { get; set; }
    public List<TemplateNode>? ElseBranch { get; set; }
}
