using DotnetAPI.Models;
using DotnetAPI.Dtos;
using DotnetAPI.Data;

using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{

    IUserRepository _userRepository;
    IMapper _mapper;
    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserToAddDto, User>();
        }, NullLoggerFactory.Instance));
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _userRepository.GetUsers();
        return users;
    }

    [HttpGet("GetUsers/{userId}")]
    public User GetSingleUser(int userId)
    {   
       return _userRepository.GetSingleUser(userId);
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        User? userDB = _userRepository.GetSingleUser(user.UserId);
        if(userDB != null)
        {
            userDB.FirstName = user.FirstName;
            userDB.LastName = user.LastName;
            userDB.Email = user.Email;
            userDB.Gender = user.Gender;
            userDB.Active = user.Active;
            if(_userRepository.SaveChanges())
            {
                return Ok();
            }            
        }
        throw new Exception("Failed to update user");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        User userDb = _mapper.Map<User>(user);

        _userRepository.AddEntity<User>(userDb);
        if(_userRepository.SaveChanges())
        {
            return Ok();
        }            
        throw new Exception("Failed to create user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _userRepository.GetSingleUser(userId);
       
        if(userDb != null)
        {
            _userRepository.RemoveEntity<User>(userDb);
            if(_userRepository.SaveChanges())
            {
                return Ok();
            }   
            throw new Exception("Failed to delete user");
        }
        throw new Exception("Failed to find user");
    }
}





