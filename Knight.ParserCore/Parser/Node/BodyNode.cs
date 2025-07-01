namespace Knight.ParserCore.Parser.Node;

public abstract class BodyNode : RootNode
{
    public IEnumerable<RootNode> Body { get; set; }
}
