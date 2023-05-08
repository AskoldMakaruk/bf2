using EconomicSimulator.Lib.Properties;
using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Entities;

/// <summary>
/// Implementation of builder pattern for facility
/// </summary>
public class FacilityBuilder
{
    public Facility CreateDefault()
    {
        return new FacilityBuilder("соска")
            .WithName("Твоя мать")
            .WithJobTypes(new List<JobType>() { "сосать" })
            .Build();
    }

    private readonly Facility _facility;

    public FacilityBuilder(FacilityType type)
    {
        _facility = new Facility()
        {
            Id = Guid.NewGuid(),
            Type = type,
            Workers = new()
        };
    }

    public FacilityBuilder WithName(string name)
    {
        _facility.Name = name;
        return this;
    }

    public FacilityBuilder WithLocation(Location location)
    {
        _facility.Location = location;
        return this;
    }

    public FacilityBuilder WithPrices(Dictionary<ItemType, HumanHours> prices)
    {
        _facility.Prices = prices;
        return this;
    }

    public FacilityBuilder WithJobTypes(List<JobType> jobTypes)
    {
        _facility.JobTypes = jobTypes;
        return this;
    }

    public FacilityBuilder WithCountry(Country country)
    {
        _facility.CountryId = country.Id;
        return this;
    }


    public Facility Build()
    {
        if (_facility.Name == null)
        {
            throw new InvalidOperationException("Name is not set");
        }

        return new(_facility);
    }
}