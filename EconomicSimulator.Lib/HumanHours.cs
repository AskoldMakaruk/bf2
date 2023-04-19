public readonly record struct HumanHours(decimal Value)
{
    public static implicit operator HumanHours(decimal value) => new HumanHours(value);
    public static implicit operator decimal(HumanHours value) => value.Value;
}