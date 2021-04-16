using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyHR_Web.Controllers
{

    public class PropertyController : BaseController
    {
        dbMyCompanyContext db = new dbMyCompanyContext();

        private IHostingEnvironment iv_host;

        public PropertyController(IHostingEnvironment p)
        {
            iv_host = p;
        }

        private string uploadImage(IFormFile image)  //儲存照片
        {
            string photoName = $"{Guid.NewGuid()}.jpg";
            using (var photo = new FileStream(iv_host.ContentRootPath + @"\wwwroot\images\" + photoName, FileMode.Create))
            {
                image.CopyTo(photo);
            }
            return "/images/" + photoName;
        }

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
                tl = tl.Where(e => e.CLostAndFoundDate <= enddate.Value);
            }

            var propertytable = (from p in tl
                                join d in db.TLostAndFoundSubjects on p.CPropertySubjectId equals d.CPropertySubjectId
                                join e in db.TLostAndFoundCategories on p.CPropertyCategoryId equals e.CPropertyCategoryId
                                join f in db.TLostAndFoundCheckStatuses on p.CPropertyCheckStatusId equals f.CPropertyCheckStatusId
                                join u in db.TUsers on p.CEmployeeId equals u.CEmployeeId
                                select new CPropertyViewModel
                                {
                                    CPropertyId = p.CPropertyId,
                                    CDeparmentId = p.CDeparmentId,
                                    CEmployeeId = u.CEmployeeId,
                                    CEmployeeName = u.CEmployeeName,
                                    CPhone = u.CPhone,
                                    CPropertySubjectId = d.CPropertySubjectId,
                                    CPropertyCategoryId = e.CPropertyCategoryId,
                                    CPropertyPhoto = p.CPropertyPhoto,
                                    CProperty = p.CProperty,
                                    CLostAndFoundDate = p.CLostAndFoundDate,
                                    CLostAndFoundSpace = p.CLostAndFoundSpace,
                                    CtPropertyDescription = p.CtPropertyDescription,
                                    CPropertyCheckStatusId = f.CPropertyCheckStatusId
                                }).ToList();

            return View(propertytable);

        }

        public IActionResult Create()
        {
            ViewBag.Departments = db.TUserDepartments.ToList();
            ViewBag.check = db.TLostAndFoundCheckStatuses.ToList();
            ViewBag.subject = db.TLostAndFoundSubjects.ToList();
            ViewBag.category = db.TLostAndFoundCategories.ToList();
            ViewBag.cphone = db.TUsers.ToList();
            DateTime now = DateTime.UtcNow.AddHours(8).Date;
            
            return View(new CPropertyViewModel
            {
                CEmployeeName = getUserName(),
                CDeparmentId = getUserDepartmentId(),
                CEmployeeId = getUserId(),
                CPhone = getUserPhone(),
                CLostAndFoundDate = now
            });
        }

        [HttpPost]
        public IActionResult Create(CPropertyViewModel pmodel)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Departments = db.TUserDepartments.ToList();
                ViewBag.check = db.TLostAndFoundCheckStatuses.ToList();
                ViewBag.subject = db.TLostAndFoundSubjects.ToList();
                ViewBag.category = db.TLostAndFoundCategories.ToList();
                ViewBag.cphone = db.TUsers.ToList();
                return View(pmodel);
            }

            if (pmodel.image != null)
            {
                pmodel.CPropertyPhoto = uploadImage(pmodel.image);
            }

            db.TLostAndFounds.Add(new TLostAndFound
            {
                CDeparmentId = pmodel.CDeparmentId,
                CEmployeeId = pmodel.CEmployeeId,
                CLostAndFoundDate = DateTime.UtcNow.AddHours(8),
                CLostAndFoundSpace = pmodel.CLostAndFoundSpace,
                CPhone = pmodel.CPhone,
                CProperty = pmodel.CProperty,
                CPropertyCategoryId = pmodel.CPropertyCategoryId,
                CPropertyPhoto = pmodel.CPropertyPhoto,
                CPropertySubjectId = pmodel.CPropertySubjectId,
                CPropertyCheckStatusId = pmodel.CPropertyCheckStatusId,
                CtPropertyDescription = pmodel.CtPropertyDescription
            });

            db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
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
                CPropertyId = tf.CPropertyId,
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
            ViewBag.cphone = db.TUsers.ToList();

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
                ViewBag.cphone = db.TUsers.ToList();
                return View(pmodel);
            }

            var entity = db.TLostAndFounds.Where(e => e.CPropertyId == pmodel.CPropertyId).FirstOrDefault();

            if (entity == null)
            {
                return RedirectToAction("List");
            }

            entity.CDeparmentId = pmodel.CDeparmentId;
            entity.CEmployeeId = pmodel.CEmployeeId;
            entity.CPropertyId = pmodel.CPropertyId;
            entity.CProperty = pmodel.CProperty;
            entity.CPropertyCategoryId = pmodel.CPropertyCategoryId;
            entity.CLostAndFoundSpace = pmodel.CLostAndFoundSpace;
            entity.CPropertySubjectId = pmodel.CPropertySubjectId;
            entity.CtPropertyDescription = pmodel.CtPropertyDescription;
            entity.CPropertyCheckStatusId = pmodel.CPropertyCheckStatusId;
            entity.CLostAndFoundDate = pmodel.CLostAndFoundDate;
            entity.CPhone = pmodel.CPhone;

            if(pmodel.image != null)
            {
                entity.CPropertyPhoto = uploadImage(pmodel.image);
            }

            db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
