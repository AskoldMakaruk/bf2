using System.Numerics;

public readonly record struct HumanHours(decimal Value) : IAdditionOperators<HumanHours, HumanHours, HumanHours>
{
    public static implicit operator HumanHours(decimal value) => new HumanHours(value);
    public static implicit operator decimal(HumanHours value) => value.Value;

    public static HumanHours operator +(HumanHours left, HumanHours right)
    {
        return (decimal)left + (decimal)right;
    }
}