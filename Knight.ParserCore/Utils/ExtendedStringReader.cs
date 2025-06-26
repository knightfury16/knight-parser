
namespace Knight.ParserCore;


internal sealed class ExtendedStringReader : TextReader
{
    private int _lineNumber;
    private int _charPosition;
    private int _matched;

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
        // let us consider \r\n on one line but two separate different character

        if (Environment.NewLine[_matched] == c)
        {
            if (Environment.NewLine.Length > 1 && _matched == 0)
            {
                _matched++;
                _charPosition++;
                return;
            }

            _lineNumber++;
            _charPosition = 0;
            _matched = 0;
        }
        else
        {
            _charPosition++;

        }

    }

    public IReaderContext GetContext()
    {
        return new ReaderContext
        {
            LineNumber = _lineNumber,
            CharacterPosition = _charPosition
        };
    }


    internal class ReaderContext : IReaderContext
    {
        public int LineNumber { get; set; }
        public int CharacterPosition { get; set; }

        public override string ToString()
        {
            return $"LineNumber: {LineNumber}, CharacterPosition: {CharacterPosition}";
        }
    }
}


