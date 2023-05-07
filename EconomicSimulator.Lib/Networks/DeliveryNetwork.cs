using EconomicSimulator.Lib.Entities;

namespace EconomicSimulator.Lib.Networks;

public class DeliveryNetwork
{
    private List<DeliveryRequirement> _deliveryRequirements = new();
    public IReadOnlyList<DeliveryRequirement> DeliveryRequirements => _deliveryRequirements.AsReadOnly();

    public List<Facility> Facilities { get; set; }
    public List<Worker> Workers { get; set; }

    public void PostDeliveryRequirement(DeliveryRequirement deliveryRequirement)
    {
        _deliveryRequirements.Add(deliveryRequirement);
    }
}