namespace EconomicSimulator.Lib.Properties;

public record Progress(decimal Value)
{
    public decimal Value { get; set; } = Value;
    public static implicit operator Progress(decimal value) => new Progress(value);
    public static implicit operator decimal(Progress value) => value.Value;
}