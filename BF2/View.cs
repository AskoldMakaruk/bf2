using Telegram.Bot;
using Telegram.Bot.Types;

namespace BF2;

public abstract class View
{
    public abstract string ToView();
    public static implicit operator View(string text) => new StringView() { Text = text };
}

public class StringView : View
{
    public string Text;

    public override string ToView()
    {
        return Text;
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