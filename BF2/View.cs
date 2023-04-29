using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace BF2;

public class ViewFactory
{
    public ViewFactory()
    {
        
    }
}
public abstract partial class View
{
    [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
    public abstract string Format { get; }

    public string ToView()
    {
        var type = GetType();
        var view = FormatRegex().Replace(Format, m => "(?'" + type.GetProperty(m.Groups[1].Value)?.GetValue(this) + "'.+)");
        return view;
    }

    public View()
    {
    }

    public View(string input)
    {
        var formatWithPlaceholders = FormatRegex().Replace(Format, m => "(?'" + m.Groups[1].Value + "'.+)");

        var groups = Regex.Match(input, formatWithPlaceholders).Groups;
        var type = GetType();
        foreach (Group group in groups)
        {
            var propertyName = group.Name;
            var property = type.GetProperty(propertyName);

            if (property == null)
            {
                continue;
            }

            var value = group.Value;

            // todo collection of views
            if (property.PropertyType == typeof(DateTime))
            {
                property.SetValue(this, DateTime.Parse(value));
                continue;
            }

            if (property.PropertyType == typeof(int))
            {
                property.SetValue(this, int.Parse(value));
                continue;
            }

            if (property.PropertyType == typeof(string))
            {
                property.SetValue(this, value);
                continue;
            }
        }
    }

    [GeneratedRegex("\\{(\\w+)\\}")]
    private static partial Regex FormatRegex();
}

public class StringView : View
{
    public string Text { get; set; }

    public override string Format => "{Text}";


    public StringView(string input) : base(input)
    {
    }

    public StringView()
    {
    }


    public static implicit operator StringView(string text) => new() { Text = text };
}

public abstract class Content
{
    public abstract Task SendAsync(ITelegramBotClient botClient, long chatId);
}

public class ImageContent : Content, IDisposable
{
    public void Dispose()
    {
        _stream.Dispose();
    }

    private readonly Stream _stream;

    public ImageContent(Stream stream)
    {
        _stream = stream;
    }

    public override async Task SendAsync(ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendPhotoAsync(chatId, new InputMedia(_stream, "screenshot.png"));
    }
}