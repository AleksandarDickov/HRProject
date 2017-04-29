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
    [Route("[controller]")]
    public class UserController : Controller
    {
        //private HRContext _ctx;
        private IUserRepository _userRepository;
        private UserManager<User> _userManager;

        public UserController(UserRepository userRepository, UserManager<User> userManager/*, HRContext ctx*/)
        {
        //    _ctx = ctx;
            _userManager = userManager;
            _userRepository = userRepository;
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

        //[Authorize(Roles = "HrManager")]
        //[HttpPost]
        //UserManager<IdentityUser> userManager
        //public IActionResult AddUser([FromBody] User user , UserManager<IdentityUser> userManager)
        //{
        //    if (user == null)
        //    {
        //        return BadRequest();
        //    }
        //    try
        //    {
        //        var chkUser = await UserManager.CreateAsync(user);
        //        _userRepository.AddUser(user);
        //    }
        //    catch (Exception)
        //    {
        //        return BadRequest();
        //    }

        //    return Ok(" Sve je u redu");
        //}

        [Authorize(Roles = "HrManager")]
        [HttpPost]
        public async Task<IActionResult> addUser([FromBody] User user)
        { 
            
            var bb = await _userManager.CreateAsync(user);
            return Ok(bb);
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
