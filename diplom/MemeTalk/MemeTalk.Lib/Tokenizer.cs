namespace MemeTalk.Lib;

public class ScanException : Exception
{
    public ScanException(string message, int lineNumber) : base(message)
    {
    }
}

public readonly record struct TokenSelection(int Line, int Start, int Length);

public readonly record struct ParseError(string Message, TokenSelection Selection);

public record TokenizedSource(string Source, List<Token> Tokens, List<ParseError> Errors);

public class Tokenizer
{
    public static readonly IReadOnlyDictionary<string, TokenType> Keywords = new Dictionary<string, TokenType>()
    {
        { "приймає", TokenType.ADRESSEE },
        { "є", TokenType.IS },
        { "і", TokenType.AND },
        { "клас", TokenType.CLASS },
        { "інакше", TokenType.ELSE },
        { "ні", TokenType.FALSE },
        { "функція", TokenType.FUN },
        { "для", TokenType.FOR },
        { "якщо", TokenType.IF },
        { "нічого", TokenType.NIL },
        { "або", TokenType.OR },
        { "друкувати", TokenType.PRINT },
        { "повернути", TokenType.RETURN },
        { "супер", TokenType.SUPER },
        { "це", TokenType.THIS },
        { "так", TokenType.TRUE },
        { "змінна", TokenType.VAR },
        { "поки", TokenType.WHILE }
    };

    private readonly string _source;
    private readonly List<Token> _tokens = new();
    private readonly List<ParseError> _errors = new();
    private int _start = 0;
    private int _current = 0;
    private int _line = 1;

    public Tokenizer(string source)
    {
        _source = source;
    }

    public TokenizedSource ScanTokens()
    {
        while (!IsAtEnd())
        {
            // We are at the beginning of the next lexeme.
            _start = _current;
            ScanToken();
        }

        _tokens.Add(new Token(TokenType.EOF, "", null, _line));
        return new TokenizedSource(_source, _tokens, _errors);
    }

    private void ScanToken()
    {
        var c = Advance();
        switch (c)
        {
            case '(':
                AddToken(TokenType.LEFT_PAREN);
                break;
            case ')':
                AddToken(TokenType.RIGHT_PAREN);
                break;
            case ',':
                AddToken(TokenType.COMMA);
                break;
            case '.':
                AddToken(TokenType.DOT);
                break;
            case '+':
                AddToken(TokenType.PLUS);
                break;
            case ';':
                AddToken(TokenType.SEMICOLON);
                break;
            case '*':
                AddToken(TokenType.STAR);
                break;
            case '!':
                AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                break;
            case '=':
                AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;
            case '<':
                AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
            case '>':
                AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;
            case '/':
                ReadComment('/', TokenType.SLASH);
                break;
            case '-':
                ReadComment('-', TokenType.MINUS);
                break;
            case ' ':
            case '\r':
            case '\t':
                break;
            case '\n':
                _line++;
                break;
            case '\'':
                ReadStringLiteral();
                break;
            default:
                if (IsDigit(c))
                {
                    ReadNumberLiteral();
                }
                else if (IsAlpha(c))
                {
                    ReadIdentifier();
                }
                else
                {
                    AddError("Unexpected character.");
                }

                break;
        }
    }

    private void ReadIdentifier()
    {
        while (IsAlphaNumeric(Peek())) Advance();

        var text = _source.Substring(_start, _current);
        if (!Keywords.TryGetValue(text, out var type)) type = TokenType.IDENTIFIER;
        AddToken(type);
    }

    private void ReadNumberLiteral()
    {
        while (IsDigit(Peek())) Advance();

        if (Peek() == ',' && IsDigit(PeekNext()))
        {
            Advance();
            while (IsDigit(Peek())) Advance();
        }

        var val = double.Parse(_source.Substring(_start, _current));
        AddToken(TokenType.NUMBER, val);
    }

    private void ReadStringLiteral()
    {
        while (Peek() != '\'' && !IsAtEnd())
        {
            if (Peek() == '\n') _line++;
            Advance();
        }

        if (IsAtEnd())
        {
            AddError("Unterminated string.");
            return;
        }

        // The closing '.
        Advance();
        var val = _source.Substring(_start + 1, _current - 1);
        AddToken(TokenType.STRING, val);
    }

    private void ReadComment(char commentStart, TokenType type)
    {
        if (Match(commentStart))
        {
            while (Peek() != '\n' && !IsAtEnd()) Advance();
        }
        else
        {
            AddToken(type);
        }
    }


    private bool Match(char expected)
    {
        if (IsAtEnd() || _source[_current] != expected)
        {
            return false;
        }

        _current++;
        return true;
    }

    private bool IsDigit(char c)
    {
        return c is >= '0' and <= '9';
    }

    private bool IsAlpha(char c)
    {
        return c is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or '_';
    }

    private bool IsAlphaNumeric(char c)
    {
        return IsAlpha(c) || IsDigit(c);
    }

    private char Advance()
    {
        return _source[_current++];
    }

    private char Peek()
    {
        return IsAtEnd() ? '\0' : _source[_current];
    }

    private char PeekNext()
    {
        return _current + 1 >= _source.Length ? '\0' : _source[_current + 1];
    }

    private void AddToken(TokenType type)
    {
        AddToken(type, null);
    }

    private void AddToken(TokenType type, object literal)
    {
        var text = _source.Substring(_start, _current);
        _tokens.Add(new Token(type, text, literal, _line));
    }

    private void AddError(string message)
    {
        var length = _current - _start;
        _errors.Add(new ParseError(message, new TokenSelection(_line, _current, length)));
    }

    private bool IsAtEnd()
    {
        return _current >= _source.Length;
    }
}