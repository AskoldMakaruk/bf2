using EconomicSimulator;

public class ItemRequirements
{
    public ItemRequirements(params ItemRequirement[] requirements)
    {
        _requirements = requirements.ToList();
    }

    public ItemRequirements(IEnumerable<ItemRequirement> requirements)
    {
        _requirements = requirements.ToList();
    }

    public static implicit operator ItemRequirements(List<ItemRequirement> list) => new ItemRequirements(list);

    private List<ItemRequirement> _requirements;
    public IReadOnlyCollection<ItemRequirement> Requirements => _requirements;

    // public void RemovedSatisfiedBy(Inventory inventory)
    // {
    //     _requirements.RemoveAll(a => a.GetProposals(inventory));
    // }

    public IEnumerable<ItemRequirement> GetSatisfiedBy(IManyItems inventory)
    {
        return _requirements.Where(a => a.GetProposals(inventory).Any(x => x.Proposal.Any()));
    }

    public IEnumerable<FulfilmentVariant> GetProposals(IManyItems inventory)
    {
        return _requirements.SelectMany(a => a.GetProposals(inventory));
    }

    public bool CanBeSatisfied(IManyItems inventory)
    {
        return GetSatisfiedBy(inventory).Any();
    }

    public bool NotEmpty()
    {
        return _requirements.Any();
    }
}