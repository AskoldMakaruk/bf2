using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EC.scripts;
using EconomicSimulator.Lib;
using EconomicSimulator.Lib.Entities;

public partial class FacilityList : VBoxContainer
{
    public PackedScene FacilityPackedScene { get; set; }
    public List<FacilityListNode> Nodes { get; set; } = new();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        FacilityPackedScene = GD.Load<PackedScene>("res://scenes/facility.tscn");
    }

    // add worker list node as a child of this node
    private void AddFacility(Facility facility)
    {
        var workerListNode = (FacilityListNode)FacilityPackedScene.Instantiate();
        workerListNode.Facility = facility;
        AddChild(workerListNode);
        Nodes.Add(workerListNode);
    }

    public override void _Process(double delta)
    {
        if (Game.Facilities != null)
        {
            foreach (var worker in Game.Facilities.Where(worker => Nodes.All(n => n.Facility.Id != worker.Id)))
            {
                AddFacility(worker);
            }
        }
    }

}