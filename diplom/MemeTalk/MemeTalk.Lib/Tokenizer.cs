namespace MemeTalk.Lib;

public class ScanException : RuntimeError
{
    public ScanException(string message, int lineNumber) : base(message)
    {
    }
}

public class ParserException : RuntimeError
{
    private readonly ParseError _parseError;

    public ParserException(ParseError parseError) : base(parseError.Message)
    {
        _parseError = parseError;
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
        // { "є", TokenType.IS },
        { "і", TokenType.AND },
        { "клас", TokenType.CLASS },
        { "інакше", TokenType.ELSE },
        { "ні", TokenType.FALSE },
        { "функція", TokenType.FUN },
        { "для", TokenType.FOR },
        { "якщо", TokenType.IF },
        { "то", TokenType.THEN },
        { "нічого", TokenType.NIL },
        { "або", TokenType.OR },
        { "друкувати", TokenType.PRINT },
        { "повернути", TokenType.RETURN },
        { "супер", TokenType.SUPER },
        { "це", TokenType.THIS },
        { "так", TokenType.TRUE },
        { "нехай", TokenType.VAR },
        { "поки", TokenType.WHILE },
        { "є", TokenType.ASSIGN },
        { "більше", TokenType.GREATER },
        { "менше", TokenType.LESS },
        { "дорівнює", TokenType.EQUAL_EQUAL },
        { "графік", TokenType.GRAPH },
        { "піксель", TokenType.PIXEL },
        { "сінус", TokenType.SIN },
        { "завантажити", TokenType.LOAD },
        { "корінь", TokenType.SQRT },
        { "модуль", TokenType.ABS }
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
            case '{':
                AddToken(TokenType.LEFT_BRACE);
                break;
            case '}':
                AddToken(TokenType.RIGHT_BRACE);
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
            case ':':
                AddToken(Match(':') ? TokenType.DOUBLE_COLON : TokenType.COLON);
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
            case '%':
                AddToken(TokenType.PERCENT);
                break;
            case ' ':
            case '\r':
            case '\t':
                break;
            case '\n':
                _line++;
                break;
            case '\"':
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

        var text = _source.Substring(_start, _current - _start);
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

        var val = double.Parse(_source.Substring(_start, _current - _start));
        AddToken(TokenType.NUMBER, val);
    }

    private void ReadStringLiteral()
    {
        while (Peek() != '\"' && !IsAtEnd())
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
        var val = _source.Substring(_start + 1, _current - _start - 2);
        AddToken(TokenType.STRING, val);
    }

    private void ReadComment(char commentStart, TokenType type)
    {
        if (Match(commentStart))
        {
            while (Peek() != '\n' && !IsAtEnd())
            {
                var c = Advance();
                if (c == commentStart && Peek() == commentStart)
                {
                    Advance();
                    break;
                }
            }
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

    private bool IsAlpha(char c) =>
        // accept all english letters
        c is >= 'a' and <= 'z'
            or >= 'A' and <= 'Z'
            // accept all ukrainian letters
            or 'а' or 'б' or 'в' or 'г' or 'ґ' or 'д' or 'е' or 'є' or 'ж' or 'з' or 'и' or 'і' or 'ї' or 'й' or 'к' or 'л' or 'м' or 'н' or 'о' or 'п' or 'р' or 'с' or 'т' or 'у' or 'ф' or 'х' or 'ц' or 'ч' or 'ш' or 'щ' or 'ь' or 'ю' or 'я'
            or '_' or '\'';

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
        var text = _source.Substring(_start, _current - _start);
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