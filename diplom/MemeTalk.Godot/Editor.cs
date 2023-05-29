using Godot;
using System;
using Godot.Collections;
using MemeTalk.Lib;

public partial class Editor : CodeEdit
{
    [Export()] public EditorConsole Console;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Text = @"нехай вік є 0.
        якщо вік більше 1 
        то друкувати ""1 або більше"".
            інакше вік є 3.

            друкувати вік.";

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
        var lang = new MemeTalkLang
        {
            Interpreter =
            {
                Print = Console.Print,
                Error = Console.Error
            }
        };
        lang.Run(Text);
        // var lang =  
    }

    public void ConsolePrint(string text)
    {
        Console.Print(text);
    }
}