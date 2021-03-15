using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;

namespace MyHR_Web.Controllers
{
    public class AbsenceController : Controller
    {
        public IActionResult List()
        {
            var table = from absence in (new dbMyCompanyContext()).TAbsences
                        select absence;
            List<CAbsenceViewModel> list = new List<CAbsenceViewModel>();
            foreach (TAbsence item in table)
                list.Add(new CAbsenceViewModel(item));
            
            return View(list);
        }
    }
}
