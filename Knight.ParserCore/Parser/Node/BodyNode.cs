namespace Knight.ParserCore.Parser.Node;

public abstract class BodyNode : RootNode
{
    public List<RootNode> Body { get; set; }

    public BodyNode()
    {
        Body = new List<RootNode>();
    }
}
