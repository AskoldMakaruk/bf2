using System.Text;

namespace MemeTalk.Lib.AST;

public class GenerateAst
{
    public static void Generate()
    {
        var args = new [] { "out" };
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: generate_ast <output directory>");
            Environment.Exit(64);
        }

        var outputDir = args[0];

        DefineAst(outputDir, "Expr", new List<string>
        {
            "Assign   : Token name, Expr value",
            "Binary   : Expr left, Token operator, Expr right",
            "Call     : Expr callee, Token paren, List<Expr> arguments",
            "Get      : Expr object, Token name",
            "Grouping : Expr expression",
            "Literal  : Object value",
            "Logical  : Expr left, Token operator, Expr right",
            "Set      : Expr object, Token name, Expr value",
            "Super    : Token keyword, Token method",
            "This     : Token keyword",
            "Unary    : Token operator, Expr right",
            "Variable : Token name"
        });

        DefineAst(outputDir, "Stmt", new List<string>
        {
            "Block      : List<Stmt> statements",
            "Class      : Token name, Expr.Variable superclass, List<Stmt.Function> methods",
            "Expression : Expr expression",
            "Function   : Token name, List<Token> params, List<Stmt> body",
            "If         : Expr condition, Stmt thenBranch, Stmt elseBranch",
            "Print      : Expr expression",
            "Return     : Token keyword, Expr value",
            "Var        : Token name, Expr initializer",
            "While      : Expr condition, Stmt body"
        });
    }

    private static void DefineAst(string outputDir, string baseName, List<string> types)
    {
        Directory.CreateDirectory(outputDir);
        var path = outputDir + "/" + baseName + ".cs";
        using var writer = new StreamWriter(path, false, Encoding.UTF8);

        writer.WriteLine("using System.Collections.Generic;");
        writer.WriteLine();
        writer.WriteLine("abstract class " + baseName + " {");

        DefineVisitor(writer, baseName, types);

        writer.WriteLine();
        writer.WriteLine("  // Nested " + baseName + " classes here...");

        foreach (var type in types)
        {
            var className = type.Split(":")[0].Trim();
            var fields = type.Split(":")[1].Trim();
            DefineType(writer, baseName, className, fields);
        }

        writer.WriteLine();
        writer.WriteLine("  public abstract R Accept<R>(Visitor<R> visitor);");

        writer.WriteLine("}");
    }

    private static void DefineVisitor(TextWriter writer, string baseName, List<string> types)
    {
        writer.WriteLine("  public interface Visitor<R> {");

        foreach (var typeName in types.Select(type => type.Split(":")[0].Trim()))
        {
            writer.WriteLine("    R Visit" + typeName + baseName + "(" +
                             typeName + " " + baseName.ToLower() + ");");
        }

        writer.WriteLine("  }");
    }

    private static void DefineType(TextWriter writer,
        string baseName,
        string className,
        string fieldList)
    {
        writer.WriteLine("  public class " + className + " : " +
                         baseName + " {");

        // Constructor.
        writer.WriteLine("    public " + className + "(" + fieldList + ") {");

        // Store parameters in fields.
        var fields = fieldList.Split(", ");
        foreach (var field in fields)
        {
            var name = field.Split(" ")[1];
            writer.WriteLine("      this." + name + " = " + name + ";");
        }

        writer.WriteLine("    }");

        // Visitor pattern.
        writer.WriteLine();
        writer.WriteLine("    public override R Accept<R>(Visitor<R> visitor) {");
        writer.WriteLine("      return visitor.Visit" +
                         className + baseName + "(this);");
        writer.WriteLine("    }");

        // Fields.
        writer.WriteLine();
        foreach (var field in fields)
        {
            writer.WriteLine("    public readonly " + field + ";");
        }

        writer.WriteLine("  }");
    }
}