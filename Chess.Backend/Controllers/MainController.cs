using Chess.Backend.Models;
using Chess.Backend.Repositories;
using Chess.Backend.Services;
using Chess.Backend.Views;
using Microsoft.AspNetCore.Mvc;


namespace Chess.Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class MainController : ControllerBase
{
    private readonly ChessContext _chessContext;
    private readonly UserRepository _userRepository;
    private readonly GameRepository _gameRepository;
    private readonly LobbyRepository _lobbyRepository;
    private readonly SessionService _sessionService;

    public MainController(
        ChessContext chessContext,
        UserRepository userRepository,
        GameRepository gameRepository,
        LobbyRepository lobbyRepository,
        SessionService sessionService)
    {
        _chessContext = chessContext;
        _userRepository = userRepository;
        _gameRepository = gameRepository;
        _lobbyRepository = lobbyRepository;
        _sessionService = sessionService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] UserCreateView model)
    {
        if (await _userRepository.UserExists(model.Name))
        {
            return BadRequest("USER-ERROR");
        }

        var user = new User()
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            PasswordHash = model.PasswordHash
        };

        await _userRepository.CreateUser(user);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserCreateView model)
    {
        /*
        todo check password and login
        generate token
        save token to memory (concurrent dictionary<TokenGuid, UserId)
        todo return token
        */
        if (await _userRepository.LoginUser(model) is not { } user)
        {
            return BadRequest("USER-ERROR");
        }

        var token = _sessionService.CreateSession(user.Id);

        return Ok(token);
    }

    //todo middleware
    // check HTTP header for token
    // if token exists, check if token is present in memory
    // if token is present in memory, add user id to request context (HttpContext.User add claim User id = id from memory) 


    [HttpPost("create-lobby")]
    public async Task<IActionResult> CreateLobbyAsync([FromBody] LobbyCreateView model)
    {
        //  get user
        var user = await GetUser();

        //  check if user can create lobby   
        var isInLobby = await _lobbyRepository.IsUserInLobbyAsync(user.Id);
        if (isInLobby)
        {
            return BadRequest("User is already in lobby");
        }

        var lobby = await _lobbyRepository.CreateLobbyAsync(model, user);

        //  add user to lobby
        //  return lobby id
        return Ok(lobby.Id);
    }

    async Task<User> GetUser()
    {
        var userId = (Guid?)HttpContext.Items["user-id"] ?? throw new Exception("User id not found");
        var user = await _userRepository.GetUser(userId) ?? throw new Exception("User id not found in Db");
        return user;
    }

    [HttpPost("join-lobby")]
    public async Task<IActionResult> JoinLobbyAsync([FromBody] LobbyJoinView model)
    {
        //check if lobby not contains 2 users
        var isLobbyFull = await _lobbyRepository.IsLobbyFilledAsync(model.Id);
        if (isLobbyFull)
            return BadRequest("Lobby is full");
        var lobby = await _lobbyRepository.GetLobbyAsync(model.Id) ?? throw new Exception("Lobby not found");

        var user = await GetUser();
        lobby.Opponent = user;

        var game = await _gameRepository.StartGameAsync(lobby);
        // todo start game
        // todo return game id and color
        // ?????
        return Ok(new {
            GameId = game.Id,
            Color = game.PlayerWhite.Id == user.Id ? "white" : "black"
        });
    }

// dotnet ef migrations add InitialCreate 
// dotnet ef database update 
// dotnet tool install --global dotnet-ef
    [HttpGet("lobby-list")]
    public async Task<LobbyListView> GetGameListAsync()
    {
        // var list = await _lobbyRepository.GetLobbyListAsync();
        return null;
    }

    [HttpGet("game-status")]
    public async Task<IActionResult> GetGameStatusAsync()
    {
        return Ok();
    }
}

//todo
// Post Register Account
// Post Login Account проверить на парольхешжопа
// Post Create Lobby
// Post Join Game
// Post Move
// Get Game List
// Get Game Status