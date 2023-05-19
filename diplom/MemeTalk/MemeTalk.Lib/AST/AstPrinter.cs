using System.Text;

namespace MemeTalk.Lib.AST;

public class AstPrinter : AstExpression.IExpressionVisitor<string>
{
    public string Print(AstExpression expression)
    {
        return expression.Accept<AstPrinter, string>();
    }

    public static string VisitAssignExpr(AstExpression.Assign expr)
    {
        throw new NotImplementedException();
    }

    public static string VisitBinaryExpr(AstExpression.Binary expr)
    {
        return Parenthesize(expr.Oper.Lexeme, expr.Left, expr.Right);
    }

    public static string VisitCallExpr(AstExpression.Call expr)
    {
        var arguments = new List<AstExpression>()
        {
            expr.Callee
        };
        arguments.AddRange(expr.Arguments);
        return Parenthesize("call", arguments.ToArray());
    }

    public static string VisitGetExpr(AstExpression.Get expr)
    {
        throw new NotImplementedException();
    }

    public static string VisitGroupingExpr(AstExpression.Grouping expr)
    {
        return Parenthesize("group", expr.Expression);
    }

    public static string VisitLiteralExpr(AstExpression.Literal expr)
    {
        return expr.Value == null ? "нічо" : expr.Value.ToString()!;
    }

    public static string VisitLogicalExpr(AstExpression.Logical expr)
    {
        throw new NotImplementedException();
    }

    public static string VisitSetExpr(AstExpression.Set expr)
    {
        throw new NotImplementedException();
    }

    public static string VisitSuperExpr(AstExpression.Super expr)
    {
        throw new NotImplementedException();
    }

    public static string VisitThisExpr(AstExpression.This expr)
    {
        throw new NotImplementedException();
    }

    public static string VisitUnaryExpr(AstExpression.Unary expr)
    {
        return Parenthesize(expr.Oper.Lexeme, expr.Right);
    }

    public static string VisitVariableExpr(AstExpression.Variable expr)
    {
        throw new NotImplementedException();
    }

    private static string Parenthesize(string name, params AstExpression[] exprs)
    {
        var builder = new StringBuilder();
        builder.Append('(').Append(name);
        foreach (var expr in exprs)
        {
            builder.Append(' ');
            builder.Append(expr.Accept<AstPrinter, string>());
        }

        builder.Append(')');
        return builder.ToString();
    }
}