namespace MemeTalk.Lib.AST;

public abstract class AstStatement
{
    public interface IVisitor<TR>
    {
        TR VisitBlockStmt(Block stmt);
        TR VisitClassStmt(Class stmt);
        TR VisitExpressionStmt(Expression stmt);
        TR VisitFunctionStmt(Function stmt);
        TR VisitIfStmt(If stmt);
        TR VisitPrintStmt(Print stmt);
        TR VisitReturnStmt(Return stmt);
        TR VisitVarStmt(Var stmt);
        TR VisitWhileStmt(While stmt);
    }

    // Nested Stmt classes here...
    public class Block : AstStatement
    {
        public Block(List<AstStatement> statements)
        {
            this.statements = statements;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitBlockStmt(this);
        }

        public readonly List<AstStatement> statements;
    }

    public class Class : AstStatement
    {
        public Class(Token name, AstExpression.Variable superclass, List<AstStatement.Function> methods)
        {
            this.name = name;
            this.superclass = superclass;
            this.methods = methods;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitClassStmt(this);
        }

        public readonly Token name;
        public readonly AstExpression.Variable superclass;
        public readonly List<AstStatement.Function> methods;
    }

    public class Expression : AstStatement
    {
        public Expression(AstExpression expression)
        {
            this.expression = expression;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitExpressionStmt(this);
        }

        public readonly AstExpression expression;
    }

    public class Function : AstStatement
    {
        public Function(Token name, List<Token> @params, List<AstStatement> body)
        {
            this.name = name;
            this.@params = @params;
            this.body = body;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitFunctionStmt(this);
        }

        public readonly Token name;
        public readonly List<Token> @params;
        public readonly List<AstStatement> body;
    }

    public class If : AstStatement
    {
        public If(AstExpression condition, AstStatement thenBranch, AstStatement elseBranch)
        {
            this.condition = condition;
            this.thenBranch = thenBranch;
            this.elseBranch = elseBranch;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitIfStmt(this);
        }

        public readonly AstExpression condition;
        public readonly AstStatement thenBranch;
        public readonly AstStatement elseBranch;
    }

    public class Print : AstStatement
    {
        public Print(AstExpression expression)
        {
            this.expression = expression;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitPrintStmt(this);
        }

        public readonly AstExpression expression;
    }

    public class Return : AstStatement
    {
        public Return(Token keyword, AstExpression value)
        {
            this.keyword = keyword;
            this.value = value;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitReturnStmt(this);
        }

        public readonly Token keyword;
        public readonly AstExpression value;
    }

    public class Var : AstStatement
    {
        public Var(Token name, AstExpression initializer)
        {
            this.name = name;
            this.initializer = initializer;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitVarStmt(this);
        }

        public readonly Token name;
        public readonly AstExpression initializer;
    }

    public class While : AstStatement
    {
        public While(AstExpression condition, AstStatement body)
        {
            this.condition = condition;
            this.body = body;
        }

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitWhileStmt(this);
        }

        public readonly AstExpression condition;
        public readonly AstStatement body;
    }

    public abstract R Accept<R>(IVisitor<R> visitor);
}