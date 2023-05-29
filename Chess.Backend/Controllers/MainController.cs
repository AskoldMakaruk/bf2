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
        if (!await _userRepository.UserExists(model.Name))
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
    // todo check password and login
    // generate token
    // save token to memory (concurrent dictionary<TokenGuid, UserId)
    // todo return token
        if (await _userRepository.LoginUser(model) is not {} user)
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
        return Ok();
    }

    [HttpPost("join-game")]
    public async Task<IActionResult> JoinGameAsync([FromBody] GameCreateView model)
    {
        return Ok();
    }
    
    
    
    [HttpGet("lobby-list")]
    public async Task<LobbyListView> GetGameListAsync()
    {
        // var list = await _lobbyRepository.GetLobbyListAsync();
        // return list;
        return default;
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