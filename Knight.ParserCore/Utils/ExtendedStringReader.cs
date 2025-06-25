
namespace Knight.ParserCore;


internal sealed class ExtendedStringReader : TextReader
{
    private int _lineNumber;
    private int _charPosition;

    private readonly TextReader _inner;

    public ExtendedStringReader(TextReader reader)
    {
        _inner = reader;
    }

    public override int Peek()
    {
        return _inner.Peek();
    }

    public override int Read()
    {
        var c = _inner.Read();
        if (c >= 0) AdvancePosition((char)c);
        return c;
    }

    private void AdvancePosition(char c)
    {
        //if the character is of line break, increment _lineNumber and reset _charPosition
        if (Environment.NewLine[0] == c)
        {
            _lineNumber++;
            _charPosition = 0;
            return;
        }
        _charPosition++;
    }

    public CharacterContext GetContext()
    {
        return new CharacterContext
        {
            LineNumber = _lineNumber,
            CharacterPosition = _charPosition
        };
    }


    internal class CharacterContext
    {
        public int LineNumber { get; set; }
        public int CharacterPosition { get; set; }

        public override string ToString()
        {
            return $"LineNumber: {LineNumber}, CharacterPosition: {CharacterPosition}";
        }
    }
}


