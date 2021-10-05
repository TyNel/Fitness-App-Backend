using Fitness.Models.Domain;
using Fitness.Models.Requests;
using Fitness.Services.Interfaces;
using Fitness.Services.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;



namespace Fitness_App.Controllers
{
    [Authorize]
    [Route("api/fitness/")]
    [ApiController]
    public class FitnessApiController : ControllerBase
    {
        private static Logger logger = LogManager.GetLogger("*");

        private readonly IConfiguration _configuration;
        IFitnessServices _service = null;

        public FitnessApiController(IConfiguration configuration, IFitnessServices service, IConfiguration config)
        {

            _service = service;

            _configuration = config;

        }


        [HttpGet("user/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(int id)
        {

            return Ok(await _service.GetById(id));

        }

        [HttpGet("ExerciseType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExerciseType()
        {

            return Ok(await _service.GetExerciseType());

        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExercises(int id)
        {

            return Ok(await _service.GetExercises(id));

        }

        [HttpGet("{id}/{userDate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExerciseByDate(int id, DateTime userDate)
        {

            return Ok(await _service.GetExerciseByDate(id, userDate));

        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserAddRequest user)
        {
            return Ok(await _service.AddUser(user));
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin user)
        {
            var LoginUser = await _service.Login(user);

            if (LoginUser == null)
            {
                return Unauthorized();
            }

            if (CommonMethods.Decrypt(LoginUser.Password) != user.Password)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);


            return Ok(new { token = tokenHandler.WriteToken(token), LoginUser });
        }
        [AllowAnonymous]
        [HttpPost("addExercise")]
        public async Task<IActionResult> AddExercise(ExerciseAddRequest exercise)
        {
            return Ok(await _service.AddExercise(exercise));
        }

        [AllowAnonymous]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUser user)
        {
            return Ok(await _service.UpdateUser(user));
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {

            await _service.DeleteExercise(id);
          
            return Ok("Exercise Deleted");
        }
    }
    
}
