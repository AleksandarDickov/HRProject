using HRProject.Models;
using HRProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public IActionResult UpdateToHrRole(string userName, [FromBody] User updateUser)
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


        [HttpGet("{id}")]
        public IActionResult GetJobsById(string id, bool includeJob = false)
        {
            var jobs = _userRepository.ListByHr(id, includeJob);

            if (jobs == null)
            {
                return NotFound();
            }

            if (includeJob)
            {
                return Ok(jobs);
            }
            return BadRequest();
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
            if (string.IsNullOrEmpty(user.UserName))
            {
                user.UserName = Guid.NewGuid().ToString();
            }

            user.DateCreated = DateTime.Now;

            var userResult = await _userManager.CreateAsync(user, user.Password);
            if (userResult.Succeeded)
            {
                var result = await _userManager.AddToRoleAsync(user, "RegularUser");
                if (!result.Succeeded)
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
        [HttpGet("datecreated/{dateFilter}")]
        public IActionResult SortByDate(string dateFilter)
        {
            var calendar = CultureInfo.InvariantCulture.Calendar;

            if (dateFilter == "today")
            {
                return Ok(_userRepository.GetUsers().Where(u => u.DateCreated.Date == DateTime.Today.Date));
            }
            else if (dateFilter == "yesterday")
            {
                return Ok(_userRepository.GetUsers().Where(u => u.DateCreated.Date == DateTime.Today.AddDays(-1).Date));
            }
            else if (dateFilter == "currentweek")
            {
                return Ok(_userRepository.GetUsers().Where(u =>
                    calendar.GetWeekOfYear(u.DateCreated.Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) ==
                    calendar.GetWeekOfYear(DateTime.Today.Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday)));
            }
            else if (dateFilter == "previousweek")
            {
                return Ok(_userRepository.GetUsers().Where(u =>
                    calendar.GetWeekOfYear(u.DateCreated.Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) ==
                    calendar.GetWeekOfYear(DateTime.Today.Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) - 1));
            }
            else if (dateFilter == "currentmonth")
            {
                return Ok(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) ==
                    calendar.GetMonth(DateTime.Today.Date)));
            }
            else if (dateFilter == "previousmonth")
            {
                return Ok(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) ==
                    calendar.GetMonth(DateTime.Today.Date) - 1));
            }
            else if (dateFilter == "currentquarter")
            {
                var currentDate = calendar.GetMonth(DateTime.Today.Date);

                if (currentDate <= 3)
                {
                    return Ok(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) >= 1
                    &&
                    calendar.GetMonth(u.DateCreated.Date) <= 3
                    &&
                    u.DateCreated.Year == DateTime.Now.Year));
                }
                else if (currentDate >= 4 && currentDate <= 6)
                {
                    return Ok(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) >= 4
                    &&
                    calendar.GetMonth(u.DateCreated.Date) <= 6 
                    &&
                    u.DateCreated.Year == DateTime.Now.Year));
                }
                else if (currentDate >= 7 && currentDate <= 9)
                {
                    return Ok(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) >= 7
                    &&
                    calendar.GetMonth(u.DateCreated.Date) <= 9
                    &&
                    u.DateCreated.Year == DateTime.Now.Year));

                }
                else if (currentDate >= 10 && currentDate <= 12)
                {
                    return Ok(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) >= 10
                    &&
                    calendar.GetMonth(u.DateCreated.Date) <= 12
                    &&
                    u.DateCreated.Year == DateTime.Now.Year));
                }

                return BadRequest();

            }
            else if (dateFilter == "previousquarter")
            {

                var currentQuarter = calendar.GetMonth(DateTime.Today.Date);

                var January = calendar.GetMonthsInYear(DateTime.Today.Year) - 11;
                var February = calendar.GetMonthsInYear(DateTime.Today.Year) - 10;
                var Mart = calendar.GetMonthsInYear(DateTime.Today.Year) - 9;
                var April = calendar.GetMonthsInYear(DateTime.Today.Year) - 8;
                var May = calendar.GetMonthsInYear(DateTime.Today.Year) - 7;
                var Jun = calendar.GetMonthsInYear(DateTime.Today.Year) - -6;
                var Jul = calendar.GetMonthsInYear(DateTime.Today.Year) - 5;
                var August = calendar.GetMonthsInYear(DateTime.Today.Year) - 4;
                var Semptembar = calendar.GetMonthsInYear(DateTime.Today.Year) - 3;
                var October = calendar.GetMonthsInYear(DateTime.Today.Year) - 2;
                var Novembar = calendar.GetMonthsInYear(DateTime.Today.Year) - 1;
                var Decembar = calendar.GetMonthsInYear(DateTime.Today.Year) - 0;

                if (currentQuarter <= 3)
                {
                    return Ok(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) <= Decembar
                    &&
                    calendar.GetMonth(u.DateCreated.Date) >= October &&
                    u.DateCreated.Year == DateTime.Now.Year-1));
                }
                else if (currentQuarter >= 4 && currentQuarter <= 6)
                {
                    return Ok(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) >= January
                    &&
                    calendar.GetMonth(u.DateCreated.Date) <= Mart
                    &&
                    u.DateCreated.Year == DateTime.Now.Year));
                }
                else if (currentQuarter >= 7 && currentQuarter <= 9)
                {
                    return Ok(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) >= April
                    &&
                    calendar.GetMonth(u.DateCreated.Date) <= Jun
                    &&
                    u.DateCreated.Year == DateTime.Now.Year));
                }
                else if (currentQuarter >= 10 && currentQuarter <= 12)
                {
                    return Ok(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) >= Jul
                    &&
                    calendar.GetMonth(u.DateCreated.Date) <= Semptembar
                    &&
                    u.DateCreated.Year == DateTime.Now.Year));
                }

                return BadRequest();

            }
            else if (dateFilter == "currentyear")
            {
                return Ok(_userRepository.GetUsers().Where(u =>
                calendar.GetYear(u.DateCreated.Date) ==
                calendar.GetYear(DateTime.Today.Date)));
            }
            else if (dateFilter == "previousyear")
            {
                return Ok(_userRepository.GetUsers().Where(u =>
                calendar.GetYear(u.DateCreated.Date) ==
                calendar.GetYear(DateTime.Today.Date) - 1));
            }

            else
            {
                return BadRequest();
            }

        }
    }
}
