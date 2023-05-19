using MemeTalk.Lib;
using MemeTalk.Lib.AST;


var expr = new AstExpression.Binary(
    new AstExpression.Unary(
        new Token(TokenType.MINUS, "-", null, 1),
        new AstExpression.Literal(123)),
    new Token(TokenType.STAR, "*", null, 1),
    new AstExpression.Grouping(
        new AstExpression.Literal(45.67)));

Console.WriteLine(new AstPrinter().Print(expr));

RunFile("main.mt");

void RunFile(string path)
{
    var source = File.ReadAllText(path);
    Run(source);

    if (global::Error.HadError)
    {
        Environment.Exit(65);
    }
}

void RunPrompt()
{
    while (true)
    {
        Console.Write("> ");
        var line = Console.ReadLine();
        if (line == null)
        {
            break;
        }

        Run(line);
        global::Error.HadError = false;
    }
}

void Run(string source)
{
    var scanner = new Tokenizer(source);
    var tokens = scanner.ScanTokens();

    // For now, just print the tokens.
    foreach (var token in tokens.Tokens)
    {
        Console.WriteLine(token);
    }
}

void Error(int line, string message)
{
    Report(line, "", message);
}

void Report(int line, string where, string message)
{
    Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
    global::Error.HadError = true;
}

public static class Error
{
    public static bool HadError = false;
}