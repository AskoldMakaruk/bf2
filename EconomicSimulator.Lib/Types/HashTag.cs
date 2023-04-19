public readonly record struct HashTag(string Name)
{
    public static implicit operator HashTag(string name) => new(name);
}