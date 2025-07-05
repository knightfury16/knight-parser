using Knight.ParserCore.Test.Fixtures.Basic;
using Knight.ParserCore.Test.Util;
using Knight.ParserCore.Utils;

namespace Knight.ParserCore.Test.ParserTest;


public class BasicParserTest
{
    [Fact]
    public void NoActionTest()
    {
        string source = BasicHelper.GetNoAction();
        var ast = TemporaryTestHelper.GiveMeAst(source);
        Assert.Collection(ast.Body,
                node => NodeAsserter.AssertTextNode(node, "Hello world.\r\nThe new world is the best world.\r\n"));

    }


    [Fact]
    public void SimpleVariableTest()
    {
        string source = BasicHelper.GetSimpleVariable();
        var ast = TemporaryTestHelper.GiveMeAst(source);
        Assert.Collection(ast.Body,
                node => NodeAsserter.AssertTextNode(node, "Hello, "),
                node => NodeAsserter.AssertKnightNode(node, "userName"),
                node => NodeAsserter.AssertTextNode(node, "!\n")
                );


    }

    [Fact]
    public void IfConditionTest()
    {
        string source = BasicHelper.GetIfCondition();
        var ast = TemporaryTestHelper.GiveMeAst(source);
        Assert.Collection(ast.Body,
                node => NodeAsserter.AssertIfBlockStatement(node, "isAdmin"),
                node => NodeAsserter.AssertTextNode(node, "\n")
                );


    }

    [Fact]
    public void IfElseConditionTest()
    {
        string source = BasicHelper.GetIfElseCondition();
        var ast = TemporaryTestHelper.GiveMeAst(source);
        Assert.Collection(ast.Body,
                node => NodeAsserter.AssertIfElseBlockStatement(node, "isLoggedIn"),
                node => NodeAsserter.AssertTextNode(node, "\n")
                );


    }

    [Fact]
    public void ForLoopTest()
    {
        string source = BasicHelper.GetForLoop();
        var ast = TemporaryTestHelper.GiveMeAst(source);
        Assert.Collection(ast.Body,
                node => NodeAsserter.AssertForBlockStatement(node, "items"),
                node => NodeAsserter.AssertTextNode(node, "\n")
                );


    }
}
