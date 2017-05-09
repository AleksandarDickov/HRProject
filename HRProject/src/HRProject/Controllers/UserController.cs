using HRProject.Models;
using HRProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        //    private HRContext _ctx;
        private IUserRepository _userRepository;
        private UserManager<User> _userManager;

        public UserController(IUserRepository userRepository, UserManager<User> userManager/*, HRContext ctx*/)
        {
            // _ctx = ctx;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        // [Authorize(Roles = "HrManager, SuperUser")]
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

        [HttpPut("RegToHr/{userName}")]
        public IActionResult UpdateToHrRole (string userName, [FromBody] User updateUser)
        {
            if (updateUser == null || updateUser.Name != userName)
            {
                return BadRequest();
            }

            var newUser = _userRepository.GetUser(userName);

            _userRepository.UpdateUser(newUser);

            if (User.IsInRole("SuperUser"))
            {
                var result = _userRepository.AddRole(newUser.Name, "HrManager");
                _userRepository.RemoveRole(newUser.Name, "RegularUser");
            }

            return Ok("Updated to Hr");

        }

        [HttpPut("RegToSuper/{userName}")]
        public IActionResult UpdateToSuperUserRole(string userName, [FromBody] User updateUser)
        {
            if (updateUser == null || updateUser.Name != userName)
            {
                return BadRequest();
            }

            var newUser = _userRepository.GetUser(userName);

            _userRepository.UpdateUser(newUser);

            if (User.IsInRole("SuperUser"))
            {
                var result = _userRepository.AddRole(newUser.Name, "SuperUser");
                _userRepository.RemoveRole(newUser.Name, "RegularUser");
            }

            return Ok("Updated to SuperUser");

        }

        [HttpPut("{userName}")]
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
            newUser.Email = updateUser.Email;
            //newUser.UserName = updateUser.UserName;

            _userRepository.UpdateUser(newUser);

            //if (User.IsInRole("SuperUser"))
            //{
            //    //var result1 = await _userManager.AddToRoleAsync(newUser, "HrManager");
            //    var result = _userRepository.AddRole(newUser.Name, "HrManager");
            //    _userRepository.RemoveRole(newUser.Name, "RegularUser");
            //}

            return new NoContentResult();
        }


        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if(string.IsNullOrEmpty(user.UserName))
            {
                user.UserName = Guid.NewGuid().ToString();
            }
            var userResult = await _userManager.CreateAsync(user, user.Password);
            if (userResult.Succeeded)
            {
                var result = await _userManager.AddToRoleAsync(user, "RegularUser");
                if(!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }
            return Ok(user);
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
