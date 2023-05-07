using EconomicSimulator.Lib.Entities;
using Godot;

namespace EC.scripts;

public partial class WorkerListNode : Control
{
    [Export]
    public RichTextLabel NameLabel { get; set; }
    [Export]
    public InventoryContainer InventoryContainer { get; set; }

    public Worker Worker { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        // NameLabel = GetNode<RichTextLabel>("Body/NameLabel");
        // InventoryContainer = GetNode<InventoryContainer>("Body/Inventory");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        NameLabel.Text = $"{Worker.Name}: {Worker.Balance.Value}HH";
        foreach (IOItem item in Worker.Inventory)
        {
            InventoryContainer.ProcessItem(item);
        }
    }
}