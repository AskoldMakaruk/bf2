using Godot;
using System;

public partial class CodePageButton : Button
{
    public string Name { get; set; }

    public string Code { get; set; }
    
    public Action<string> OnPressed { get; set; } = _ => { };

    public override void _Ready()
    {
        Text = Name;
        base._Ready();
    }

    public void Pressed()
    {
        OnPressed(Code.Trim());
    }
}