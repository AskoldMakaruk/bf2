namespace MemeTalk.Lib.AST;

public abstract class AstStatement
{
    public interface IStatementVisitor<TResult>
    {
        TResult VisitBlockStmt(Block stmt);
        TResult VisitClassStmt(Class stmt);
        TResult VisitExpressionStmt(Expression stmt);
        TResult VisitFunctionStmt(Function stmt);
        TResult VisitIfStmt(If stmt);
        TResult VisitPrintStmt(Print stmt);
        TResult VisitReturnStmt(Return stmt);
        TResult? VisitVarStmt(Var stmt);
        TResult VisitWhileStmt(While stmt);
    }

    // Nested Stmt classes here...
    public class Block : AstStatement
    {
        public Block(List<AstStatement> statements)
        {
            this.Statements = statements;
        }

        public override TR Accept<TR>(IStatementVisitor<TR> statementVisitor)
        {
            return statementVisitor.VisitBlockStmt(this);
        }

        public readonly List<AstStatement> Statements;
    }

    public class Class : AstStatement
    {
        public Class(Token name, AstExpression.Variable superclass, List<AstStatement.Function> methods)
        {
            this.Name = name;
            this.Superclass = superclass;
            this.Methods = methods;
        }

        public override TR Accept<TR>(IStatementVisitor<TR> statementVisitor)
        {
            return statementVisitor.VisitClassStmt(this);
        }

        public readonly Token Name;
        public readonly AstExpression.Variable Superclass;
        public readonly List<AstStatement.Function> Methods;
    }

    public class Expression : AstStatement
    {
        public Expression(AstExpression astExpression)
        {
            this.AstExpression = astExpression;
        }

        public override TR Accept<TR>(IStatementVisitor<TR> statementVisitor)
        {
            return statementVisitor.VisitExpressionStmt(this);
        }

        public readonly AstExpression AstExpression;
    }

    public class Function : AstStatement
    {
        public Function(Token name, List<Token> @params, List<AstStatement> body)
        {
            this.Name = name;
            this.Params = @params;
            this.Body = body;
        }

        public override TR Accept<TR>(IStatementVisitor<TR> statementVisitor)
        {
            return statementVisitor.VisitFunctionStmt(this);
        }

        public readonly Token Name;
        public readonly List<Token> Params;
        public readonly List<AstStatement> Body;
    }

    public class If : AstStatement
    {
        public If(AstExpression condition, AstStatement thenBranch, AstStatement? elseBranch)
        {
            this.Condition = condition;
            this.ThenBranch = thenBranch;
            this.ElseBranch = elseBranch;
        }

        public override TR Accept<TR>(IStatementVisitor<TR> statementVisitor)
        {
            return statementVisitor.VisitIfStmt(this);
        }

        public readonly AstExpression Condition;
        public readonly AstStatement ThenBranch;
        public readonly AstStatement? ElseBranch;
    }

    public class Print : AstStatement
    {
        public Print(AstExpression astExpression)
        {
            this.AstExpression = astExpression;
        }

        public override TR Accept<TR>(IStatementVisitor<TR> statementVisitor)
        {
            return statementVisitor.VisitPrintStmt(this);
        }

        public readonly AstExpression AstExpression;
    }

    public class Return : AstStatement
    {
        public Return(Token keyword, AstExpression value)
        {
            this.Keyword = keyword;
            this.Value = value;
        }

        public override TR Accept<TR>(IStatementVisitor<TR> statementVisitor)
        {
            return statementVisitor.VisitReturnStmt(this);
        }

        public readonly Token Keyword;
        public readonly AstExpression Value;
    }

    public class Var : AstStatement
    {
        public Var(Token name, AstExpression initializer)
        {
            this.Name = name;
            this.Initializer = initializer;
        }

        public override TR Accept<TR>(IStatementVisitor<TR> statementVisitor)
        {
            return statementVisitor.VisitVarStmt(this);
        }

        public readonly Token Name;
        public readonly AstExpression? Initializer;
    }

    public class While : AstStatement
    {
        public While(AstExpression condition, AstStatement body)
        {
            this.Condition = condition;
            this.Body = body;
        }

        public override TR Accept<TR>(IStatementVisitor<TR> statementVisitor)
        {
            return statementVisitor.VisitWhileStmt(this);
        }

        public readonly AstExpression Condition;
        public readonly AstStatement Body;
    }

    public abstract TR Accept<TR>(IStatementVisitor<TR> statementVisitor);
}