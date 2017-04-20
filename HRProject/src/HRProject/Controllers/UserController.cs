using HRProject.Models;
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


        [HttpPut("{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] User updateUser)
        {
            if (updateUser == null || updateUser.UserId != userId)
            {
                return BadRequest();
            }

            var newUser = _userRepository.GetUser(userId);
            if (newUser == null)
            {
                return NotFound();
            }

            newUser.Name = updateUser.Name;
            newUser.SurName = updateUser.SurName;
            newUser.City = updateUser.City;
            newUser.Country = updateUser.Country;
            newUser.PartTime_FullTime = updateUser.PartTime_FullTime;
            newUser.WorkExperience = updateUser.WorkExperience;
            newUser.Status = updateUser.Status;
            newUser.DateOfBirth = updateUser.DateOfBirth;
            newUser.Sex = updateUser.Sex;
            newUser.NoteField = updateUser.NoteField;
            newUser.Keywords = updateUser.Keywords;

            _userRepository.UpdateUser(newUser);
            return new NoContentResult();
        }
    }
}
