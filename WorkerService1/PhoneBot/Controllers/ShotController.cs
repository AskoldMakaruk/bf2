using System.Net;
using BF2;
using RtspClientSharp;
using RtspClientSharp.RawFrames.Audio;
using RtspClientSharp.RawFrames.Video;
using Telegram.Bot.Types;

namespace WorkerService1.PhoneBot;

public class ShotController : BotController
{
    public const string IP = "192.168.3.3:8080";

    [Command("/shot")]
    public async Task<ImageContent> Shot()
    {
        Console.WriteLine($"{DateTime.Now:T} User {Update.Message.From.Username} made a picture.");
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "http://192.168.3.3:8080/photo.jpg");
        request.Headers.Add("Authorization", "Digest username=\"admin\", realm=\"IP Webcam\", nonce=\"1681471444\", uri=\"/photo.jpg\", response=\"9ee7d91be11de53f0efb5e166a07d51d\", qop=auth, nc=000000b1, cnonce=\"f2263e8b95b224ed\"");
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var shot = await response.Content.ReadAsStreamAsync();
        // var token = new CancellationToken();
        // var serverUri = new Uri($"rtsp://{IP}/h264_ulaw.sdp");
        // var credentials = new NetworkCredential("admin", "123456");
        // var connectionParameters = new ConnectionParameters(serverUri, credentials)
        // {
        //     RtpTransport = RtpTransportProtocol.TCP
        // };
        // var oneTimeReceiveTask = new TaskCompletionSource<MemoryStream>();
        // using var rtspClient = new RtspClient(connectionParameters);
        // rtspClient.FrameReceived += (sender, frame) =>
        // {
        //     if (oneTimeReceiveTask.Task.IsCompleted)
        //     {
        //         return;
        //     }
        //
        //     if (frame is not RawJpegFrame)
        //         return;
        //
        //     var frameSegment = frame.FrameSegment;
        //     var stream = new MemoryStream();
        //     stream.Write(frameSegment.Array, frameSegment.Offset, frameSegment.Count);
        //     oneTimeReceiveTask.TrySetResult(stream);
        // };
        // Console.WriteLine("Connecting...");
        // await rtspClient.ConnectAsync(token);
        // Console.WriteLine("Receiving...");
        // await rtspClient.ReceiveAsync(token);
        //
        // var shot = await oneTimeReceiveTask.Task;


        return new ImageContent(shot);
    }
}