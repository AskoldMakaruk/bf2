using Godot;
using System;
using EconomicSimulator.Lib.Entities;
using EconomicSimulator.Lib.Types;
using Godot.Collections;

public partial class InventoryContainer : VBoxContainer
{
    private System.Collections.Generic.Dictionary<ItemType, InventoryItem> _items = new();
    private PackedScene _itemScene;

    public override void _Ready()
    {
        _itemScene = ResourceLoader.Load<PackedScene>("res://scenes/inventory_item.tscn");
    }

    public void ProcessItem(IOItem item)
    {
        if (_items.TryGetValue(item.Item, out var itemNode))
        {
            itemNode.Item = item;
        }
        else
        {
            var node = _itemScene.Instantiate<InventoryItem>();
            node.Item = item;
            node._Ready();
            AddChild(node);
            _items.Add(item.Item, node);
        }
    }
}