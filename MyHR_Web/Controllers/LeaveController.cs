using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyHR_Web.Controllers
{
    public class LeaveController : Controller
    {
        public IActionResult LeaveCreate()
        {
            return View();
        }

        public IActionResult LeaveList()
        {
            return View();
        }


        public IActionResult LeaveEdit()
        {
            return View();
        }
    }
}
