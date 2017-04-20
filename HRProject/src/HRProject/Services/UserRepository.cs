using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HRProject.Services
{
    public class UserRepository : IUserRepository
    {
        private HRContext _context;

        public UserRepository(HRContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

        }

        public User Find(int id)
        {
            return _context.Users.FirstOrDefault(t => t.UserId == id);
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.OrderBy(c => c.Name).ToList();
        }

        public User GetUser(int userId)
        {
            return _context.Users.Where(c => c.UserId == userId).FirstOrDefault();
        }

        public void Remove(int id)
        {
            var entity = _context.Users.First(t => t.UserId == id);
            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
