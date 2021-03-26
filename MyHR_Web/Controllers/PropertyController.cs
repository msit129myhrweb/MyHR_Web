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
    
    public class PropertyController : Controller
    {
        private IHostingEnvironment iv_host;
        dbMyCompanyContext db = new dbMyCompanyContext();
       
        public PropertyController(IHostingEnvironment p)
        {
            iv_host = p;
        }

        public JsonResult pSubject()
        {
            var psubject = from l in db.TLostAndFoundSubjects
                           select l;
            return Json(psubject);
        }
        public JsonResult pCategory()
        {
            var pcategory = from c in db.TLostAndFoundCategories
                            select c;
            return Json(pcategory);
        }
        public JsonResult pCheckStatus()
        {
            var pcheck = from s in db.TLostAndFoundCheckStatuses
                         select s;
            return Json(pcheck);
        }
        public JsonResult propertyfound(CPropertyListViewModel model)
        {

            var pfound = from pf in db.TLostAndFounds
                         where pf.CLostAndFoundDate <=model.CLostAndFoundDate
                         select new
                         {
                                 CPropertyId = pf.CPropertyId,
                                 CDeparmentId = pf.CDeparmentId,
                                 CEmployeeId = pf.CEmployeeId,
                                 CPhone = pf.CPhone,
                                 CPropertySubjectId = pf.CPropertySubjectId,
                                 CPropertyCategoryId = pf.CPropertyCategoryId,
                                 CPropertyPhoto = pf.CPropertyPhoto,
                                 CProperty = pf.CProperty,
                                 CLostAndFoundDate = pf.CLostAndFoundDate,
                                 CLostAndFoundSpace = pf.CLostAndFoundSpace,
                                 CtPropertyDescription = pf.CtPropertyDescription,
                                 CPropertyCheckStatusId = pf.CPropertyCheckStatusId
                             };
            return Json(pfound);
        }
        public IActionResult List()
        {
            var propertytable = from p in db.TLostAndFounds
                                join d in db.TLostAndFoundSubjects on p.CPropertyCheckStatusId equals d.CPropertySubjectId
                                join e in db.TLostAndFoundCategories on p.CPropertyCategoryId equals e.CPropertyCategoryId
                                join f in db.TLostAndFoundCheckStatuses on p.CPropertyCheckStatusId equals f.CPropertyCheckStatusId
                                select new
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
            List<CPropertyListViewModel> plist = new List<CPropertyListViewModel>();
            foreach (var pitem in propertytable)
            {
                CPropertyListViewModel cvm = new CPropertyListViewModel()
                {
                    CPropertyId = pitem.CPropertyId,
                    CDeparmentId = pitem.CDeparmentId,
                    CEmployeeId = pitem.CEmployeeId,
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
            string uSERDEPARTMENT = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            string uSERID = HttpContext.Session.GetString(CDictionary.LOGIN_USERID);
            string uSERPHONE = HttpContext.Session.GetString(CDictionary.LOGIN_USERPHONE);
            ViewData[CDictionary.LOGIN_USERID] = HttpContext.Session.GetString(CDictionary.LOGIN_USERID);
            ViewData[CDictionary.LOGIN_USERPHONE] = HttpContext.Session.GetString(CDictionary.LOGIN_USERPHONE);

            return View();
        }

        [HttpPost]
        public IActionResult Create(CPropertyViewModel p)
        {
            //如果有上傳照片
            if(p.image != null)
            {
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                using (var photo = new FileStream(iv_host.ContentRootPath + @"\wwwroot\images\" + photoName, FileMode.Create))
                {
                    p.image.CopyTo(photo);
                }
                p.CPropertyPhoto = "../images/" + photoName;
            }
            var depart = db.TUserDepartments.FirstOrDefault(e => e.CDepartment == p.CDepartmentName);

            if(depart == null)
            {
                //TODO 找不到部門
                throw new NotImplementedException();
            }
            p.CDeparmentId = depart.CDepartmentId;
            db.TLostAndFounds.Add(p.property);
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
            string uSERDEPARTMENT = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            string uSERID = HttpContext.Session.GetString(CDictionary.LOGIN_USERID);
            string uSERPHONE = HttpContext.Session.GetString(CDictionary.LOGIN_USERPHONE);
            ViewData[CDictionary.LOGIN_USERID] = HttpContext.Session.GetString(CDictionary.LOGIN_USERID);
            ViewData[CDictionary.LOGIN_USERPHONE] = HttpContext.Session.GetString(CDictionary.LOGIN_USERPHONE);
            
            if (id != null)
            {
                TLostAndFound d = db.TLostAndFounds.FirstOrDefault(t => t.CPropertyId == id);
                TLostAndFoundSubject s = db.TLostAndFoundSubjects.FirstOrDefault(s => s.CPropertySubjectId == d.CPropertySubjectId);
                TLostAndFoundCategory c = db.TLostAndFoundCategories.FirstOrDefault(c => c.CPropertyCategoryId == d.CPropertyCategoryId);
                TLostAndFoundCheckStatus e = db.TLostAndFoundCheckStatuses.FirstOrDefault(e => e.CPropertyCheckStatusId == d.CPropertyCheckStatusId);

                
                if (d != null)
                {
                    var result = new CPropertyViewModel(d, s, c, e);
                    result.CDepartmentName = db.TUserDepartments.FirstOrDefault(e => e.CDepartmentId == result.CDeparmentId)?.CDepartment;
                    return View(result);
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult Edit(CPropertyViewModel t_propertyEdit)
        {
            if (t_propertyEdit != null)
            {
                TLostAndFound l_laf被修改 = db.TLostAndFounds.FirstOrDefault(t => t.CPropertyId == t_propertyEdit.CPropertyId);
                TLostAndFoundSubject l_Subject被修改 = db.TLostAndFoundSubjects.FirstOrDefault(t => t.CPropertySubjectId == t_propertyEdit.CPropertySubjectId);
                TLostAndFoundCategory l_Category被修改 = db.TLostAndFoundCategories.FirstOrDefault(t => t.CPropertyCategoryId == t_propertyEdit.CPropertyCategoryId);
               // TLostAndFoundCheckStatus l_CheckStatus被修改 = db.TLostAndFoundCheckStatuses.FirstOrDefault(t => t.CPropertyCheckStatusId == t_propertyEdit.CPropertyCheckStatusId);

                if (l_laf被修改 != null)
                {
                    l_laf被修改.CEmployeeId = t_propertyEdit.CEmployeeId;
                    
                    l_laf被修改.CDeparmentId = t_propertyEdit.CDeparmentId;
                    l_laf被修改.CPhone = t_propertyEdit.CPhone;
                    l_Subject被修改.CPropertySubjectId = t_propertyEdit.CPropertySubjectId;
                    l_Category被修改.CPropertyCategoryId = t_propertyEdit.CPropertyCategoryId;
                    l_laf被修改.CPropertyPhoto = t_propertyEdit.CPropertyPhoto;
                    l_laf被修改.CProperty = t_propertyEdit.CProperty;
                    l_laf被修改.CLostAndFoundDate = t_propertyEdit.CLostAndFoundDate;
                    l_laf被修改.CLostAndFoundSpace = t_propertyEdit.CLostAndFoundSpace;
                    l_laf被修改.CtPropertyDescription = t_propertyEdit.CtPropertyDescription;
                    l_laf被修改.CPropertyCheckStatusId = (int)t_propertyEdit.CPropertyCheckStatusId;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
    }
}
