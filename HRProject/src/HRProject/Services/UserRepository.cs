using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HRProject.Services
{
    public class UserRepository : IUserRepository
    {
        private HRContext _context;
      //  private UserManager<User> _userManager;

        public UserRepository(HRContext context)
        {
            _context = context;
        //    _userManager = userManager;
        }

        public void AddUser(User user)
        {
            _context.RegUsers.Add(user);
            _context.SaveChanges();

        }

        public User Find(string name)
        {
            return _context.RegUsers.FirstOrDefault(t => t.Name == name);
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.RegUsers.OrderBy(c => c.Name).ToList();
        }

        public User GetUser(string name)
        {
            return _context.RegUsers.Where(c => c.Name == name).FirstOrDefault();
        }

        public void Remove(string name)
        {
            var entity = _context.RegUsers.First(t => t.Name == name);
            _context.RegUsers.Remove(entity);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.RegUsers.Update(user);
            _context.SaveChanges();
        }

        public ICollection<JobPosition> ListByHr (string id, bool includeJob)
        {

            var users = _context.Users.Include(c => c.CreatedJobs)
                    .FirstOrDefault(c => c.Id == id);
            if (users != null)
            {
                // return users.SelectMany(u => u.CreatedJobs).ToList();
                return users.CreatedJobs.ToList();
            }
            return null;
        }

        //public ICollection<JobPosition> ListByRole (string name, string roleName)
        //{
        //    var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
        //    if (role != null)
        //    {

        //    }
        //}

        public bool AddRole(string name, string roleName)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role != null)
            {
                var entity = _context.RegUsers.Include(u => u.Roles).FirstOrDefault(t => t.Name == name);
                if (!entity.Roles.Any(r => r.RoleId == role.Id))
                {
                    entity.Roles.Add(new Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string> { RoleId = role.Id, UserId = entity.Id });
                    _context.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public bool RemoveRole(string name, string roleName)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role != null)
            {
                var entity = _context.RegUsers.Include(u => u.Roles).FirstOrDefault(t => t.Name == name);
                if (!entity.Roles.Any(r => r.RoleId == role.Id))
                {
                    entity.Roles.Remove(new Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string> { RoleId = role.Id, UserId = entity.Id });
                    _context.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public ICollection<User> FindRole(string roleName)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Name == roleName);
            if (role != null)
            {
                var entity = _context.RegUsers.Include(u => u.Roles).Where(u => u.Roles.Any(r => r.RoleId == role.Id)).ToList();
                return entity;
            }
            return null;
           
        }
    }
}
