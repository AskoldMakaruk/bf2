using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Networks;

public static class JobNetwork
{
    private static IEnumerable<JobPost> Jobs => Game.Facilities.SelectMany(a => a.GetJobPosts());

    public static IEnumerable<JobPost> GetJobPost(ItemRequirements requirements)
    {
        return Jobs.Where(a => a.Slots > 0);
        //.Where(a => requirements.CanBeSatisfied(new ManyItems(a.Type.Outputs)));
    }
}