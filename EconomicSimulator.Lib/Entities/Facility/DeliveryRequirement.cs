namespace EconomicSimulator.Lib.Entities;

public class DeliveryRequirement : ItemRequirements
{
    public Facility Facility { get; }

    public DeliveryRequirement(Facility facility, IEnumerable<ItemRequirement> requirements) : base(requirements)
    {
        Facility = facility;
    }
}