using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Controllers
{

    public class SuperUserController : Controller
    {
        private HRContext _ctx;
        public SuperUserController(HRContext ctx)
        {
            _ctx = ctx;
        }
    }
}
