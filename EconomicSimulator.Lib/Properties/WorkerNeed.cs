using EconomicSimulator;

public class WorkerNeed
{
    public WorkerNeedType Type { get; set; }
    public Progress Progress { get; set; }

    public bool TryToSatisfy(Inventory inventory)
    {
        var result = false;

        while (IsNeeded() && Type.Requirements.CanBeSatisfied(inventory))
        {
            var proposal = Type.Requirements.GetProposals(inventory).OrderBy(a => Random.Shared.Next()).FirstOrDefault();

            foreach (var item in proposal.Proposal)
            {
                while (IsNeeded() && inventory.TryRemoveItem(item))
                {
                    Progress -= Type.ProgressPerItem;
                    result = true;
                }
            }
        }

        return result;
    }

    public bool IsNeeded() => Progress.Value >= Type.ProgressPerItem;

    public static implicit operator WorkerNeed(string typeName) =>
        new WorkerNeed()
        {
            Type = WorkerNeedTypes.Get(typeName),
            Progress = 0
        };
}