using Godot;
using System;

public partial class EditorConsole : RichTextLabel
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void Print(string text)
    {
        AddText($"|> {text}\n");
    }

    public void Error(string text)
    {
        AddText($"|> [color=red]{text}[/color]\n");
    }
}