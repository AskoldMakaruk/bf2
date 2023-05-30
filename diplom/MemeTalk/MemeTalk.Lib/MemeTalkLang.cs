using MemeTalk.Lib.AST;

namespace MemeTalk.Lib;

public class MemeTalkLang
{
    public static MemeTalkLang Instance { get; } = new();

    public readonly Interpreter Interpreter = new();
    public string[] Source = Array.Empty<string>();

    public void RunFile(string path)
    {
        var source = File.ReadAllText(path);
        Run(source);

        if (HadError)
        {
            System.Environment.Exit(65);
        }

        if (HadRuntimeError)
        {
            System.Environment.Exit(70);
        }
    }

    public void RunPrompt()
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
            HadError = false;
        }
    }

    public void Run(string source)
    {
        this.Source = source.Split('\n');
        var scanner = new Tokenizer(source);
        var tokens = scanner.ScanTokens();
        var parser = new Parser(tokens.Tokens);
        var statements = parser.Parse();

        if (HadError)
        {
            return;
        }

        Interpreter.Interpret(statements);
    }

    public void Error(Token token, string message)
    {
        if (token.Type == TokenType.EOF)
        {
            Report(token.Line, " at end", message);
        }
        else
        {
            Report(token.Line, " at '" + token.Lexeme + "'", message);
        }
    }

    public void Error(int line, string message)
    {
        Report(line, "", message);
    }

    void Report(int line, string where, string message)
    {
        var sourceLine = "";
        if (Source.Length > 0 && line > Source.Length)
        {
            sourceLine = Source[line - 1];
        }

        Interpreter.Error($"[line {line}]{sourceLine}\nError: {where}: {message}");
        HadError = true;
    }

    public bool HadError = false;
    public bool HadRuntimeError = false;

    public void RuntimeError(RuntimeError runtimeError)
    {
        var line = runtimeError.Token?.Line ?? 0;
        var sourceLine = "";
        if (Source.Length > 0 && line > Source.Length)
        {
            sourceLine = Source[line - 1];
        }

        Interpreter.Error($"[line {line}]{sourceLine}\nError: {runtimeError.Message}");
        HadRuntimeError = true;
    }
}