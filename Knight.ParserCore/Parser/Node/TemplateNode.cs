namespace Knight.ParserCore.Parser.Node;

//this is to mark the starting ndoe sort of like main function
public class TemplateNode : BodyNode
{

    public TemplateNode(List<RootNode> body)
    {
        Body = body;
    }
}

