using Godot;
using MemeTalk.Lib.AST;

namespace MemeTalk.Lib;

public class Parser
{
    public Parser(List<Token> tokens)
    {
        Tokens = tokens;
    }

    public List<Token> Tokens { get; private set; }
    public int Position { get; private set; }

    public List<AstStatement> Parse()
    {
        var statements = new List<AstStatement>();
        while (!IsAtEnd())
        {
            statements.Add(Declaration()!);
        }

        return statements;
    }

    private AstStatement? Declaration()
    {
        try
        {
            return Match(TokenType.VAR) ? VarDeclaration() : Statement();
        }
        catch (ParserException e)
        {
            Synchronize();
            return null;
        }
    }

    private AstStatement VarDeclaration()
    {
        var name = Consume(TokenType.IDENTIFIER, "Expect variable name.");

        AstExpression? initializer = null;
        if (Match(TokenType.ASSIGN))
        {
            initializer = Expression();
        }

        Consume(TokenType.DOT, "Expect '.' after variable declaration.");
        return new AstStatement.Var(name, initializer);
    }

    private AstStatement Statement()
    {
        if (Match(TokenType.IF))
        {
            return IfStatement();
        }

        if (Match(TokenType.PRINT))
        {
            return PrintStatement();
        }

        if (Match(TokenType.WHILE))
        {
            return WhileStatement();
        }

        if (Match(TokenType.LEFT_BRACE)) return BlockStatement();
        if (Match(TokenType.COLON))
        {
            return ColonBlockStatement();
        }

        if (Match(TokenType.GRAPH))
        {
            return GraphStatement();
        }

        if (Match(TokenType.PIXEL))
        {
            return PixelStatement();
        }

        return ExpressionStatement();
    }

    private AstStatement PixelStatement()
    {
        var x = Expression();
        var y = Expression();
        var color = ColorExpression();
        Consume(TokenType.DOT, "Expect '.' after pixel statement.");
        return new AstStatement.Pixel(x, y, color);
    }

    private AstExpression ColorExpression()
    {
        if (Check(TokenType.STRING))
        {
            return new AstExpression.ColorStat(Expression());
        }

        return new AstExpression.ColorStat(Expression(), Expression(), Expression());
    }

    private AstStatement GraphStatement()
    {
        var token = Consume(TokenType.IDENTIFIER, "Expect input name.");
        Consume(TokenType.COLON, "Expect ':' after input name.");
        var input = Expression();
        Consume(TokenType.DOT, "Expect '.' after input expression.");
        return new AstStatement.Graph(token, input);
    }

    private AstStatement ExpressionStatement()
    {
        var expr = Expression();
        if (!Match(TokenType.BANG, TokenType.DOT))
        {
            throw Error(Peek(), "Expect '.' or '!' after expression.");
        }

        return new AstStatement.Expression(expr);
    }

    private AstStatement PrintStatement()
    {
        var value = Expression();
        if (!Match(TokenType.BANG, TokenType.DOT))
        {
            throw Error(Peek(), "Expect '.' or '!' after expression.");
        }

        return new AstStatement.Print(value);
    }

    private AstStatement IfStatement()
    {
        var condition = Expression();
        Consume(TokenType.THEN, "Expect 'then' after if condition.");
        var thenBranch = Statement();
        AstStatement? elseBranch = null;
        if (Match(TokenType.ELSE))
        {
            elseBranch = Statement();
        }

        return new AstStatement.If(condition, thenBranch, elseBranch);
    }

    private AstStatement WhileStatement()
    {
        var condition = Expression();
        Consume(TokenType.THEN, "Expect 'then' after while condition.");
        var body = Statement();
        return new AstStatement.While(condition, body);
    }

    private AstStatement ColonBlockStatement()
    {
        var statements = new List<AstStatement>();

        while (!Check(TokenType.BANG) && !IsAtEnd())
        {
            statements.Add(Declaration()!);
        }

        Consume(TokenType.BANG, "Expect '!' after block.");
        return new AstStatement.Block(statements);
    }

    private AstStatement BlockStatement()
    {
        var statements = new List<AstStatement>();

        while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
        {
            statements.Add(Declaration()!);
        }

        Consume(TokenType.RIGHT_BRACE, "Expect '}' after block.");
        return new AstStatement.Block(statements);
    }

    private AstExpression Expression()
    {
        return Assignment();
    }

    private AstExpression Assignment()
    {
        var expr = Sin();

        if (Match(TokenType.ASSIGN))
        {
            var equals = Previous();
            var value = Assignment();

            if (expr is AstExpression.Variable variable)
            {
                var name = variable.Name;
                return new AstExpression.Assign(name, value);
            }

            Error(equals, "Invalid assignment target.");
        }

        return expr;
    }

