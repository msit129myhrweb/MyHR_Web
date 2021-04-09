using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class InterviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
