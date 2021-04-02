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
            if (id.HasValue == false)
            {
                return RedirectToAction("List");
            }
            TLostAndFound tf = db.TLostAndFounds.Where(e => e.CPropertyId == id).FirstOrDefault();

            if (tf == null)
            {
                return RedirectToAction("List");
            }

            var result = new CPropertyViewModel
            {
                CLostAndFoundDate = tf.CLostAndFoundDate,
                CEmployeeId = tf.CEmployeeId,
                CLostAndFoundSpace = tf.CLostAndFoundSpace,
                CPropertyCheckStatusId = tf.CPropertyCheckStatusId,
                CPhone = getUserPhone(),
                CPropertySubjectId = tf.CPropertySubjectId,
                CtPropertyDescription = tf.CtPropertyDescription,
                CDeparmentId = tf.CDeparmentId,
                CPropertyCategoryId = tf.CPropertyCategoryId,
                CProperty = tf.CProperty,
                CPropertyPhoto = tf.CPropertyPhoto
            };
            ViewBag.Departments = db.TUserDepartments.ToList();
            ViewBag.check = db.TLostAndFoundCheckStatuses.ToList();
            ViewBag.subject = db.TLostAndFoundSubjects.ToList();
            ViewBag.category = db.TLostAndFoundCategories.ToList();

            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(CPropertyViewModel pmodel)
        {

            if (ModelState.IsValid == false)
            {
                ViewBag.Departments = db.TUserDepartments.ToList();
                ViewBag.check = db.TLostAndFoundCheckStatuses.ToList();
                ViewBag.subject = db.TLostAndFoundSubjects.ToList();
                ViewBag.category = db.TLostAndFoundCategories.ToList();
                return View(pmodel);
            }
            var entity = db.TLostAndFounds.Where(e => e.CPropertyId == pmodel.CPropertyId).FirstOrDefault();

            if (entity == null)
            {
                return RedirectToAction("List");
            }
            else
            {
                entity.CPropertyCheckStatusId = pmodel.CPropertyCheckStatusId;

                db.SaveChanges();
                return RedirectToAction("List");
            }          
         
        }
    }
}
