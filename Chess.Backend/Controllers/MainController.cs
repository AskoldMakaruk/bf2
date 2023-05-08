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
    private readonly ChestContext _chestContext;
    private readonly UserRepository _userRepository;

    public MainController(ChestContext chestContext, UserRepository userRepository)
    {
        _chestContext = chestContext;
        _userRepository = userRepository;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] UserCreateView model)
    {
        if (!await  _userRepository.UserExists(model.Name))
        {
            return BadRequest("USER-ERROR");
        }
        
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Name = model.Name,
            PasswordHash = model.PasswordHash
        };
        
        await  _userRepository.CreateUser(user);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserCreateView model)
    {
        return Ok();
    }
    
    [HttpPost("create-game")]
    public async Task<IActionResult> CreateGameAsync([FromBody] GameCreateView model)
    {
        
    }

//todo
    // Post Register Account
    // Post Login Account проверить на парольхешжопа
    // Post Create Game
    // Post Join Game
    // Post Move
    // Get Game List
    // Get Game Status

