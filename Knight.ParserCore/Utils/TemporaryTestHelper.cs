using Knight.ParserCore.Parser.Node;

namespace Knight.ParserCore.Utils;

public class TemporaryTestHelper
{
    public static TemplateNode GiveMeAst(string source)
    {
        var extendedStringReader = new ExtendedStringReader(source);
        var tokens = Tokenizer.Tokenizer.Tokenize(extendedStringReader).ToList();
        var parser = new Knight.ParserCore.Parser.Parser(tokens);
        var ast = parser.ParseTemplate();
        return ast;
    }
}
