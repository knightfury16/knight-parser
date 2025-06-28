namespace Knight.ParserCore.Parser.Node;

public class ForNode : TemplateNode
{
    public required string CollectionName { get; set; } //ex: Users
    public string EnumeratorName { get; set; } = "this"; //default to 'this'. ex: user
    public required List<TemplateNode> Body { get; set; }
}

