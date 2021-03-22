using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.ViewModel;
using MyHR_Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prjCoreDemo.ViewModel;
using Microsoft.AspNetCore.Http;

namespace MyHR_Web.Controllers
{
    
    public class PropertyController : Controller
    {
        private IHostingEnvironment iv_host;

        public PropertyController(IHostingEnvironment p)
        {
            iv_host = p;
        }
        //public JsonResult pSubject()
        //{
        //    var psubject = from l in (new dbMyCompanyContext()).TLostAndFoundSubjects
        //                select l;
        //    return Json(psubject);
        //}
        //public JsonResult pCategory()
        //{
        //    var pcategory = from c in (new dbMyCompanyContext()).TLostAndFoundCategories
        //                select c;
        //    return Json(pcategory);
        //}
        //public JsonResult pCheckStatus()
        //{
        //    var pcheck = from s in (new dbMyCompanyContext()).TLostAndFoundCheckStatuses
        //                select s;
        //    return Json(pcheck);
        //}
        public IActionResult List()
        {
            var propertytable = from p in (new dbMyCompanyContext()).TLostAndFounds
                                join d in (new dbMyCompanyContext()).TLostAndFoundSubjects on p.CPropertyCheckStatusId equals d.CPropertySubjectId
                                join e in (new dbMyCompanyContext()).TLostAndFoundCategories on p.CPropertyCategoryId equals e.CPropertyCategoryId
                                join f in (new dbMyCompanyContext()).TLostAndFoundCheckStatuses on p.CPropertyCheckStatusId equals f.CPropertyCheckStatusId
                                select new CPropertyViewModel
                                {
                                    CPropertyId = p.CPropertyId,
                                    CDeparmentId = p.CDeparmentId,
                                    CEmployeeId=p.CEmployeeId,
                                    CPhone=p.CPhone,
                                    CPropertySubjectId=d.CPropertySubjectId,
                                    CPropertyCategoryId=e.CPropertyCategoryId,
                                    CPropertyPhoto=p.CPropertyPhoto,
                                    CProperty=p.CProperty,
                                    CLostAndFoundDate=p.CLostAndFoundDate,
                                    CLostAndFoundSpace=p.CLostAndFoundSpace,
                                    CtPropertyDescription=p.CtPropertyDescription,
                                    CPropertyCheckStatusId=f.CPropertyCheckStatusId
                                };
            //List<CPropertyViewModel> list = new List<CPropertyViewModel>();
            //foreach (TLostAndFound t in propertytable)
            //    list.Add(new CPropertyViewModel(t));
            return View(propertytable);
        }

        public IActionResult Create()
        {
            string a = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            string b = HttpContext.Session.GetString(CDictionary.LOGIN_USERID);
            string c = HttpContext.Session.GetString(CDictionary.LOGIN_USERPHONE);
            ViewData[CDictionary.LOGIN_USERID] = HttpContext.Session.GetString(CDictionary.LOGIN_USERID);
            ViewData[CDictionary.LOGIN_USERPHONE] = HttpContext.Session.GetString(CDictionary.LOGIN_USERPHONE);
            return View();
        }

        [HttpPost]
        public IActionResult Create(CPropertyViewModel p)
        {
            dbMyCompanyContext db = new dbMyCompanyContext();
            db.TLostAndFounds.Add(p.property);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TLostAndFound d = db.TLostAndFounds.FirstOrDefault(t => t.CPropertyId == id);

                if (d != null)
                {
                    db.TLostAndFounds.Remove(d);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TLostAndFound d = db.TLostAndFounds.FirstOrDefault(t => t.CPropertyId == id);

                if (d != null)
                {
                    return View(new CPropertyViewModel(d));
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult Edit(CPropertyViewModel t_propertyEdit)
        {
            if (t_propertyEdit != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TLostAndFound l_product被修改 = db.TLostAndFounds.FirstOrDefault(t => t.CPropertyId == t_propertyEdit.CPropertyId);

                if (l_product被修改 != null)
                {
                    l_product被修改.CEmployeeId = t_propertyEdit.CEmployeeId;
                    l_product被修改.CDeparmentId = t_propertyEdit.CDeparmentId;
                    l_product被修改.CPhone = t_propertyEdit.CPhone;
                    l_product被修改.CPropertySubjectId = t_propertyEdit.CPropertySubjectId;
                    l_product被修改.CPropertyPhoto = t_propertyEdit.CPropertyPhoto;
                    l_product被修改.CProperty = t_propertyEdit.CProperty;
                    l_product被修改.CLostAndFoundDate = t_propertyEdit.CLostAndFoundDate;
                    l_product被修改.CLostAndFoundSpace = t_propertyEdit.CLostAndFoundSpace;
                    l_product被修改.CtPropertyDescription = t_propertyEdit.CtPropertyDescription;
                    l_product被修改.CPropertyCheckStatusId = t_propertyEdit.CPropertyCheckStatusId;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
    }
}
