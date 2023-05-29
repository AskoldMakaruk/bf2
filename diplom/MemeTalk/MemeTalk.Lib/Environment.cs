namespace MemeTalk.Lib;

public class Environment
{
    public Environment? Parent { get; }

    public Environment(Environment parent)
    {
        Parent = parent;
    }

    private readonly Dictionary<string, object?> _values = new();

    public void Define(string name, object? value)
    {
        _values[name] = value;
    }

    public object? Get(Token name)
    {
        if (_values.TryGetValue(name.Lexeme, out var value))
        {
            return value;
        }

        if (Parent != null)
        {
            return Parent.Get(name);
        }

        throw new RuntimeError(name, $"Undefined variable '{name.Lexeme}'.");
    }

    public void Assign(Token exprName, object? value)
    {
        if (_values.ContainsKey(exprName.Lexeme))
        {
            _values[exprName.Lexeme] = value;
            return;
        }

        if (Parent != null)
        {
            Parent.Assign(exprName, value);
            return;
        }

        throw new RuntimeError(exprName, $"Undefined variable '{exprName.Lexeme}'.");
    }
}