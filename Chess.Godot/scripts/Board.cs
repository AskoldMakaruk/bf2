using Godot;
using System;

public partial class Board : Godot.GridContainer
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        for (var i = 0; i < 64; i++)
        {
            var row = i / 8;
            var colorRect = new ColorRect()
            {
                Color = (i + row) % 2 == 0 ? new Color(0.5f, 0.5f, 0.5f) : new Color(0.8f, 0.8f, 0.8f),
                SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill
            };
            AddChild(colorRect);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}