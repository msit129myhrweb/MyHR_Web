using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class PCheckStatusController : BaseController
    {
        dbMyCompanyContext db = new dbMyCompanyContext();
        List<CPropertyViewModel> plist = new List<CPropertyViewModel>();
        public IActionResult List()
        {
            

            return View();
        }
    }
}
