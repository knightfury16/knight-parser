namespace Knight.ParserCore.Parser.Node;

public class EvaluatorVariable(string name)
{
    public string Name { get; set; } = name;
    public string? EvalutorEnumerator { get; set; }
}
