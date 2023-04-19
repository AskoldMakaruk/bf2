using System.Collections.Concurrent;
using BlazorApp1.Shared;
using EconomicSimulator;
using EconomicSimulator.Lib;
using Grpc.Core;
using StatFrame = BlazorApp1.Shared.StatFrame;

namespace BlazorApp1.Server.Controllers;

public class ProductionStatsService : ProductionStats.ProductionStatsBase
{
    // public override Task<Empty> Post(PostRequest request, ServerCallContext context)
    // {
    //     ProductionStats.Post(request.Name, request.Count);
    //
    //     _statQueue.Enqueue(new StatFrame { Name = request.Name, Count = request.Count });
    //
    //     return Task.FromResult(new Empty());
    // }
    //
    // public override async Task<Empty> PostFrames(IAsyncStreamReader<PostFramesRequest> requestStream, ServerCallContext context)
    // {
    //     await foreach (var request in requestStream.ReadAllAsync())
    //     {
    //         foreach (var frame in request.Frame)
    //         {
    //             ProductionStats.Post(frame.Name, frame.Count);
    //
    //             _statQueue.Enqueue(frame);
    //         }
    //     }
    //
    //     await ProductionStats.PostFrames();
    //
    //     return new Empty();
    // }

    public override async Task Subscribe(SubscribeRequest request, IServerStreamWriter<StatFrame> responseStream, ServerCallContext context)
    {
        // var seenStats = new HashSet<string>();

        while (!context.CancellationToken.IsCancellationRequested)
        {
            while (GameStats.Stats.TryDequeue(out var frame))
            {
                // if (!seenStats.Contains(frame.Name))
                {
                    await responseStream.WriteAsync(new StatFrame() { Count = frame.Count, Name = frame.Name });
                    // seenStats.Add(frame.Name);
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
        }
    }
}