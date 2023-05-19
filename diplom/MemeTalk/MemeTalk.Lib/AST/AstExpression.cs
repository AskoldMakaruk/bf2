namespace MemeTalk.Lib.AST;

public abstract class AstExpression
{
    public abstract TResult Accept<TVisitor, TResult>()
        where TVisitor : IExpressionVisitor<TResult>;

    public interface IExpressionVisitor<TR>
    {
        static abstract TR VisitAssignExpr(Assign expr);
        static abstract TR VisitBinaryExpr(Binary expr);
        static abstract TR VisitCallExpr(Call expr);
        static abstract TR VisitGetExpr(Get expr);
        static abstract TR VisitGroupingExpr(Grouping expr);
        static abstract TR VisitLiteralExpr(Literal expr);
        static abstract TR VisitLogicalExpr(Logical expr);
        static abstract TR VisitSetExpr(Set expr);
        static abstract TR VisitSuperExpr(Super expr);
        static abstract TR VisitThisExpr(This expr);
        static abstract TR VisitUnaryExpr(Unary expr);
        static abstract TR VisitVariableExpr(Variable expr);
    }

    // Nested Expr classes here...
    public class Assign : AstExpression
    {
        public Assign(Token name, AstExpression value)
        {
            this.Name = name;
            this.Value = value;
        }

        public override TResult Accept<TVisitor, TResult>()
        {
            return TVisitor.VisitAssignExpr(this);
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

        public override TResult Accept<TVisitor, TResult>()
        {
            return TVisitor.VisitBinaryExpr(this);
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

        public override TResult Accept<TVisitor, TResult>()
        {
            return TVisitor.VisitCallExpr(this);
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

        public override TResult Accept<TVisitor, TResult>()
        {
            return TVisitor.VisitGetExpr(this);
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

        public override TResult Accept<TVisitor, TResult>()
        {
            return TVisitor.VisitGroupingExpr(this);
        }

        public readonly AstExpression Expression;
    }

    public class Literal : AstExpression
    {
        public Literal(object value)
        {
            this.Value = value;
        }

        public override TResult Accept<TVisitor, TResult>()
        {
            return TVisitor.VisitLiteralExpr(this);
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

        public override TResult Accept<TVisitor, TResult>()
        {
            return TVisitor.VisitLogicalExpr(this);
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

        public override TResult Accept<TVisitor, TResult>()
        {
            return TVisitor.VisitSetExpr(this);
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

        public override TResult Accept<TVisitor, TResult>()
        {
            return TVisitor.VisitSuperExpr(this);
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

        public override TResult Accept<TVisitor, TResult>()
        {
            return TVisitor.VisitThisExpr(this);
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

        public override TResult Accept<TVisitor, TResult>()
        {
            return TVisitor.VisitUnaryExpr(this);
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

        public override TResult Accept<TVisitor, TResult>()
        {
            return TVisitor.VisitVariableExpr(this);
        }

        public readonly Token Name;
    }
}