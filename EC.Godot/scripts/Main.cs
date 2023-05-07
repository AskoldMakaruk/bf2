using Godot;
using System;
using EconomicSimulator.Lib;

public partial class Main : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StartStatic.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		StartStatic.Run();
		
		// on e key press, create popup window 
		if (Input.IsActionJustPressed("e"))
		{
			var popup = (Popup)GD.Load<PackedScene>("res://scenes/form_popup.tscn").Instantiate<FormPopup>();
			AddChild(popup);
		}
	}
}
