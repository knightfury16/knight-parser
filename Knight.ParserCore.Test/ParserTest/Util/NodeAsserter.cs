using Knight.ParserCore.Parser.Node;

namespace Knight.ParserCore.Test.Util;

public static class NodeAsserter
{
    public static void AssertTextNode(RootNode node, string expected)
    {
        var text = Assert.IsType<TextNode>(node);
        Assert.Equal(expected, text.Text);
    }

    public static void AssertKnightNode(RootNode node, string evalName)
    {
        var knightNode = Assert.IsType<KnightStatement>(node);
        Assert.Equal(evalName, knightNode.EvaluatorVariable.Name);
    }
}
