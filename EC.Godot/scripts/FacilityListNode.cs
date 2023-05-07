using EconomicSimulator.Lib.Entities;
using Godot;

namespace EC.scripts;

public partial class FacilityListNode : Control
{
	[Export] public RichTextLabel NameLabel { get; set; }
	[Export] public InventoryContainer InventoryContainer { get; set; }

	public Facility Facility { get; set; }


	public override void _Process(double delta)
	{
		if (Facility == null)
		{
			return;
		}

		NameLabel.Text = $"{Facility.Name}: {Facility.Balance.Value}HH";
		foreach (IOItem item in Facility.Inventory)
		{
			InventoryContainer.ProcessItem(item);
		}
	}

	// tool to populate the inventory
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept"))
		{
			Facility.Inventory.Add(new IOItem("stone", 1));
		}
	}
}
