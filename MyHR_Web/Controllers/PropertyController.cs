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
using System.IO;

namespace MyHR_Web.Controllers
{

    public class PropertyController : BaseController
    {
        dbMyCompanyContext db = new dbMyCompanyContext();
        List<CPropertyViewModel> plist = new List<CPropertyViewModel>();

        private IHostingEnvironment iv_host;

        public PropertyController(IHostingEnvironment p)
        {
            iv_host = p;
        }

        public IActionResult List(DateTime? startdate = null,DateTime? enddate = null)
        {
             ViewBag.stardate=startdate;
             ViewBag.enddate=enddate;
            IQueryable<TLostAndFound> tl = db.TLostAndFounds.AsQueryable();
            if (startdate.HasValue)
            {
                tl = tl.Where(e => e.CLostAndFoundDate >= startdate.Value);
            }
            if (enddate.HasValue)
            {
                tl = tl.Where(e => e.CLostAndFoundDate <= enddate.Value);
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
                                    CEmployeeName = u.CEmployeeName,
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
                        CEmployeeName = pitem.CEmployeeName,
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

        public IActionResult Create()
        {
            ViewBag.Departments =db.TUserDepartments.ToList();
            ViewBag.check = db.TLostAndFoundCheckStatuses.ToList();
            ViewBag.subject = db.TLostAndFoundSubjects.ToList();
            ViewBag.category = db.TLostAndFoundCategories.ToList();
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
                return View(pmodel);
            }
           

            string photoName = Guid.NewGuid().ToString() + ".jpg";
            using (var photo = new FileStream(
                iv_host.ContentRootPath + @"\wwwroot\images\" + photoName,
                FileMode.Create))
            {
                pmodel.image.CopyTo(photo);
            }
            pmodel.CPropertyPhoto = "../images/" + photoName;

            db.TLostAndFounds.Add(new TLostAndFound
            {
                CDeparmentId=pmodel.CDeparmentId,
                CEmployeeId=pmodel.CEmployeeId,
                CLostAndFoundDate =DateTime.UtcNow.AddHours(8),
                CLostAndFoundSpace=pmodel.CLostAndFoundSpace,
                CPhone=pmodel.CPhone,
                CProperty=pmodel.CProperty,
                CPropertyCategoryId=pmodel.CPropertyCategoryId,
                CPropertyPhoto=pmodel.CPropertyPhoto,
                CPropertySubjectId=pmodel.CPropertySubjectId,
                CPropertyCheckStatusId=pmodel.CPropertyCheckStatusId,
                CtPropertyDescription=pmodel.CtPropertyDescription
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
            string photoName = Guid.NewGuid().ToString() + ".jpg";
            tf.CPropertyPhoto = "../images/" + photoName;

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
                CProperty=tf.CProperty,
                CPropertyPhoto=tf.CPropertyPhoto
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
            //無法修改 及圖片
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
                entity.CProperty = pmodel.CProperty;
                entity.CPropertyCategoryId = pmodel.CPropertyCategoryId;
                entity.CLostAndFoundSpace = pmodel.CLostAndFoundSpace;
                entity.CPropertySubjectId = pmodel.CPropertyCheckStatusId;
                entity.CtPropertyDescription = pmodel.CtPropertyDescription;
                entity.CPropertyCheckStatusId = pmodel.CPropertyCheckStatusId;

                string photoName = Guid.NewGuid().ToString() + ".jpg";
                using (var photo = new FileStream(
                    iv_host.ContentRootPath + @"\wwwroot\images\" + photoName,
                    FileMode.Create))
                {
                    pmodel.image.CopyTo(photo);
                }

                pmodel.CPropertyPhoto = "../images/" + photoName;
                entity.CPropertyPhoto = pmodel.CPropertyPhoto;

                db.SaveChanges();
                return RedirectToAction("List");
            }
    }
}
