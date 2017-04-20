using HRProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Services
{
    interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUser(int userId);
        void AddUser(User user);
        void UpdateUser(User user);
        User Find(int id);
        void Remove(int id);
    }
}
