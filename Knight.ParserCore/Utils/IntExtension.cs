jnamespace Knight.ParserCore.Utils;

public static class IntExtension
{
    
    /// <summary>
    /// Converts an integer to its corresponding Unicode character.
    /// </summary>
    /// <param name="value">The integer value representing a Unicode code point.</param>
    /// <returns>The character corresponding to the Unicode code point.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is outside the valid Unicode character range.</exception>
    public static char ToChar(this int value)
    {
        if (value < 0 || value > 0xFFFF)
        {
            throw new ArgumentOutOfRangeException(nameof(value),
                "Value must be between 0 and 65535 to be converted to a Unicode character.");
        }
        return (char)value;
    }
}
