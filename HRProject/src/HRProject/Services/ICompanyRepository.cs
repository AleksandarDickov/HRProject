using HRProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Services
{
    interface ICompanyRepository
    {
        IEnumerable<Company> GetCompanies();
        Company GetCompany(int companyId);
        Company GetCompany(int id, bool includeJob);
        void AddCompany(Company company);
        void UpdateCompany(Company company);
        Company Find(int id);
        void Remove(int id);
    }
}
