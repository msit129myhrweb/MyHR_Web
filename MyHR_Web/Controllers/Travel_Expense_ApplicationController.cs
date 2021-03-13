using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyHR_Web.Controllers
{
    public class Travel_Expense_ApplicationController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
