using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRProject.Models;
using HRProject.Services;

namespace HRProject.Controllers
{
    [Route("api/company")]
    public class CompanyController : Controller
    {
        private ICompanyRepository _companyRepository;
        private HRContext _ctx;

        public CompanyController(HRContext ctx)
        {
            _ctx = ctx;
            _companyRepository = new CompanyRepository(ctx); 
        }

        [HttpGet("{testDatabase}")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }

        [HttpGet()]
        public IActionResult GetCompanies()
        {
            var companyEntity = _companyRepository.GetCompanies();

            return Ok(companyEntity);
        }

        [HttpPost]
        public IActionResult AddCompany([FromBody] Company company)
        {
            if (company == null)
            {
                return BadRequest();
            }
            try
            {
                _companyRepository.AddCompany(company);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok("Sve je u redu");
        }

        [HttpPut("{companyId}")]
        public IActionResult Update(int companyId, [FromBody] Company updateCompany)
        {
            if (updateCompany == null || updateCompany.CompanyId != companyId)
            {
                return BadRequest();
            }
            //var newPerson = _ctx.People.Find(Jmbg);
            var newCompany = _companyRepository.GetCompany(companyId);
            if (newCompany == null)
            {
                return NotFound();
            }

            newCompany.Name = updateCompany.Name;
            newCompany.Phone = updateCompany.Phone;
            newCompany.Website = updateCompany.Website;
            newCompany.EmailAddress = updateCompany.EmailAddress;
            newCompany.Country = updateCompany.Country;
            newCompany.Description = updateCompany.Description;
            newCompany.City = updateCompany.City;
            newCompany.Jobs = updateCompany.Jobs;
            newCompany.CompanyId = updateCompany.CompanyId;

            _companyRepository.UpdateCompany(newCompany);
            return new NoContentResult();
        }

        [HttpDelete("{companyId}")]
        public IActionResult Delete(int companyId)
        {
            var company = _companyRepository.Find(companyId);
            if (company == null)
            {
                return NotFound();
            }

            _companyRepository.Remove(companyId);
            return new NoContentResult();
        }

        [HttpGet("{companyId}")]
        public IActionResult GetCompany(int companyId, bool includeJob = false)
       {
            var company = _companyRepository.GetCompany(companyId, includeJob);

            if (company == null)
            {
                return NotFound();
            }

            if (includeJob)
            {
                return Ok(company);
            }
            var p = _companyRepository.GetCompany(companyId);
            if (company == null)
            {
                return NotFound();
            }
            _companyRepository.GetCompany(companyId);

            return Ok(p);

        }



    }
}
