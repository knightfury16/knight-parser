using Knight.ParserCore.Parser.Node;
using Knight.ParserCore.Parser;

namespace Knight.ParserCore.Test.Util;

public static class NodeAsserter
{
    public static void AssertTextNode(RootNode node, string expected)
    {
        var text = AssertTextNodeType(node);
        Assert.Equal(expected, text.Text);
    }
    public static void AssertTextNode(RootNode node, params string[] expected)
    {
        var text = AssertTextNodeType(node);
        string actual = text.Text;
        Assert.Contains(actual, expected);
    }

    public static TextNode AssertTextNodeType(RootNode node)
    {
        var text = Assert.IsType<TextNode>(node);
        return text;
    }

    public static void AssertKnightNode(RootNode node, string evalName)
    {
        var knightNode = Assert.IsType<KnightStatement>(node);
        Assert.Equal(evalName, knightNode.EvaluatorVariable.Name);
    }

    public static void AssertIfBlockStatement(RootNode node, string paramName)
    {
        var blockStatement = Assert.IsType<BlockStatement>(node);
        Assert.Equal(TemplateKeywords.If, blockStatement.EvaluatorVariable.Name);
        var param = blockStatement.Parameter.FirstOrDefault();
        Assert.NotNull(param);
        Assert.Null(blockStatement.Alternate);
        Assert.Equal(paramName, param.Name);
    }

    public static void AssertIfElseBlockStatement(RootNode node, string paramName)
    {
        var blockStatement = Assert.IsType<BlockStatement>(node);
        Assert.Equal(TemplateKeywords.If, blockStatement.EvaluatorVariable.Name);
        var param = blockStatement.Parameter.FirstOrDefault();
        Assert.NotNull(param);
        Assert.Equal(paramName, param.Name);
        Assert.NotNull(blockStatement.Alternate);
    }

    public static void AssertForBlockStatement(RootNode node, string paramName)
    {
        var blockStatement = Assert.IsType<BlockStatement>(node);
        Assert.Equal(TemplateKeywords.For, blockStatement.EvaluatorVariable.Name);

        var param = blockStatement.Parameter.FirstOrDefault();
        Assert.NotNull(param);
        Assert.Equal(paramName, param.Name);
        Assert.Null(blockStatement.Alternate);
    }
}

public  class LineEndingComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        throw new NotImplementedException();
    }

    // i need a assert block statement method that assert inside the block statement
    public static void AssertBlock(BlockNode block, int? numberOfStatement, IEnumerable<string>? expectedStatementsInOrder = null)
    {
        if (numberOfStatement is not null)
        {
            Assert.Equal(numberOfStatement, block.Body.Count);
        }

        if (expectedStatementsInOrder is not null)
        {
            List<string> listOfBodyTypes = block.Body.Select(node => node.GetType().ToString()).ToList();
            var areSameInOrder = listOfBodyTypes.SequenceEqual(expectedStatementsInOrder);
            Assert.True(areSameInOrder, "The block statements are not in order or same");
        }
    }

}
