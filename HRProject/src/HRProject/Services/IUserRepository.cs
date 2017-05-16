using HRProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Services
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetUser(string name);
        void AddUser(User user);
        void UpdateUser(User user);
        User Find(string name);
        void Remove(string name);
        bool AddRole(string name, string roleName);
        bool RemoveRole(string name, string roleName);
        ICollection<JobPosition> ListByHr(string id, bool includeJob);
        ICollection<User> FindRole(string roleName);
    }
}
