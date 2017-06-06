using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRProject;
using HRProject.Models;
using System.Globalization;
using HRProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HRProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly HRContext _context;
        private IUserRepository _userRepository;
        private UserManager<User> _userManager;

        public UsersController(HRContext context, IUserRepository userRepository, UserManager<User> userManager)
        {
            _context = context;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        // GET: Users 
        public async Task<IActionResult> Index([FromQuery]string dateFilter, [FromQuery]string keyWord, [FromQuery]DateTime startDate , [FromQuery]DateTime endDate, [FromQuery]string userType, [FromQuery] int statusOfUser)
        {
            var calendar = CultureInfo.InvariantCulture.Calendar;

            if (dateFilter == "today")
            {
                return View(_userRepository.GetUsers().Where(u => u.DateCreated.Date == DateTime.Today.Date).ToList());
            }
            else if (dateFilter == "yesterday")
            {
                return View(_userRepository.GetUsers().Where(u => u.DateCreated.Date == DateTime.Today.AddDays(-1).Date));
            }
            else if (dateFilter == "currentweek")
            {
                return View(_userRepository.GetUsers().Where(u =>
                    calendar.GetWeekOfYear(u.DateCreated.Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) ==
                    calendar.GetWeekOfYear(DateTime.Today.Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday)));
            }
            else if (dateFilter == "previousweek")
            {
                return View(_userRepository.GetUsers().Where(u =>
                    calendar.GetWeekOfYear(u.DateCreated.Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) ==
                    calendar.GetWeekOfYear(DateTime.Today.Date, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday) - 1));
            }
            else if (dateFilter == "currentmonth")
            {
                return View(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) ==
                    calendar.GetMonth(DateTime.Today.Date)));
            }
            else if (dateFilter == "previousmonth")
            {
                return View(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) ==
                    calendar.GetMonth(DateTime.Today.Date) - 1));
            }
            else if (dateFilter == "currentquarter")
            {
                var currentDate = calendar.GetMonth(DateTime.Today.Date);

                if (currentDate <= 3)
                {
                    return View(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) >= 1
                    &&
                    calendar.GetMonth(u.DateCreated.Date) <= 3
                    &&
                    u.DateCreated.Year == DateTime.Now.Year));
                }
                else if (currentDate >= 4 && currentDate <= 6)
                {
                    return View(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) >= 4
                    &&
                    calendar.GetMonth(u.DateCreated.Date) <= 6
                    &&
                    u.DateCreated.Year == DateTime.Now.Year));
                }
                else if (currentDate >= 7 && currentDate <= 9)
                {
                    return View(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) >= 7
                    &&
                    calendar.GetMonth(u.DateCreated.Date) <= 9
                    &&
                    u.DateCreated.Year == DateTime.Now.Year));

                }
                else if (currentDate >= 10 && currentDate <= 12)
                {
                    return View(_userRepository.GetUsers().Where(u =>
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
                    return View(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) <= Decembar
                    &&
                    calendar.GetMonth(u.DateCreated.Date) >= October &&
                    u.DateCreated.Year == DateTime.Now.Year - 1));
                }
                else if (currentQuarter >= 4 && currentQuarter <= 6)
                {
                    return View(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) >= January
                    &&
                    calendar.GetMonth(u.DateCreated.Date) <= Mart
                    &&
                    u.DateCreated.Year == DateTime.Now.Year));
                }
                else if (currentQuarter >= 7 && currentQuarter <= 9)
                {
                    return View(_userRepository.GetUsers().Where(u =>
                    calendar.GetMonth(u.DateCreated.Date) >= April
                    &&
                    calendar.GetMonth(u.DateCreated.Date) <= Jun
                    &&
                    u.DateCreated.Year == DateTime.Now.Year));
                }
                else if (currentQuarter >= 10 && currentQuarter <= 12)
                {
                    return View(_userRepository.GetUsers().Where(u =>
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
                return View(_userRepository.GetUsers().Where(u =>
                calendar.GetYear(u.DateCreated.Date) ==
                calendar.GetYear(DateTime.Today.Date)));
            }
            else if (dateFilter == "previousyear")
            {
                return View(_userRepository.GetUsers().Where(u =>
                calendar.GetYear(u.DateCreated.Date) ==
                calendar.GetYear(DateTime.Today.Date) - 1));
            }

            if (keyWord != null)
            {
                var users = _context.Users.Where(u => u.UserName.Contains(keyWord)
                || u.Name.Contains(keyWord) || u.SurName.Contains(keyWord)
                || u.City.Contains(keyWord) || u.Country.Contains(keyWord)
                || u.PartTime_FullTime.Contains(keyWord) || u.Sex.Contains(keyWord)
                || u.PhoneNumber.Contains(keyWord));

                return View(users);
            }

            //DateTime d1 = DateTime.ParseExact(startDate, "MM/dd/yyyy", null);
            //DateTime d2 = DateTime.ParseExact(endDate, "MM/dd/yyyy", null);
            var d1 = startDate;
            var d2 = endDate;

            if (startDate.Year > 1 && (d2 - d1).TotalDays <= 366)
            {
                return View(_userRepository.GetUsers().Where(u =>
                u.DateCreated >= d1 && u.DateCreated <= d2));
            }

            if (userType == "HrManager")
            {
                var res = _context.Users.ToList().Where(u => IsInRole(u, "HrManager"));
                return View(res);
            }
            if (userType == "RegularUser")
            {
                
                return View(_context.Users.ToList().Where(u=> IsInRole(u, "RegularUser")));
            }
            if (userType == "SuperUser")
            {
                return View(_context.Users.ToList().Where(u => IsInRole(u, "S`uperUser")));
            }

            if (statusOfUser == 0)
            {
                return View(_context.Users.Where(u => u.StatusOfUser == Status.available));
            }
            if (statusOfUser == 1)
            {
                return View(_context.Users.Where(u => u.StatusOfUser == Status.assigned));
            }

            if (statusOfUser == 2)
            {
                return View(_context.Users.Where(u => u.StatusOfUser == Status.frozen));
            }


            else
            {
                return View(await _context.RegUsers.ToListAsync());
            }
        }

        public bool IsInRole(User user, string role)
        {
            var result = _userManager.IsInRoleAsync(user, role).Result;
            return result;
        }

        private string GetRoleName(string roleId)
        {
            return _context.Roles.FirstOrDefault(r => r.Id == roleId).Name;
        }




        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.RegUsers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,SurName,City,Country,PartTime_FullTime,WorkExperience,StatusOfUser,DateOfBirth,Sex,NoteField,DateCreated,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                user.DateCreated = DateTime.Now;
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.RegUsers.SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,SurName,City,Country,PartTime_FullTime,WorkExperience,StatusOfUser,DateOfBirth,Sex,NoteField,DateCreated,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.RegUsers
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.RegUsers.SingleOrDefaultAsync(m => m.Id == id);
            _context.RegUsers.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool UserExists(string id)
        {
            return _context.RegUsers.Any(e => e.Id == id);
        }
    }
}
