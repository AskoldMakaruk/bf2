using System.Collections.Generic;
using System.Linq;
using EconomicSimulator.Lib;
using EconomicSimulator.Lib.Entities;
using Godot;

namespace EC.scripts;

public partial class WorkerList : VBoxContainer
{
    public PackedScene WorkerPackedScene { get; set; }
    public List<WorkerListNode> Nodes { get; set; } = new();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        WorkerPackedScene = GD.Load<PackedScene>("res://scenes/worker.tscn");
    }

    // add worker list node as a child of this node
    private void AddWorker(Worker worker)
    {
        var workerListNode = (WorkerListNode)WorkerPackedScene.Instantiate();
        workerListNode.Worker = worker;
        AddChild(workerListNode);
        Nodes.Add(workerListNode);
    }

    public override void _Process(double delta)
    {
        if (Game.Workers != null)
        {
            foreach (var worker in Game.Workers.Where(worker => Nodes.All(n => n.Worker.Id != worker.Id)))
            {
                AddWorker(worker);
            }
        }
    }
}