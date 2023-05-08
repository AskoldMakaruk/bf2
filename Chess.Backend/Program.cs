using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<Database>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var database = app.Services.GetService<Database>();
var games = database.Games.ToList();
games[0].WhitePlayerId = 20;
games[0].WhitePlayerName = "лох";
database.SaveChanges();

app.Run();


public class GameEntity
{
    public int Id { get; set; }
    public int WhitePlayerId { get; set; }
    public string WhitePlayerName { get; set; } = "";
}

public class Database : DbContext
{
    public DbSet<GameEntity> Games { get; set; }
    public List<object> Data { get; set; } = new();

    public IEnumerable<object> Get()
    {
        return Data;
    }
}