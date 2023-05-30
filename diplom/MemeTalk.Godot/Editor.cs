using Godot;
using Godot.Collections;
using MemeTalk.Lib;

/*
-- намалювати квадрат
    
    
 */


public partial class Editor : CodeEdit
{
    [Export] public EditorConsole Console;
    [Export] public OutputPanel ImagePanel;
    [Export] public HBoxContainer CodePagesContainer;
    public PackedScene CodePageButtonScene = ResourceLoader.Load<PackedScene>("res://code_page_button.tscn");

    public System.Collections.Generic.Dictionary<string, string> CodePages = new()
    {
        {
            "Квадрат 1", @"
нехай розмір є 151.
нехай у є 0.
поки у менше розмір то:
    нехай х є 0.
    поки х менше розмір то:
        якщо х % 10 дорівнює 0-- або у % 10 дорівнює 0
        то піксель х у ""білий"".
            інакше піксель х у ""чорний"".
            х є х +1!!    
        у є у + 1!!"
        },
        {
            "Трикутник", @"нехай розмір є 151.
нехай у є 0.
поки у менше розмір то:
    нехай х є 0.
    поки х менше розмір то:
        якщо х >= у і х + у < розмір
        то піксель х у ""синій"".
            інакше піксель х у ""білий"".
            х є х +1!!    
            у є у + 1!!"
        },
        {
            "Графік", @"
графік ч: ч * ч.
графік ч: -ч * ч.
графік ч: -ч * ч * ч.
графік ч: ч * ч * ч.

графік ч: ч * ч.
графік ч: модуль ч * ч * ч.

графік х: сінус х.
графік х: сінус -х.
графік х: -(корінь х).
графік х: (корінь х).
графік х: (корінь -х).
графік х: -(корінь -х).
"
        },
        {
            "Градієнт",
            @"
нехай розмір є 151.
нехай у є -100.
поки у менше розмір то:
    нехай х є 0.
    поки х менше розмір то:
        нехай колір є х / розмір * 255.
        піксель х у (255-колір) 0 (255-колір/2).
        х є х +1!!
    у є у + 1!!
            "
        },
        {
            "Піксельний сінус", @"
нехай розмір є 250.
нехай ширина_графіка є 4 * 3,14.
нехай масштаб є розмір / ширина_графіка.
нехай y_середина є розмір / 2.

нехай х є 0.
поки х менше ширина_графіка то:
    х є х + 0,01.
    нехай y є сінус х.    
    y є y * масштаб.    
    y є y_середина - y - 100.
    піксель (х * масштаб - 100) y ""білий"".!"
        }
    };

    public override void _Ready()
    {
        foreach (var codePage in CodePages)
        {
            var button = CodePageButtonScene.Instantiate<CodePageButton>();
            button.Name = codePage.Key;
            button.Code = codePage.Value;
            button.OnPressed = code => { Text = code; };
            CodePagesContainer.AddChild(button);
        }

        DelimiterComments = new Array<string>(new[]
        {
            "--"
        });

        MinimapDraw = false;

        var keywordColor = Color.Color8(198, 120, 221);
        var methodColor = Color.Color8(220, 220, 170);
        var stringColor = Color.Color8(214, 125, 73);


        var code = SyntaxHighlighter as CodeHighlighter;
        code.AddColorRegion("--", "", Color.Color8(114, 160, 60));
        code.AddColorRegion("\"", "\"", stringColor);
        foreach (var keyword in Tokenizer.Keywords)
        {
            code.AddKeywordColor(keyword.Key, keywordColor);
        }

        code.AddKeywordColor("друкувати", methodColor);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey { PhysicalKeycode: Key.F5 })
        {
            OnButtonPressed();
        }
        else
        {
            base._Input(@event);
        }
    }

    public override void _ShortcutInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventKey { PhysicalKeycode: Key.Space or Key.Pause, CtrlPressed: true })
        {
            var text = "нехай";
            AddCodeCompletionOption(CodeCompletionKind.Function, text, text);
            UpdateCodeCompletionOptions(true);
        }
        else if (inputEvent is InputEventKey { PhysicalKeycode: Key.Enter, CtrlPressed: true })
        {
            OnButtonPressed();
        }
        else
        {
            base._ShortcutInput(inputEvent);
        }
    }

    public override void _ConfirmCodeCompletion(bool replace)
    {
        var index = GetCodeCompletionSelectedIndex();
        var option = GetCodeCompletionOption(index);
        var text = option["insert_text"].ToString().Trim('\"');
        SelectWordUnderCaret();
        DeleteSelection();
        InsertTextAtCaret(text);
        UpdateCodeCompletionOptions(false);
    }

    public void OnButtonPressed()
    {
        var script = GetSelectedText();
        if (string.IsNullOrWhiteSpace(script))
        {
            script = Text;
        }

        ImagePanel.Clear();
        Console.Clear();
        var lang = new MemeTalkLang
        {
            Interpreter =
            {
                Print = Console.Print,
                Error = Console.Error,
                AddGraph = ImagePanel.AddGraph,
                AddPixel = ImagePanel.AddPixel
            }
        };
        lang.Run(script);
    }


    public void ConsolePrint(string text)
    {
        Console.Print(text);
    }
}