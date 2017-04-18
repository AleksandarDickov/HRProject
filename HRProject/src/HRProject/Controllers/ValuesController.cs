using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRProject.Models;

namespace HRProject.Controllers
{
    [Route("api")]
    public class ValuesController : Controller
    {
        private HRContext _ctx;
        public ValuesController(HRContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("{testDatabase}")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }

        [HttpGet()]
        public IActionResult GetCompanys()
        {
            var companyEntity = _ctx.Companys.OrderBy(c => c.Name).ToList();

            return Ok(companyEntity);
        }

        [HttpPost]
        public IActionResult AddPerson([FromBody] Company company)
        {
            if (company == null)
            {
                return BadRequest();
            }
            try
            {
                _ctx.Companys.Add(company);
                _ctx.SaveChanges();
            }
            catch (Exception)
            {

                return BadRequest();
            }

            return Ok("Sve je u redu");
        }

        [HttpDelete("{companyId}")]
        public IActionResult Delete(int companyId)
        {
            var company = _ctx.Companys.FirstOrDefault(t => t.CompanyId == companyId);
            if (company == null)
            {
                return NotFound();
            }

            _ctx.Companys.Remove(company);
            _ctx.SaveChanges();
            return new NoContentResult();
        }



    }
}
