namespace MemeTalk.Lib.AST;

public abstract class AstExpression
{
    public abstract TResult? Accept<TResult>(IExpressionVisitor<TResult> visitor);

    public interface IExpressionVisitor<out TResult>
    {
        TResult? VisitAssignExpr(Assign expr);
        TResult? VisitBinaryExpr(Binary expr);
        TResult? VisitCallExpr(Call expr);
        TResult? VisitGetExpr(Get expr);
        TResult? VisitGroupingExpr(Grouping expr);
        TResult? VisitLiteralExpr(Literal expr);
        TResult? VisitLogicalExpr(Logical expr);
        TResult? VisitSetExpr(Set expr);
        TResult? VisitSuperExpr(Super expr);
        TResult? VisitThisExpr(This expr);
        TResult? VisitUnaryExpr(Unary expr);
        TResult? VisitVariableExpr(Variable expr);
    }

    // Nested Expr classes here...
    public class Assign : AstExpression
    {
        public Assign(Token name, AstExpression value)
        {
            this.Name = name;
            this.Value = value;
        }

        public override TResult? Accept<TResult>(IExpressionVisitor<TResult> visitor) where TResult : default
        {
            return visitor.VisitAssignExpr(this);
        }

        public readonly Token Name;
        public readonly AstExpression Value;
    }

    public class Binary : AstExpression
    {
        public Binary(AstExpression left, Token @oper, AstExpression right)
        {
            this.Left = left;
            this.Oper = oper;
            this.Right = right;
        }

        public override TResult Accept<TResult>(IExpressionVisitor<TResult> visitor) where TResult : default
        {
            return visitor.VisitBinaryExpr(this);
        }

        public readonly AstExpression Left;
        public readonly Token Oper;
        public readonly AstExpression Right;
    }

    public class Call : AstExpression
    {
        public Call(AstExpression callee, Token paren, List<AstExpression> arguments)
        {
            this.Callee = callee;
            this.Paren = paren;
            this.Arguments = arguments;
        }

        public override TResult Accept<TResult>(IExpressionVisitor<TResult> visitor) where TResult : default
        {
            return visitor.VisitCallExpr(this);
        }

        public readonly AstExpression Callee;
        public readonly Token Paren;
        public readonly List<AstExpression> Arguments;
    }

    public class Get : AstExpression
    {
        public Get(AstExpression obj, Token name)
        {
            this.Obj = obj;
            this.Name = name;
        }

        public override TResult Accept<TResult>(IExpressionVisitor<TResult> visitor) where TResult : default
        {
            return visitor.VisitGetExpr(this);
        }

        public readonly AstExpression Obj;
        public readonly Token Name;
    }

    public class Grouping : AstExpression
    {
        public Grouping(AstExpression expression)
        {
            this.Expression = expression;
        }

        public override TResult Accept<TResult>(IExpressionVisitor<TResult> visitor) where TResult : default
        {
            return visitor.VisitGroupingExpr(this);
        }

        public readonly AstExpression Expression;
    }

    public class Literal : AstExpression
    {
        public Literal(object value)
        {
            this.Value = value;
        }

        public override TResult Accept<TResult>(IExpressionVisitor<TResult> visitor) where TResult : default
        {
            return visitor.VisitLiteralExpr(this);
        }

        public readonly object? Value;
    }

    public class Logical : AstExpression
    {
        public Logical(AstExpression left, Token oper, AstExpression right)
        {
            this.Left = left;
            this.Oper = oper;
            this.Right = right;
        }

        public override TResult Accept<TResult>(IExpressionVisitor<TResult> visitor) where TResult : default
        {
            return visitor.VisitLogicalExpr(this);
        }

        public readonly AstExpression Left;
        public readonly Token Oper;
        public readonly AstExpression Right;
    }

    public class Set : AstExpression
    {
        public Set(AstExpression obj, Token name, AstExpression value)
        {
            this.Obj = obj;
            this.Name = name;
            this.Value = value;
        }

        public override TResult Accept<TResult>(IExpressionVisitor<TResult> visitor) where TResult : default
        {
            return visitor.VisitSetExpr(this);
        }

        public readonly AstExpression Obj;
        public readonly Token Name;
        public readonly AstExpression Value;
    }

    public class Super : AstExpression
    {
        public Super(Token keyword, Token method)
        {
            this.Keyword = keyword;
            this.Method = method;
        }

        public override TResult Accept<TResult>(IExpressionVisitor<TResult> visitor) where TResult : default
        {
            return visitor.VisitSuperExpr(this);
        }

        public readonly Token Keyword;
        public readonly Token Method;
    }

    public class This : AstExpression
    {
        public This(Token keyword)
        {
            this.Keyword = keyword;
        }

        public override TResult Accept<TResult>(IExpressionVisitor<TResult> visitor) where TResult : default
        {
            return visitor.VisitThisExpr(this);
        }

        public readonly Token Keyword;
    }

    public class Unary : AstExpression
    {
        public Unary(Token oper, AstExpression right)
        {
            this.Oper = oper;
            this.Right = right;
        }

        public override TResult Accept<TResult>(IExpressionVisitor<TResult> visitor) where TResult : default
        {
            return visitor.VisitUnaryExpr(this);
        }

        public readonly Token Oper;
        public readonly AstExpression Right;
    }

    public class Variable : AstExpression
    {
        public Variable(Token name)
        {
            this.Name = name;
        }

        public override TResult Accept<TResult>(IExpressionVisitor<TResult> visitor) where TResult : default
        {
            return visitor.VisitVariableExpr(this);
        }

        public readonly Token Name;
    }
}