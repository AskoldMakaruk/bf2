using Godot;
using System;
using System.Linq;
using EconomicSimulator.Lib;

public partial class StatGraph : Godot.Line2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddPoint(new(0, 0));
        for (int i = 1; i < 10; i++)
        {
            AddPoint(new Vector2(i * 10, -i * 10 + 400));
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // when key is pressed
        if (Input.IsActionJustPressed("w"))
        {
            for (int i = 0; i < Points.Length; i++)
            {
                SetPointPosition(i, Points[i] + new Vector2(10, 10));
            }
        }
    }

    public override void _Draw()
    {
        base._Draw();
    }
}