    private AstExpression Sin()
    {
        if (Match(TokenType.SIN))
        {
            var value = Abs();
            return new AstExpression.Sin(value);
        }

        return Abs();
    }

    private AstExpression Abs()
    {
        if (Match(TokenType.ABS))
        {
            var value = Expression();
            return new AstExpression.Abs(value);
        }

        return Sqrt();
    }

    private AstExpression Sqrt()
    {
        if (Match(TokenType.SQRT))
        {
            var value = Expression();
            return new AstExpression.Sqrt(value);
        }

        return Or();
    }

    private AstExpression Or()
    {
        var expr = And();

        while (Match(TokenType.OR))
        {
            var @operator = Previous();
            var right = And();
            expr = new AstExpression.Logical(expr, @operator, right);
        }

        return expr;
    }

    private AstExpression And()
    {
        var expr = Equality();

        while (Match(TokenType.AND))
        {
            var @operator = Previous();
            var right = Equality();
            expr = new AstExpression.Logical(expr, @operator, right);
        }

        return expr;
    }

    private AstExpression Equality()
    {
        var expr = Comparison();

        var ops = new TokenType[] { TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL };
        while (Match(ops))
        {
            var op = Previous();
            var right = Comparison();
            expr = new AstExpression.Binary(expr, op, right);
        }

        return expr;
    }

    private AstExpression Comparison()
    {
        var expr = Term();
        var ops = new TokenType[] { TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL };
        while (Match(ops))
        {
            var op = Previous();
            var right = Term();
            expr = new AstExpression.Binary(expr, op, right);
        }

        return expr;
    }

    private AstExpression Term()
    {
        var expr = Remainder();

        var ops = new TokenType[] { TokenType.MINUS, TokenType.PLUS };
        while (Match(ops))
        {
            var op = Previous();
            var right = Remainder();
            expr = new AstExpression.Binary(expr, op, right);
        }

        return expr;
    }

    private AstExpression Remainder()
    {
        var expr = Factor();

        var ops = new TokenType[] { TokenType.PERCENT };
        while (Match(ops))
        {
            var op = Previous();
            var right = Factor();
            expr = new AstExpression.Binary(expr, op, right);
        }

        return expr;
    }

    private AstExpression Factor()
    {
        var expr = Unary();

        var ops = new TokenType[] { TokenType.SLASH, TokenType.STAR };
        while (Match(ops))
        {
            var op = Previous();
            var right = Unary();
            expr = new AstExpression.Binary(expr, op, right);
        }

        return expr;
    }

    private AstExpression Unary()
    {
        var ops = new TokenType[] { TokenType.BANG, TokenType.MINUS };
        while (Match(ops))
        {
            var op = Previous();
            var right = Unary();
            return new AstExpression.Unary(op, right);
        }

        return Primary();
    }

    private AstExpression Primary()
    {
        if (Match(TokenType.FALSE)) return new AstExpression.Literal(false);
        if (Match(TokenType.TRUE)) return new AstExpression.Literal(true);
        if (Match(TokenType.NIL)) return new AstExpression.Literal(null);

        if (Match(TokenType.NUMBER, TokenType.STRING))
        {
            return new AstExpression.Literal(Previous().Literal);
        }

        if (Match(TokenType.IDENTIFIER))
        {
            return new AstExpression.Variable(Previous());
        }

        if (Match(TokenType.LEFT_PAREN))
        {
            var expr = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
            return new AstExpression.Grouping(expr);
        }

        throw Error(Peek(), "Expect expression.");
    }

    private Token Consume(TokenType type, string message)
    {
        if (Check(type)) return Advance();
        throw Error(Peek(), message);
    }

    private bool Match(params TokenType[] types)
    {
        foreach (var type in types)
        {
            if (Check(type))
            {
                Advance();
                return true;
            }
        }

        return false;
    }

    private bool Check(TokenType type)
    {
        if (IsAtEnd()) return false;
        return Peek().Type == type;
    }

    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EOF;
    }

    private Token Peek()
    {
        return Tokens[Position];
    }

    private Token Advance()
    {
        if (!IsAtEnd()) Position++;
        return Previous();
    }

    private Token Previous()
    {
        return Tokens[Position - 1];
    }

    private ParserException Error(Token token, string message)
    {
        MemeTalkLang.Instance.Error(token, message);
        return new(new ParseError());
    }

    private void Synchronize()
    {
        Advance();

        while (!IsAtEnd())
        {
            if (Previous().Type == TokenType.SEMICOLON) return;

            switch (Peek().Type)
            {
                case TokenType.CLASS:
                case TokenType.FUN:
                case TokenType.VAR:
                case TokenType.FOR:
                case TokenType.IF:
                case TokenType.WHILE:
                case TokenType.PRINT:
                case TokenType.RETURN:
                    return;
            }

            Advance();
        }
    }
}