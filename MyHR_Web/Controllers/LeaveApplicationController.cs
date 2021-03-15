using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;

namespace MyHR_Web.Controllers
{
    public class LeaveApplicationController : Controller
    {
        public IActionResult List()
        {
            var table = from leave in (new dbMyCompanyContext()).TLeaveApplications
                        select leave;
            List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();
            foreach (TLeaveApplication item in table)
                list.Add(new TLeaveApplicationViewModel(item));

            return View(list) ;
        }
    }
}
