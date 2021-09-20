using Fitness.Models.Requests;
using Fitness.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fitness_App.Controllers
{
    [Route("api/fitness")]
    [ApiController]
    public class FitnessApiController : ControllerBase
    {

        IFitnessServices _service = null;

        public FitnessApiController(IConfiguration configuration, IFitnessServices service)
        {
            int sCode = 200;

            _service = service;

           

        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(int id)
        {
            
            return Ok(await _service.GetById(id));

        }
        [HttpPost]
        public async Task<IActionResult> AddUser(UserAddRequest user)
        {
            return Ok(await _service.AddUser(user));
        }
    }
}
