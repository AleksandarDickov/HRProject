using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRProject.Controllers
{

    public class UserController : Controller
    {
        private HRContext _ctx;
        public UserController(HRContext ctx)
        {
            _ctx = ctx;
        }
    }
}
