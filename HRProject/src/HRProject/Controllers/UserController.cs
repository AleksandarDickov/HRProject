using HRProject.Models;
using HRProject.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "HrManager, SuperUser")]
        [HttpGet()]
        public IActionResult GetUsers()
        {
            var userEntity = _userRepository.GetUsers();

            return Ok(userEntity);
        }

       
        [HttpGet("{userId}")]
        public IActionResult GetUser(string name)
        {
            var user = _userRepository.GetUser(name);


            if (user == null)
            {
                return NotFound();
            }

            _userRepository.GetUser(name);

            return Ok(user);    
        }



        [HttpPut("{Name}")]
        public IActionResult UpdateUser(string userName, [FromBody] User updateUser)
        {
            if (updateUser == null || updateUser.Name != userName)
            {
                return BadRequest();
            }

            var newUser = _userRepository.GetUser(userName);
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
            newUser.PasswordHash = updateUser.PasswordHash; 
           

            _userRepository.UpdateUser(newUser);
            return new NoContentResult();
        }

        [Authorize(Roles = "HrManager")]
        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            try
            {
                _userRepository.AddUser(user);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(" Sve je u redu");
        }

        [HttpDelete("userId")]
        public IActionResult Delete(string name)
        {
            var user = _userRepository.Find(name);
            if (user == null)
            {
                return NotFound();
            }
            _userRepository.Remove(name);
            return new NoContentResult();
        }
        }
}
