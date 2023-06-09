using Chess.Backend.Middlewares;
using Chess.Backend.Repositories;
using Chess.Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ChessContext>(options => options
        .UseNpgsql("Server=188.166.81.64;Database=ChessTest;Username=ilya;Password=123"));
builder.Services.AddScoped<UserRepository>();
builder.Services.AddSingleton<SessionService>();
builder.Services.AddScoped<GameRepository>();
builder.Services.AddScoped<LobbyRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<LoginMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();


// Readlist
// mediator
// maping
// migrates