using EconomicSimulator.Types;

namespace EconomicSimulator;

public static class ItemTypes
{
    private static readonly IReadOnlyCollection<ItemType> _ItemTypes = new[]
    {
        new ItemType("Вода", "рідина", "water"),
        new ItemType("Відро", "може носити рідини", "bucket"),
        Fruits,
        new ItemType("Коса", "інструмент для роботи на фермі", "wheat_saw"),
        new ItemType("Пшениця", "перетворюється на хліб", "wheat"),
    };

    public static readonly ItemType Fruits = new ItemType("Фрукти", "можна їсти", "fruits");

    public static ItemType Get(string name)
    {
        return _ItemTypes.FirstOrDefault(x => string.Equals(x.TypeName, name, StringComparison.CurrentCultureIgnoreCase))!;
    }
}