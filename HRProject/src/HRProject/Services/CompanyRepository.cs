using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HRProject.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private HRContext _context;

        public CompanyRepository(HRContext context)
        {
            _context = context;
        }
        public void AddCompany(Company company)
        {
            _context.Companys.Add(company);
            _context.SaveChanges();
        }

        public Company Find(int id)
        {
            return _context.Companys.FirstOrDefault(t => t.CompanyId == id);
        }

        public IEnumerable<Company> GetCompanies()
        {
            return _context.Companys.OrderBy(c => c.Name).ToList();
        }

        public Company GetCompany(int companyId)
        {
            return _context.Companys.Where(c => c.CompanyId == companyId).FirstOrDefault();
        }

        public Company GetCompany(int id, bool includeJob)
        {
            if (includeJob)
            {
                return _context.Companys.Include(c => c.Jobs)
                    .Where(c => c.CompanyId == id).FirstOrDefault();
            }
            return _context.Companys.Where(c => c.CompanyId == id).FirstOrDefault();
        }

        public void Remove(int id)
        {
            var entity = _context.Companys.First(t => t.CompanyId == id);
            _context.Companys.Remove(entity);
            _context.SaveChanges();
        }

        public void UpdateCompany(Company company)
        {
            _context.Companys.Update(company);
            _context.SaveChanges();
        }
    }
}
