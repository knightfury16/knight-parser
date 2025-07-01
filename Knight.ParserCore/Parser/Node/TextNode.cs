namespace Knight.ParserCore.Parser.Node;

public class TextNode : RootNode
{
    public string Text { get; set; }

    public TextNode(string text)
    {
        Text = text;
    }
}
