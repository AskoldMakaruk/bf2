using Godot;
using System;
using System.Collections.Generic;

public record Cell(int X, int Y);

public record CellRect(Cell Cell, ColorRect Rect);

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
        //redraw
        QueueRedraw();
    }

    private List<CellRect> _cells = new List<CellRect>();

    private Cell _selectedCell = new Cell(0, 0);

    public override void _Draw()
    {
        var cellRect = GetCellRect(_selectedCell);
        DrawRect(cellRect.Rect.GetRect(), Colors.Red);
    }

    private CellRect GetCellRect(Cell cell)
    {
        var index = cell.X + cell.Y * 8;
        return new CellRect(cell, (ColorRect)GetChild(index));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _Input(InputEvent @event)
    {
        // if input key is h move left
        if (@event.IsActionPressed("ui_left"))
        {
            if (_selectedCell.X > 0)
            {
                _selectedCell = new Cell(_selectedCell.X - 1, _selectedCell.Y);
            }
        }
        // if input key is j move down
        else if (@event.IsActionPressed("ui_down"))
        {
            if (_selectedCell.Y < 7)
            {
                _selectedCell = new Cell(_selectedCell.X, _selectedCell.Y + 1);
            }
        }
        // if input key is k move up
        else if (@event.IsActionPressed("ui_up"))
        {
            if (_selectedCell.Y > 0)
            {
                _selectedCell = new Cell(_selectedCell.X, _selectedCell.Y - 1);
            }
        }
        // if input key is l move right
        else if (@event.IsActionPressed("ui_right"))
        {
            if (_selectedCell.X < 7)
            {
                _selectedCell = new Cell(_selectedCell.X + 1, _selectedCell.Y);
            }
        }
    }
}