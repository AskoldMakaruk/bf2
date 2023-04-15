using Telegram.Bot.Types;

public interface IEndpointAttribute
{
    public bool IsSuitable(Update update);
}

public class ButtonAttribute : Attribute, IEndpointAttribute
{
    public string Text { get; }

    public ButtonAttribute(string text)
    {
        Text = text;
    }

    public bool IsSuitable(Update update)
    {
        return update.Message?.Text == Text;
    }
}

public class CommandAttribute : Attribute, IEndpointAttribute
{
    public string Text { get; }

    public CommandAttribute(string text)
    {
        Text = text;
    }

    public bool IsSuitable(Update update)
    {
        return update.Message?.Text?.StartsWith(Text) ?? false;
    }
}