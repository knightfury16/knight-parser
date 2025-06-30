namespace Knight.ParserCore.Parser.Node;

public abstract class BodyNode : RootNode
{
    public required IEnumerable<RootNode> Body { get; set; }
}
