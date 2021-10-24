using Fitness.Models.Domain;
using Fitness.Models.Domain.Responses;
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
using TaskManagementApp.Core.Entities;

namespace Fitness_App.Controllers
{
    [Authorize]
    [Route("api/fitness/")]
    [ApiController]
    public class FitnessApiController : ControllerBase
    {
        private static Logger logger = LogManager.GetLogger("fitnessAppLogger");
        private readonly IConfiguration _configuration;
        private readonly AccessTokenGenerator _tokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenRepo _refreshTokenRepo;
        IFitnessServices _service = null;


        public FitnessApiController(IConfiguration configuration, IFitnessServices service, IConfiguration config, AccessTokenGenerator token, RefreshTokenGenerator refreshToken, RefreshTokenValidator refreshTokenValidator, IRefreshTokenRepo refreshTokenRepo)
        {

            _service = service;

            _configuration = config;

            _tokenGenerator = token;

            _refreshTokenGenerator = refreshToken;

            _refreshTokenValidator = refreshTokenValidator;

            _refreshTokenRepo = refreshTokenRepo;

        }


        [HttpGet("user/{id}")]
    
        public async Task<IActionResult> GetUserById([FromBody] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (await _service.GetById(id) == null)
            {
                return NotFound("User Not Found");
            }

            return Ok(await _service.GetById(id));

        }

        [HttpGet("ExerciseType")]
  
        public async Task<IActionResult> GetExerciseType()
        {
             
            return Ok(await _service.GetExerciseType());

        }

        [HttpGet("{id}")]
   
        public async Task<IActionResult> GetExercises(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (await _service.GetExercises(id) == null)
            {
                return NotFound("Exercise Not Found");
            }

            return Ok(await _service.GetExercises(id));

        }

        [HttpGet("{id}/{userDate}")]
        public async Task<IActionResult> GetExerciseByDate(int id, DateTime userDate)
        {
            if(await _service.GetExerciseByDate(id, userDate) == null)
            {
                return NotFound("Exercise not found");
            }
            return Ok(await _service.GetExerciseByDate(id, userDate));

        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserAddRequest user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (user.Password != user.ConfirmPassword)

            {
                return BadRequest(new ErrorResponse("Password does not match confirm password."));
            }

            User existingEmail = await _service.GetByEmail(user.Email);

            if (existingEmail != null)
            {
                return Conflict(new ErrorResponse("Email already exists"));
            }

            return Ok(await _service.AddUser(user));
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogin loginrequest)
        {
            if (!ModelState.IsValid)
            {
                logger.Info("Email or password is invalid.");
                return BadRequestModelState();

            }

            User user = await _service.GetByEmail(loginrequest.Email);

            if (user == null)
            {
                logger.Info("User not found");
                return Unauthorized();
                
            }

            var LoginUser = await _service.Login(loginrequest);

            if (CommonMethods.Decrypt(LoginUser.Password) != loginrequest.Password)
            {
                return Unauthorized();
            }

            string accessToken = _tokenGenerator.GenerateToken(LoginUser);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            RefreshToken refreshTokenDTO = new RefreshToken()
            {
                Token = refreshToken,
                UserId = LoginUser.UserId
            };

            await _refreshTokenRepo.Create(refreshTokenDTO);


            return Ok(new { accessToken, refreshToken, LoginUser });
        }
        [AllowAnonymous]
        [HttpPost("addExercise")]
        public async Task<IActionResult> AddExercise([FromBody]ExerciseAddRequest exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            return Ok(await _service.AddExercise(exercise));
        }

        [AllowAnonymous]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUser user)
        {
            return Ok(await _service.UpdateUser(user));
        }

        [AllowAnonymous]
        [HttpPut("UpdateExercise")]
        public async Task<IActionResult> UpdateExercise([FromBody]ExerciseUpdate exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            return Ok(await _service.UpdateExercise(exercise));
        }

        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            if (await _service.GetExerciseById(id) == null)
            {
                return NotFound("User not found");
            }

            await _service.DeleteExercise(id);

            return Ok("Exercise Deleted");
        }
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);

            if(!isValidRefreshToken)
            {
                return BadRequest(new ErrorResponse("Invalid refresh token."));
            }

            RefreshToken refreshTokenDTO = await _refreshTokenRepo.GetByToken(refreshRequest.RefreshToken);

            if(refreshTokenDTO == null)
            {
                return NotFound(new ErrorResponse("Invalid refresh token."));
            }

            await _refreshTokenRepo.Delete(refreshTokenDTO.UserId);


            User user = await _service.GetById(refreshTokenDTO.UserId);

            if(user == null)
            {
                return NotFound(new ErrorResponse("User Not Found"));
            }

            string accessToken = _tokenGenerator.GenerateToken(user);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

     
            await _refreshTokenRepo.Create(refreshTokenDTO);


            return Ok(new { accessToken, refreshToken });
        }

       
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            string rawId = HttpContext.User.FindFirstValue("id");

            if (!int.TryParse(rawId, out int userId))
            {
                return Unauthorized();
            }

            await _refreshTokenRepo.DeleteAll(userId);


            return NoContent();

        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new ErrorResponse(errorMessages));
        }


    }


    
}
