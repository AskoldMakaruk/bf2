namespace EconomicSimulator.Types;

public readonly record struct ItemType(string Name, string Description, string TypeName)
{
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
}