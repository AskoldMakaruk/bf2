﻿using EconomicSimulator.Lib.Entities;
using EconomicSimulator.Lib.Properties;
using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Interfaces;

public interface ITrading
{
    public Dictionary<ItemType, HumanHours> Prices { get; }
    public Inventory Inventory { get; }
    public HumanHours Balance { get; set; }
    public string Name { get; }

    public int GetCount(ItemType type)
    {
        var count = (Inventory as IManyItems)[type];
        return count;
    }

    public int GetCount(ItemRequirement type)
    {
        var count = Inventory.Items.Where(a => type.Matches(a.Key)).Sum(a => a.Value);
        return count;
    }

    // todo maybe base price on some percentage of worker productivity so workers would
    // be able to buy like 80% what they've produced back
    // but soloing the economy is inefficient and workers should strive to work for more 
    // paying job
    public HumanHours? GetPrice(ItemRequirement itemNeeded)
    {
        return Prices.FirstOrDefault(a => itemNeeded.Matches(a.Key)).Value;
    }

    public HumanHours? GetPrice(ItemType itemNeeded)
    {
        return Prices.FirstOrDefault(a => itemNeeded == a.Key).Value;
    }

    public decimal? GetPrice(IOItem item)
    {
        return item.GetPrice(Prices.FirstOrDefault(a => a.Key == item.Item).Value);
    }

    public bool Sell(IOItem item, HumanHours price)
    {
        if (GetPrice(item) < price || !Inventory.TryRemoveItem(item)) return false;
        Balance += price;
        return true;
    }

    public bool TryBuyFrom(ITrading another, ItemRequirements requirements)
    {
        var proposalsList = requirements.GetProposals(another.Inventory);
        return TryBuyFrom(another, proposalsList);
    }

    public bool TryBuyFrom(ITrading another, ItemRequirement requirements)
    {
        var proposalsList = requirements.GetProposals(another.Inventory).ToList();
        return TryBuyFrom(another, proposalsList);
    }

    public bool TryBuyFrom(ITrading another, IEnumerable<FulfilmentVariant> proposalsList)
    {
        foreach (var proposals in proposalsList)
        {
            var cheaper = proposals.Proposal.OrderBy(a =>
                    a.GetPrice(another.Prices.FirstOrDefault(x => x.Key == a.Item).Value))
                .FirstOrDefault();
            if (cheaper != default && another.GetPrice(cheaper) is { } price && price <= Balance && another.Sell(cheaper, price))
            {
                Balance -= price;
                Inventory.Add(cheaper);
                // Console.WriteLine($"Seller {another.Name} sold {cheaper.Item.Name}x{cheaper.Count} for {price} to {Name}");
                return true;
            }
        }

        return false;
    }
}