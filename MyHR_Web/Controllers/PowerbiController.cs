using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class PowerbiController : FilterController
    {
        // GET: PowerbiController
        public ActionResult Index()
        {
            return View();
        }
    }
}
