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
        public IActionResult List(DateTime? startdate = null, DateTime? enddate = null)
        {
            //todo 樣式沒搞定
            ViewBag.stardate = startdate;
            ViewBag.enddate = enddate;
            
            IQueryable<TLostAndFound> tl = db.TLostAndFounds.AsQueryable();
            if (startdate.HasValue)
            {
                tl = tl.Where(e => e.CLostAndFoundDate >= startdate.Value);
            }
            if (enddate.HasValue)
            {
                tl = tl.Where(e => e.CLostAndFoundDate < enddate.Value);
            }
            
            var propertytable = from p in tl
                                join d in db.TLostAndFoundSubjects on p.CPropertyCheckStatusId equals d.CPropertySubjectId
                                join e in db.TLostAndFoundCategories on p.CPropertyCategoryId equals e.CPropertyCategoryId
                                join f in db.TLostAndFoundCheckStatuses on p.CPropertyCheckStatusId equals f.CPropertyCheckStatusId
                                join u in db.TUsers on p.CEmployeeId equals u.CEmployeeId
                                select new
                                {
                                    CPropertyId = p.CPropertyId,
                                    CDeparmentId = p.CDeparmentId,
                                    CEmployeeId = p.CEmployeeId,
                                    CEmployeeName=u.CEmployeeName,
                                    CPhone = p.CPhone,
                                    CPropertySubjectId = d.CPropertySubjectId,
                                    CPropertyCategoryId = e.CPropertyCategoryId,
                                    CPropertyPhoto = p.CPropertyPhoto,
                                    CProperty = p.CProperty,
                                    CLostAndFoundDate = p.CLostAndFoundDate,
                                    CLostAndFoundSpace = p.CLostAndFoundSpace,
                                    CtPropertyDescription = p.CtPropertyDescription,
                                    CPropertyCheckStatusId = f.CPropertyCheckStatusId
                                };

            foreach (var pitem in propertytable)
            {
                CPropertyViewModel cvm = new CPropertyViewModel()
                {
                    CPropertyId = pitem.CPropertyId,
                    CDeparmentId = pitem.CDeparmentId,
                    CEmployeeId = pitem.CEmployeeId,
                    CEmployeeName =pitem.CEmployeeName,
                    CPhone = pitem.CPhone,
                    CPropertySubjectId = pitem.CPropertySubjectId,
                    CPropertyCategoryId = pitem.CPropertyCategoryId,
                    CPropertyPhoto = pitem.CPropertyPhoto,
                    CProperty = pitem.CProperty,
                    CLostAndFoundDate = pitem.CLostAndFoundDate,
                    CLostAndFoundSpace = pitem.CLostAndFoundSpace,
                    CtPropertyDescription = pitem.CtPropertyDescription,
                    CPropertyCheckStatusId = pitem.CPropertyCheckStatusId
                };
                plist.Add(cvm);
            }
            return View(plist);
        }
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                TLostAndFound tlaf = db.TLostAndFounds.FirstOrDefault(e => e.CPropertyId == id);

                if (tlaf != null)
                {
                    tlaf.CPropertyCheckStatusId = 2;
                    db.SaveChanges();
                }

            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public IActionResult Edit(CPropertyViewModel tlaf)
        {
            
            if (tlaf != null)
            {

                TLostAndFound c = db.TLostAndFounds.FirstOrDefault(e => e.CPropertyId == tlaf.CPropertyId);
                if (c != null)
                {
                    c.CPropertyCheckStatusId = tlaf.CPropertyCheckStatusId;

                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
        public IActionResult pUpdate(int? id)
        {
            //todo 已領取功能
            if(id != null)
            {
                TLostAndFound tlaf = db.TLostAndFounds.FirstOrDefault(e => e.CPropertyId == id);

                if (tlaf != null)
                {
                    tlaf.CPropertyCheckStatusId = 2;
                    db.SaveChanges();
                }
            }
            
            return RedirectToAction("List");
        }

        public JsonResult updateall(string x)
        {
            string a = x;
            string[] ids = a.Split('\\', '"', '[', ',', ']');

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
                TLostAndFound tlaf = db.TLostAndFounds.FirstOrDefault(e => e.CPropertyId == i);
                
                if (tlaf != null)
                {
                    tlaf.CPropertyCheckStatusId = 2;
                    db.SaveChanges();
                }
            }

            return Json(new { result = true, msg = "成功" });


        }
    }
}
