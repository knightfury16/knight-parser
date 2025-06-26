namespace Knight.ParserCore;


public interface IReaderContext
{
    public int LineNumber { get; set; }
    public int CharacterPosition { get; set; }

}
