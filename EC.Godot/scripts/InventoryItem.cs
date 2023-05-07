using Godot;
using System;
using EconomicSimulator.Lib.Entities;

public partial class InventoryItem : Control
{
    private IOItem _item;

    public IOItem Item
    {
        get => _item;
        set
        {
            if (Label != null)
            {
                Label.Text = value.ToString();
            }

            _item = value;
        }
    }

    [Export(PropertyHint.NodePathValidTypes, "Label")]
    public NodePath LabelPath { get; set; }

    public Label Label { get; set; }


    public override void _Ready()
    {
        Label = GetNode<Label>(LabelPath);
        Label.Text = Item.ToString();
    }

    public override void _Process(double delta)
    {
    }
}