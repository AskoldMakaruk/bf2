using MemeTalk.Lib;
using MemeTalk.Lib.AST;


var expr = new AstExpression.Binary(
    new AstExpression.Unary(
        new Token(TokenType.MINUS, "-", null, 1),
        new AstExpression.Literal(123)),
    new Token(TokenType.STAR, "*", null, 1),
    new AstExpression.Grouping(
        new AstExpression.Literal(45.67)));

// Console.WriteLine(new AstPrinter().Print(expr));

var lang = new MemeTalkLang();

var code = File.ReadAllText("scope.mt");
// lang.RunPrompt();
// lang.RunFile("main.mt");
lang.Run(code);