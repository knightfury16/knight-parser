namespace Knight.ParserCore.Test.Fixtures.Basic;


public static class BasicHelper
{
    public static string GetNoAction()
    {
        var source = "Hello world.\r\nThe new world is the best world.\r\n";
        return source;
    }

    public static string GetSimpleVariable()
    {
        var source = "Hello, {{userName}}!\n";
        return source;
    }

    public static string GetIfCondition()
    {
        var source = File.ReadAllText("./Fixtures/Basic/IfCondition.text");
        return source;
    }

    public static string GetIfElseCondition()
    {
        var source = File.ReadAllText("./Fixtures/Basic/IfelseBlock.text");
        return source;
    }

    public static string GetForLoop()
    {
        var source = File.ReadAllText("./Fixtures/Basic/ForLoop.text");
        return source;
    }
}
