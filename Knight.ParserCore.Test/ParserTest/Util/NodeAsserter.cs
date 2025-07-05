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

    public static void AssertIfBlockStatement(RootNode node, string paramName)
    {
        var blockStatement = Assert.IsType<BlockStatement>(node);
        Assert.Equal("if", blockStatement.EvaluatorVariable.Name);
        var param = blockStatement.Parameter.FirstOrDefault();
        Assert.NotNull(param);
        Assert.Equal(paramName, param.Name);
    }

    public static void AssertIfElseBlockStatement(RootNode node, string paramName)
    {
        var blockStatement = Assert.IsType<BlockStatement>(node);
        Assert.Equal("if", blockStatement.EvaluatorVariable.Name);
        var param = blockStatement.Parameter.FirstOrDefault();
        Assert.NotNull(param);
        Assert.Equal(paramName, param.Name);
        Assert.NotNull(blockStatement.Alternate);
    }

    public static void AssertForBlockStatement(RootNode node, string paramName)
    {
        var blockStatement = Assert.IsType<BlockStatement>(node);
        Assert.Equal("for", blockStatement.EvaluatorVariable.Name);
        var param = blockStatement.Parameter.FirstOrDefault();
        Assert.NotNull(param);
        Assert.Equal(paramName, param.Name);
        Assert.Null(blockStatement.Alternate);
    }
}
