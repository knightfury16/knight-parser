using Knight.ParserCore.Parser.Node;

namespace Knight.ParserCore.Utils;

public class TemporaryTestHelper
{
    public static TemplateNode GiveMeAst(string source)
    {
        var extendedStringReader = new ExtendedStringReader(source);
        var tokenizer = new Knight.ParserCore.Tokenizer.Tokenizer();
        var tokens = tokenizer.Tokenize(extendedStringReader).ToList();
        var parser = new Knight.ParserCore.Parser.Parser(tokens);
        var ast = parser.ParseTemplate();
        return ast;
    }
}
