namespace EconomicSimulator.Lib.Types;

public static class ItemTypes
{
    public static readonly ItemType Fruits = new("Фрукти", "можна їсти", "fruits");
    public static readonly ItemType IronOre = new("Залізна руда", "прогрес", "iron_ore");
    public static readonly ItemType Iron = new("Залізо", "залізне", "iron");
    public static readonly ItemType Stone = new("Камінь", "матеріал будівництва", "stone");

    public static readonly IReadOnlyCollection<ItemType> _ItemTypes = new[]
    {
        new ItemType("Вода", "рідина", "water"),
        new ItemType("Відро", "може носити рідини", "bucket"),
        Fruits,
        new ItemType("Коса", "інструмент для роботи на фермі", "wheat_saw"),
        new ItemType("Пшениця", "перетворюється на хліб", "wheat"),
        IronOre,
        Iron,
        Stone
    };

    public static ItemType Get(string name)
    {
        return _ItemTypes.FirstOrDefault(x => string.Equals(x.TypeName, name, StringComparison.CurrentCultureIgnoreCase))!;
    }
}