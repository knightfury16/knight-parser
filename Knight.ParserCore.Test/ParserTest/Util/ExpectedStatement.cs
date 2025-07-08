namespace Knight.ParserCore.Test.Util;

public class ExpectedStatement
{
    public int ConsequentStatementCount { get; set; } = 0;

    public int? AlternateStatementCount { get; set; } = null;

    public List<string>? ConsequentStatementsInOrder { get; set; } = null;

    public List<string>? AlternateStatementsInOrder { get; set; } = null;



    public ExpectedStatement(int consequentStatementCount)
    {
        ConsequentStatementCount = consequentStatementCount;
    }

    public ExpectedStatement(int consequentStatementCount, params Type[] consequentStatementsTypeInOrder) : this(consequentStatementCount)
    {

        ConsequentStatementsInOrder = new List<string>();

        foreach (var type in consequentStatementsTypeInOrder)
        {
            ConsequentStatementsInOrder.Add(type.ToString());
        }

    }

    public void SetAlternateStatementsInOrder(int alternateStatementCount, params Type[] alternateStatementsTypeInOrder)
    {
        AlternateStatementCount = alternateStatementCount;
        AlternateStatementsInOrder = new List<string>();
        foreach (var type in alternateStatementsTypeInOrder)
        {
            AlternateStatementsInOrder.Add(type.ToString());
        }
    }

}
