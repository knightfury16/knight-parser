using Knight.ParserCore.Parser.Node;
using Knight.ParserCore.Test.Fixtures.Basic;
using Knight.ParserCore.Test.Util;
using Knight.ParserCore.Utils;

namespace Knight.ParserCore.Test.ParserTest;


public class BasicParserTest
{
    #region Valid Cases
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
                node => NodeAsserter.AssertTextNode(node, "\n", "\r\n")
                );


    }

    //i need to evaluate the inside of a block statement too
    [Fact]
    public void IfConditionTestWithRecurse()
    {
        string source = BasicHelper.GetIfCondition();
        var ast = TemporaryTestHelper.GiveMeAst(source);

        //expeted statement
        var expectedStatement = new ExpectedStatement(1, typeof(TextNode));


        Assert.Collection(ast.Body,
                node => NodeAsserter.AssertIfBlockStatement(node, "isAdmin", expectedStatement),
                node => NodeAsserter.AssertTextNode(node, "\n", "\r\n")
                );
    }

    [Fact]
    public void IfElseConditionTest()
    {
        string source = BasicHelper.GetIfElseCondition();
        var ast = TemporaryTestHelper.GiveMeAst(source);
        Assert.Collection(ast.Body,
                node => NodeAsserter.AssertIfElseBlockStatement(node, "isLoggedIn"),
                node => NodeAsserter.AssertTextNode(node, "\n", "\r\n")
                );


    }

    [Fact]
    public void IfElseConditionTestWithRecurse()
    {
        string source = BasicHelper.GetIfElseCondition();
        var ast = TemporaryTestHelper.GiveMeAst(source);

        var expectedStatement = new ExpectedStatement(1, [typeof(TextNode)]);
        expectedStatement.SetAlternateStatementsInOrder(1, [typeof(TextNode)]);


        Assert.Collection(ast.Body,
                node => NodeAsserter.AssertIfElseBlockStatement(node, "isLoggedIn", expectedStatement),
                node => NodeAsserter.AssertTextNode(node, "\n", "\r\n")
                );


    }

    [Fact]
    public void ForLoopTest()
    {
        string source = BasicHelper.GetForLoop();
        var ast = TemporaryTestHelper.GiveMeAst(source);
        Assert.Collection(ast.Body,
                node => NodeAsserter.AssertForBlockStatement(node, "items"),
                node => NodeAsserter.AssertTextNode(node, "\n", "\r\n")
                );


    }

    [Fact]
    public void ForLoopTestWithRecurse()
    {
        string source = BasicHelper.GetForLoop();
        var ast = TemporaryTestHelper.GiveMeAst(source);

        //expected
        var expectedStatement = new ExpectedStatement(3, [typeof(TextNode), typeof(KnightStatement), typeof(TextNode)]);

        Assert.Collection(ast.Body,
                node => NodeAsserter.AssertForBlockStatement(node, "items", expectedStatement),
                node => NodeAsserter.AssertTextNode(node, "\n", "\r\n")
                );


    }
    #endregion Valid Cases


    #region Invalid Cases
    // I should all test all the invalid cases with exception
    #endregion Invalid Cases
}
