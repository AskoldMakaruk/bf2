using System.Data;
using System.Globalization;
using Godot;
using MemeTalk.Lib.AST;

namespace MemeTalk.Lib;

public class Interpreter : AstExpression.IExpressionVisitor<object>,
    AstStatement.IStatementVisitor<object>
{
    public Action<string> Print { get; set; } = Console.WriteLine;
    public Action<string> Error { get; set; } = Console.Error.WriteLine;
    public Action<Func<double, double>> AddGraph { get; set; } = _ => { };
    public Action<Vector2, Color> AddPixel { get; set; } = (_, _) => { };

    private Environment _environment = new Environment(null);

    public void Interpret(List<AstStatement> statements)
    {
        foreach (var statement in statements)
        {
            try
            {
                Execute(statement);
            }
            catch (RuntimeError e)
            {
                MemeTalkLang.Instance.RuntimeError(e);
            }
            catch (Exception e)
            {
                MemeTalkLang.Instance.RuntimeError(new RuntimeError(e.Message));
            }
        }
    }

    private object Execute(AstStatement statement)
    {
        return statement.Accept(this);
    }

    public object? VisitAssignExpr(AstExpression.Assign expr)
    {
        var value = Evaluate(expr.Value);
        _environment.Assign(expr.Name, value);
        return value;
    }

    public object? VisitBinaryExpr(AstExpression.Binary expr)
    {
        var left = Evaluate(expr.Left);
        var right = Evaluate(expr.Right);

        return expr.Oper.Type switch
        {
            TokenType.MINUS => CheckNumbers((d, d1) => d - d1),
            TokenType.PLUS => Plus(),
            TokenType.SLASH => CheckNumbers((d, d1) => d / d1),
            TokenType.STAR => CheckNumbers((d, d1) => d * d1),
            TokenType.GREATER => CheckNumbers((d, d1) => d > d1),
            TokenType.GREATER_EQUAL => CheckNumbers((d, d1) => d >= d1),
            TokenType.LESS => CheckNumbers((d, d1) => d < d1),
            TokenType.LESS_EQUAL => CheckNumbers((d, d1) => d < d1),
            TokenType.BANG_EQUAL => !IsEqual(left, right),
            TokenType.EQUAL_EQUAL => IsEqual(left, right),
            TokenType.PERCENT => CheckNumbers((d, d1) => d % d1),
            _ => null
        };

        // todo toString for numbers
        object? Plus()
        {
            // cast to string if one of operands is string
            if (left is string || right is string)
            {
                return left.ToString() + right;
            }

            return left switch
            {
                double dleft when right is double dright => dleft + dright,
                string sleft when right is string sright => sleft + sright,
                _ => throw new RuntimeError(expr.Oper, "Operands must be two numbers or two strings.")
            };
        }

        bool IsEqual(object? a, object? b) =>
            a switch
            {
                null when b is null => true,
                null => false,
                _ => a.Equals(b)
            };

        object? CheckNumbers(Func<double, double, object?> func)
        {
            return CheckNumberOperands(expr.Oper, left, right, func);
        }
    }

    public object VisitCallExpr(AstExpression.Call expr)
    {
        throw new NotImplementedException();
    }

    public object VisitGetExpr(AstExpression.Get expr)
    {
        throw new NotImplementedException();
    }

    public object? VisitGroupingExpr(AstExpression.Grouping expr)
    {
        return Evaluate(expr.Expression);
    }

    public object? VisitLiteralExpr(AstExpression.Literal expr)
    {
        return expr.Value;
    }

    public object VisitLogicalExpr(AstExpression.Logical expr)
    {
        var left = Evaluate(expr.Left);

        if (expr.Oper.Type == TokenType.OR)
        {
            if (IsTruthy(left))
            {
                return left;
            }
        }
        else
        {
            if (!IsTruthy(left))
            {
                return left;
            }
        }

        return Evaluate(expr.Right);
    }

    public object VisitSetExpr(AstExpression.Set expr)
    {
        throw new NotImplementedException();
    }

    public object VisitSuperExpr(AstExpression.Super expr)
    {
        throw new NotImplementedException();
    }

    public object VisitThisExpr(AstExpression.This expr)
    {
        throw new NotImplementedException();
    }

    public object? VisitUnaryExpr(AstExpression.Unary expr)
    {
        var right = Evaluate(expr.Right);

        return expr.Oper.Type switch
        {
            TokenType.BANG => !(bool)right,
            TokenType.MINUS => CheckNumberOperand(expr.Oper, right, d => -d),
            _ => null
        };
    }

    public object VisitVariableExpr(AstExpression.Variable expr)
    {
        return _environment.Get(expr.Name);
    }


    private object? Evaluate(AstExpression expr)
    {
        return expr.Accept<object>(this);
    }

    private static bool IsTruthy(object? obj)
    {
        return obj switch
        {
            null => false,
            bool b => b,
            _ => true
        };
    }

    private static object? CheckNumberOperand(Token oper,
        object? operand,
        Func<double, object?> func)
    {
        if (operand is double l)
            return func(l);
        throw new RuntimeError(oper, "Operands must be numbers.");
    }

    private static object? CheckNumberOperands(Token oper,
        object? left,
        object? right,
        Func<double, double, object?> func)
    {
        if (left is double l && right is double r)
            return func(l, r);
        throw new RuntimeError(oper, "Operands must be numbers.");
    }

    private string Stringify(object? obj)
    {
        switch (obj)
        {
            case null:
                return "nil";
            case double d:
            {
                var text = d.ToString(CultureInfo.InvariantCulture);
                if (text.EndsWith(".0"))
                    text = text[..^2];
                return text;
            }
            default:
                return obj.ToString() ?? string.Empty;
        }
    }

    public object? VisitBlockStmt(AstStatement.Block stmt)
    {
        ExecuteBlock(stmt.Statements, new Environment(_environment));
        return null;
    }

    private void ExecuteBlock(List<AstStatement> stmtStatements, Environment environment)
    {
        var previous = _environment;
        try
        {
            _environment = environment;
            foreach (var statement in stmtStatements)
                Execute(statement);
        }
        finally
        {
            _environment = previous;
        }
    }

    public object VisitClassStmt(AstStatement.Class stmt)
    {
        throw new NotImplementedException();
    }

    public object VisitExpressionStmt(AstStatement.Expression stmt)
    {
        Evaluate(stmt.AstExpression);
        return null;
    }

    public object VisitFunctionStmt(AstStatement.Function stmt)
    {
        throw new NotImplementedException();
    }

    public object VisitIfStmt(AstStatement.If stmt)
    {
        if (IsTruthy(Evaluate(stmt.Condition)))
            return Execute(stmt.ThenBranch);
        else if (stmt.ElseBranch != null)
            return Execute(stmt.ElseBranch);

        return null;
    }

    public object VisitPrintStmt(AstStatement.Print stmt)
    {
        var val = Evaluate(stmt.AstExpression);
        Print(Stringify(val));
        return null;
    }

    public object VisitReturnStmt(AstStatement.Return stmt)
    {
        throw new NotImplementedException();
    }

    public object? VisitVarStmt(AstStatement.Var stmt)
    {
        object? value = null!;
        if (stmt.Initializer != null)
            value = Evaluate(stmt.Initializer);
        _environment.Define(stmt.Name.Lexeme, value);
        return null;
    }

    public object VisitWhileStmt(AstStatement.While stmt)
    {
        while (IsTruthy(Evaluate(stmt.Condition)))
        {
            Execute(stmt.Body);
        }

        return null;
    }

    public object VisitGraphStatement(AstStatement.Graph stmt)
    {
        AddGraph(d =>
        {
            _environment.Define(stmt.Input.Lexeme, d);
            return (double)Evaluate(stmt.Expression)!;
        });

        return null;
    }

    public object VisitPixelStatement(AstStatement.Pixel pixel)
    {
        var x = (int)(double)Evaluate(pixel.X);
        var y = (int)(double)Evaluate(pixel.Y);
        var parsedColor = (Color)Evaluate(pixel.Color)!;

        AddPixel(new Vector2(x, y), parsedColor);
        return null;
    }

    public object VisitColorStat(AstExpression.ColorStat colorStat)
    {
        Color parsedColor;
        if (colorStat.Color != null)
        {
            var str = Evaluate(colorStat.Color);
            parsedColor = (str as string) switch
            {
                "чорний" => Colors.Black,
                "білий" => Colors.White,
                "червоний" => Colors.Red,
                "зелений" => Colors.Green,
                "синій" => Colors.Blue,
                "жовтий" => Colors.Yellow,
                "рожевий" => Colors.Pink,
                "оранжевий" => Colors.Orange,
                "фіолетовий" => Colors.Purple,
                "сірий" => Colors.Gray,
                _ => Color.FromHtml(str as string)
            };
        }
        else
        {
            var r = (byte)(double)Evaluate(colorStat.R);
            var g = (byte)(double)Evaluate(colorStat.G);
            var b = (byte)(double)Evaluate(colorStat.B);
            parsedColor = Color.Color8(r, g, b);
        }

        return parsedColor;
    }

    public object VisitSinExpression(AstExpression.Sin sin)
    {
        var value = (double)Evaluate(sin.Value);
        return Math.Sin(value);
    }

    public object VisitSqrtExpression(AstExpression.Sqrt sqrt)
    {
        var value = (double)Evaluate(sqrt.Value);
        return Math.Sqrt(value);
    }

    public object VisitAbsExpression(AstExpression.Abs abs)
    {
        var value = (double)Evaluate(abs.Value);
        return Math.Abs(value);
    }
}

public class RuntimeError : Exception
{
    public Token Token { get; }

    public RuntimeError(string message) : base(message)
    {
    }

    public RuntimeError(Token token, string operandsMustBeNumbers) : base(operandsMustBeNumbers)
    {
        Token = token;
    }
}