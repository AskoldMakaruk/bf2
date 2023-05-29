namespace MemeTalk.Lib;

public enum TokenType
{
// Single-character tokens.
    LEFT_PAREN,
    RIGHT_PAREN,
    LEFT_BRACE,
    RIGHT_BRACE,
    COMMA,
    DOT,
    MINUS,
    PLUS,
    SLASH,
    STAR,

// One or two character tokens.
    COLON,
    DOUBLE_COLON,
    SEMICOLON,
    DOUBLE_SEMICOLON,
    BANG,
    BANG_EQUAL,
    EQUAL,
    EQUAL_EQUAL,
    GREATER,
    GREATER_EQUAL,
    LESS,
    LESS_EQUAL,

// Literals.
    SYMBOL,
    IDENTIFIER,
    STRING,
    NUMBER,

// Keywords.
    ADRESSEE,
    ASSIGN,
    IS,
    AND,
    CLASS,
    ELSE,
    FALSE,
    FUN,
    FOR,
    IF,
    THEN,
    NIL,
    OR,
    PRINT,
    RETURN,
    SUPER,
    THIS,
    TRUE,
    VAR,
    WHILE,
    EOF,
    PERCENT
}