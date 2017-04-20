using HRProject.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private HRContext _ctx;
        private IUserRepository _userRepository;

        public UserController(HRContext ctx)
        {
            _ctx = ctx;
            _userRepository = new UserRepository(ctx);
        }


        [HttpGet()]
        public IActionResult GetUsers()
        {
            var userEntity = _userRepository.GetUsers();

            return Ok(userEntity);
        }

        [HttpGet("{userId}")]
        public IActionResult GetUser(int userId)
        {
            var user = _userRepository.GetUser(userId);


            if (user == null)
            {
                return NotFound();
            }

            _userRepository.GetUser(userId);

            return Ok(user);    

        }


    }
}
