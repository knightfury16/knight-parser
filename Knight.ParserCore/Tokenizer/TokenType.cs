namespace Knight.ParserCore.Tokenizer;

public enum TokenType
{
    Static = 0, // normal string. ex: Hello there
    StartExpression = 1, // {{, this is start expression
    EndExpression = 2, // }}, this is end expression
    BlockWord = 3, // #if,#endif,#for,#endfor, these are block word
    BlockAlias = 4, // {{#for ~name in names}}, ~name this is the block alias. Default alias value will be this
    Variable = 5, // {{name}}, name is variable here
    Comment = 6 // will support later
}
