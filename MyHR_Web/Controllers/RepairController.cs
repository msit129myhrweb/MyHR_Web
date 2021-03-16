using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;

namespace MyHR_Web.Views.Home
{
    public class RepairController : Controller
    {

        public IActionResult RepairList()
        {
            IEnumerable<TRepair> table = null;

            if (!string.IsNullOrEmpty(Request.ContentType))
            {
                string keyword = Request.Form["repairnumber"];
                table = from r in (new dbMyCompanyContext()).TRepairs
                        where r.CRepairNumber==(int.Parse(keyword))
                        select r;
            }
            else
            { 
            table = from r in (new dbMyCompanyContext()).TRepairs
                        select r;
            }

                 
            
            List<CReairViewModel> list = new List<CReairViewModel>();
            foreach (TRepair i in table)
                list.Add(new CReairViewModel(i));
            return View(list);
            //return View(table);
        }
        public IActionResult RepairCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RepairCreate(CReairViewModel cReair)
        {
            dbMyCompanyContext db = new dbMyCompanyContext();
            db.TRepairs.Add(cReair.repair);
            db.SaveChanges();
            return RedirectToAction("RepairList");


        }

       

        public IActionResult RepairEdit(int? id)
        {
            if (id != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TRepair repair = db.TRepairs.FirstOrDefault(c => c.CRepairNumber == id);

                if (repair != null)
                {
                    return View(repair);
                }

            }
            return RedirectToAction("RepairList");
        }

        [HttpPost]
        public IActionResult RepairEdit(TRepair repair)
        {
            if (repair != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();

                TRepair c = db.TRepairs.FirstOrDefault(p => p.CRepairNumber == repair.CRepairNumber);
                if (c != null)
                {  
                    c.CContentofRepair = repair.CContentofRepair;
                    c.CAppleDate = repair.CAppleDate;
                    c.CLocation = repair.CLocation;
                    c.CPhone = repair.CPhone;
                    c.CRepairCategory = repair.CRepairCategory;
                    c.CRepairStatus = repair.CRepairStatus;
                    

                    db.SaveChanges();
                }
            }
            return RedirectToAction("RepairList");
        }

        public IActionResult RepairDelete(int? id)
        {
            if (id != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TRepair repair = db.TRepairs.FirstOrDefault(c => c.CRepairNumber == id);

                if (repair != null)
                {
                    db.TRepairs.Remove(repair);
                    db.SaveChanges();
                }

            }
            return RedirectToAction("RepairList");
        }
    }
}
