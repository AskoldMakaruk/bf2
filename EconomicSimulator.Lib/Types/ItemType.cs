using EconomicSimulator.Lib.Entities;

namespace EconomicSimulator.Lib.Types;

public class ItemType
{
    public ItemType(string Name, string Description, string TypeName)
    {
        this.Name = Name;
        this.Description = Description;
        this.TypeName = TypeName;
    }

    public Item CreateInstance()
    {
        return new Item()
        {
            Id = new Guid(),
            Type = this
        };
    }

    public static implicit operator ItemType(string name) => ItemTypes.Get(name);

    public bool Equals(ItemType? other)
    {
        return string.Equals(this.TypeName, other?.TypeName, StringComparison.InvariantCultureIgnoreCase);
    }

    public static bool operator ==(ItemType? left, ItemType? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ItemType? left, ItemType? right)
    {
        return !(left == right);
    }

    // public override string ToString()
    // {
    //     return TypeName;
    // }

    public string Name { get; init; }
    public string Description { get; init; }
    public string TypeName { get; init; }

    public void Deconstruct(out string Name, out string Description, out string TypeName)
    {
        Name = this.Name;
        Description = this.Description;
        TypeName = this.TypeName;
    }
}