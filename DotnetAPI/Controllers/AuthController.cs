using System.Data;
using System.Security.Cryptography;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;

        private readonly AuthHelper _authHelper;
        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto registrationDto)
        {
            if (registrationDto.Password == registrationDto.PasswordConfirm)
            {
                string sqlCheckUserExists = "SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '" + registrationDto.Email + "'";
                IEnumerable<string> existingUsers = _dapper.LoadData<string>(sqlCheckUserExists);
                if (existingUsers.Count() == 0)
                {
                    byte[] passwordSalt = new byte[128 / 8];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetNonZeroBytes(passwordSalt);
                    }

                    byte[] passwordHash = _authHelper.GetPasswordHash(registrationDto.Password, passwordSalt);

                    string sqlAddAuth = @"
                    INSERT INTO TutorialAppSchema.Auth ([Email],
                    [PasswordHash],
                    [PasswordSalt]) VALUES ('" + registrationDto.Email +
                    "', @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>();

                    SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary);
                    passwordSaltParameter.Value = passwordSalt;

                    SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary);
                    passwordHashParameter.Value = passwordHash;

                    sqlParameters.Add(passwordSaltParameter);
                    sqlParameters.Add(passwordHashParameter);

                    if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {

                        string sqlAddUser = @"
                        INSERT INTO TutorialAppSchema.Users 
                            ([FirstName]
                            , [LastName]
                            , [Email]
                            , [Gender]
                            , [Active]) VALUES (
                            '" +registrationDto.FirstName + 
                            "', '" + registrationDto.LastName + 
                            "', '" + registrationDto.Email + 
                            "', '" + registrationDto.Gender + 
                            "', 1)";
                        if (_dapper.ExecuteSql(sqlAddUser))
                        {
                            return Ok();
                        }
                        throw new Exception("Failed to add user.");

                    }
                    throw new Exception("Failed to register user.");
                }
                throw new Exception("User Already exist");
            }
            throw new Exception("Passwords do not match");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto loginDto)
        {
            string sqlForHashAndSalt = @"SELECT 
                [PasswordHash],
                [PasswordSalt] FROM TutorialAppSchema.Auth WHERE EMail = '" +
                loginDto.Email + "'";
            
            UserForLoginConfirmationDto userForConfirmation = _dapper.LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);
            byte[] passwordHash = _authHelper.GetPasswordHash(loginDto.Password, userForConfirmation.PasswordSalt);

            for(int index =0; index < passwordHash.Length; index++)
            {
                if(passwordHash[index] != userForConfirmation.PasswordHash[index])
                {
                    return StatusCode(401, "Incorrect Password");
                }
            }
            string userIdSql = @"SELECT 
                UserId FROM TutorialAppSchema.Users WHERE Email = '" + 
                loginDto.Email + "'";
           
            int userId = _dapper.LoadDataSingle<int>(userIdSql);
            return Ok(new Dictionary<string, string>
            {
                {"token", _authHelper.CreateToken(userId)}
            });
        }

        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string userId = User.FindFirst("userId")?.Value + "";

            string userIdSql = @"SELECT 
                UserId FROM TutorialAppSchema.Users WHERE UserId = " + 
                userId;
            int userIdFromDB = _dapper.LoadDataSingle<int>(userIdSql);
            return Ok(new Dictionary<string, string>
            {
                {"token", _authHelper.CreateToken(userIdFromDB)}
            });
        }
       
    }
}