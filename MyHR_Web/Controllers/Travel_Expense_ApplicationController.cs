using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;

namespace MyHR_Web.Controllers
{
    public class Travel_Expense_ApplicationController : Controller
    {
        public IActionResult List()
        {
            var table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications
                        select travel;

            List<CTravelViewModel> list = new List<CTravelViewModel>();
            foreach (TTravelExpenseApplication item in table)
                list.Add(new CTravelViewModel(item));

            return View(list);
        }
    }
}
