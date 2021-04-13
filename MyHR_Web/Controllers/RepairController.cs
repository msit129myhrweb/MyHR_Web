using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using prjCoreDemo.ViewModel;

namespace MyHR_Web.Views.Home
{
    public class RepairController : Controller
    {
     

        public IActionResult RepairList()
        {
            ViewData[CDictionary.CURRENT_LOGINED_USERNAME] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
            ViewBag.Session_USERID_USERDEPARTMENTID = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID));
            string USERDEPARTMENTID = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID);
            ViewBag.Session_USERIDSERID = int.Parse( HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            
            ViewBag.Session_USERJOBTITLE= HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERJOBTITLE);
            ViewBag.Session_USERJOBTITLEID = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERJOBTITLEID));
            int userid= int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
           

            IEnumerable< TRepair > table = null;
            if (ViewBag.Session_USERJOBTITLE == "經理"&&(ViewBag.Session_USERID_USERDEPARTMENTID == 4 || ViewBag.Session_USERID_USERDEPARTMENTID == 5 )) 
            {
                 table = from r in (new dbMyCompanyContext()).TRepairs
                         where (r.CRepairCategory.Contains("資訊") && int.Parse(USERDEPARTMENTID) == 4) || (r.CRepairCategory.Contains("總務") && int.Parse(USERDEPARTMENTID) == 5)
                         select r;
            } 
            else
            {
                if (!string.IsNullOrEmpty(Request.ContentType))
                            {
                                string Srepairnumber = Request.Form["repairnumber"];
                                string SrepairdateStart = Request.Form["repairdateStart"];
                                string SrepairdateEnd = Request.Form["repairdateEnd"];
                                string Srepaircontent = Request.Form["repaircontent"];

                                //單號不為空
                                if (!string.IsNullOrEmpty(Srepairnumber))
                                {
                                    table = from r in (new dbMyCompanyContext()).TRepairs
                                            where r.CRepairNumber == (int.Parse(Srepairnumber)) && r.CEmployeeId == userid
                                            orderby r.CAppleDate descending
                                            select r;
                                }//開始日期 結束日期 內容不為空
                                else if (!string.IsNullOrEmpty(SrepairdateStart) && !string.IsNullOrEmpty(SrepairdateEnd) && !string.IsNullOrEmpty(Srepaircontent))
                                {
                                    table = from r in (new dbMyCompanyContext()).TRepairs
                                            where r.CContentofRepair.Contains(Srepaircontent) && r.CAppleDate <= DateTime.Parse(SrepairdateEnd) && r.CAppleDate >= DateTime.Parse(SrepairdateStart) && r.CEmployeeId == userid
                                            orderby r.CAppleDate descending
                                            select r;
                                }//開始日期 結束日期不為空
                                else if (!string.IsNullOrEmpty(SrepairdateStart) && !string.IsNullOrEmpty(SrepairdateEnd))
                                {
                                    table = from r in (new dbMyCompanyContext()).TRepairs
                                            where r.CAppleDate <= DateTime.Parse(SrepairdateEnd) && r.CAppleDate >= DateTime.Parse(SrepairdateStart) && r.CEmployeeId == userid
                                            orderby r.CAppleDate descending
                                            select r;
                                }//開始日期  內容不為空
                                else if (!string.IsNullOrEmpty(SrepairdateStart) && !string.IsNullOrEmpty(Srepaircontent))
                                {
                                    table = from r in (new dbMyCompanyContext()).TRepairs
                                            where r.CAppleDate >= DateTime.Parse(SrepairdateStart) && r.CContentofRepair.Contains(Srepaircontent) && r.CEmployeeId == userid
                                            orderby r.CAppleDate descending
                                            select r;
                                }
                                // 結束日期 內容不為空
                                else if ( !string.IsNullOrEmpty(SrepairdateEnd) && !string.IsNullOrEmpty(Srepaircontent))
                                {
                                    table = from r in (new dbMyCompanyContext()).TRepairs
                                            where r.CAppleDate <= DateTime.Parse(SrepairdateEnd) && r.CContentofRepair.Contains(Srepaircontent) && r.CEmployeeId == userid
                                            orderby r.CAppleDate descending
                                            select r;
                                }//開始日期不為空
                                else if (!string.IsNullOrEmpty(SrepairdateStart))
                                {
                                    table = from r in (new dbMyCompanyContext()).TRepairs
                                            where r.CAppleDate >= DateTime.Parse(SrepairdateStart) && r.CEmployeeId == userid
                                            orderby r.CAppleDate descending
                                            select r;
                                }//結束日期不為空
                                else if (!string.IsNullOrEmpty(SrepairdateEnd))
                                {
                                    table = from r in (new dbMyCompanyContext()).TRepairs
                                            where r.CAppleDate <= DateTime.Parse(SrepairdateEnd) && r.CEmployeeId == userid
                                            orderby r.CAppleDate descending
                                            select r;
                                }//內容不為空
                                else
                                if (!string.IsNullOrEmpty(Srepaircontent))
                                {
                                    table = from r in (new dbMyCompanyContext()).TRepairs
                                            where r.CContentofRepair.Contains(Srepaircontent) && r.CEmployeeId == userid
                                            orderby r.CAppleDate descending
                                            select r;
                                }
                                else
                                {
                                    table = from r in (new dbMyCompanyContext()).TRepairs
                                            where r.CEmployeeId == userid
                                            orderby r.CAppleDate descending
                                            select r;
                                }
                            }
                            else
                            { 
                            table = from r in (new dbMyCompanyContext()).TRepairs
                                    where r.CEmployeeId== userid
                                    orderby r.CAppleDate descending
                                    select r;
                            }
            }    
            

                 
            
            List<CReairViewModel> list = new List<CReairViewModel>();
            foreach (TRepair i in table)
                list.Add(new CReairViewModel(i, null));
            return View(list);
            //return View(table);
        }
       
     
        public IActionResult RepairCreate()
        {
           
            ViewData[CDictionary.CURRENT_LOGINED_USERID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            ViewData[CDictionary.CURRENT_LOGINED_PHONE] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_PHONE);
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
                    return View(new CReairViewModel(repair, null));
                }

            }
            return RedirectToAction("RepairList");
        }

        [HttpPost]
        public IActionResult RepairEdit(CReairViewModel repair)
        { 
            dbMyCompanyContext db = new dbMyCompanyContext();
            if (repair != null)
            {

                TRepair c = db.TRepairs.FirstOrDefault(p => p.CRepairNumber == repair.CRepairNumber); 
                if (c != null)
                {  
                    c.CContentofRepair = repair.CContentofRepair;
                    c.CAppleDate =(DateTime)repair.CAppleDate;
                    c.CLocation = repair.CLocation;
                    c.CPhone = repair.CPhone;
                    c.CRepairCategory = repair.CRepairCategory;
                    c.CRepairStatus = (byte)repair.CRepairStatus;
                    

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
        public IActionResult update(int? id)
        {
            if (id != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TRepair repair = db.TRepairs.FirstOrDefault(c => c.CRepairNumber == id);
                
                if (repair != null)
                {
                    repair.CRepairStatus = 1;
                    db.SaveChanges();
                }
              
            }
            return RedirectToAction("RepairList");
        }


        public JsonResult updateall(string x)
        {
            string a = x;
            string[] ids =a.Split('\\','"','[',',',']');

            List<int> list = new List<int>();
            foreach (var item in ids)
            {

                if (item != "")
                {
                    list.Add(int.Parse(item));
                }

            }
            foreach (var i in list)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TRepair repair = db.TRepairs.FirstOrDefault(c => c.CRepairNumber == i);

                if (repair != null)
                {
                    repair.CRepairStatus = 1;
                    db.SaveChanges();
                }

                 
            }

            return Json(new { result = true, msg = "成功" });



        }
            
    
    }
}
