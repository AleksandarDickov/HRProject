﻿using System;
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
    }
}