using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.MyClass;
using MyHR_Web.ViewModel;
using Newtonsoft.Json;
using prjCoreDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class InterviewController : FilterController
    {
        dbMyCompanyContext myHR = new dbMyCompanyContext();        

        public IActionResult List()
        {            
            List<TInterViewStatus> status = getInterViewStatus();
            ViewBag.InterViewStatus = status;
            List<TUserDepartment> dept = getDept();
            ViewBag.Dept = dept;
            ViewBag.Id = TempData["Id"];

            var table = myHR.TInterViews;
            List<CInterviewListViewModel> list = new List<CInterviewListViewModel>();
            foreach (TInterView i in table)
                list.Add(new CInterviewListViewModel(i));
            return View(list);
        }

        private List<TUserDepartment> getDept()
        {
            List<TUserDepartment> list = myHR.TUserDepartments.ToList();
            return list;
        }

        private List<TInterViewStatus> getInterViewStatus()
        {
            List<TInterViewStatus> list = myHR.TInterViewStatuses.ToList();            
            return list;            
        }

        public IActionResult Edit(int? Id)
        {
            if (Id != null)
            {
                List<TInterViewStatus> status = GetStatus();  //從資料庫抓下拉式選單
                ViewBag.Status = status;
                TInterView I = myHR.TInterViews.FirstOrDefault(n => n.CInterVieweeId == Id);
                if (I != null)
                {
                    ViewBag.CurrentStatus = I.CInterViewStatusId;
                    return PartialView("Edit", new CInterviewListViewModel(I));
                }
            }
            return RedirectToAction("List");
        }

        private List<TInterViewStatus> GetStatus() //獲取狀態下拉式選單
        {
            try
            {
                List<TInterViewStatus> list = new List<TInterViewStatus>();
                list = (from i in myHR.TInterViewStatuses
                        select i).ToList();
                return list;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
        }
        [HttpPost]
        public IActionResult Edit(CInterviewEditViewModel I)
        {
            if (I != null)
            {
                TInterView table = myHR.TInterViews.Where(n => n.CInterVieweeId == I.CInterVieweeId).FirstOrDefault();
                if (table != null)
                {
                    table.CInterVieweeName = I.CInterVieweeName;
                    table.CInterViewStatusId = int.Parse(I.CStatus);
                    myHR.SaveChanges();
                }
            }
            
            // 如果狀態為待報到 跳至新增帳號頁面
            if (int.Parse(I.CStatus) == 3)
            {
                var interviewer = myHR.TInterViews.Where(n => n.CInterVieweeId == I.CInterVieweeId).FirstOrDefault();
                //HttpContext.Session.SetObject<TInterView>(CDictionary.Register_User, interviewer);
                TempData.Put("Register", interviewer); 
                AddNoti(1, "新人報到通知", $"貴部門 {interviewer.CInterVieweeName} 確認報到，已發送報到通知。");
                return RedirectToAction("register", "employee");
            }
                
            TempData["Id"] = I.CInterVieweeId;
            
            return RedirectToAction("List");
        }

        public IActionResult Delete(int? Id)
        {
            if (Id != null)
            {
                TInterView I = myHR.TInterViews.FirstOrDefault(n => n.CInterVieweeId == Id);
                if (I != null)
                {
                    myHR.TInterViews.Remove(I);
                    myHR.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }

        public IActionResult Details(int? id)
        {
            if(id != null)
            {
                int? i = myHR.TInterViews.Where(n => n.CInterVieweeId == id).Select(n => n.CInterViewProcessId).FirstOrDefault();
                var table = myHR.TInterViewProcesses
                    .Join(
                    myHR.TInterViews.Where(n => n.CInterVieweeId == id),
                    p => p.CInterViewProcessId,
                    i => i.CInterViewProcessId,
                    (p, i) => new { P = p, I = i })
                    .Join(
                    myHR.TUserDepartments,
                    x => x.I.CDepartment,
                    d => d.CDepartmentId,
                    (x, d) => new { X = x, D = d })
                    .Select(s => new CInterviewDetailsViewModel
                    {
                        processId = s.X.P.CInterViewProcessId,
                        editor = s.X.P.CEditorNavigation.CEmployeeName,
                        process = s.X.P.CInterViewProcess,
                        departname = s.D.CDepartment,
                        processDate = s.X.P.CProcessTime
                    }).OrderByDescending(n => n.processDate);
                    
                return PartialView("Details", table);
            }
            else
                return RedirectToAction("List");
        }

        public IActionResult Filter(int? cate, int? dept)
        {
            var table = myHR.TInterViews
                .Where(n => (cate != null ? n.CInterViewStatusId == cate : true) &&
                (dept != null ? n.CDepartmentNavigation.CDepartmentId == dept : true));
            List<CInterviewListViewModel> I = new List<CInterviewListViewModel>();

            foreach (var item in table)
            {
                I.Add(new CInterviewListViewModel(item));
            }
            return PartialView("Filter", I);
        }
        public IActionResult ProcessCreate(int? id)
        {
            if (id != null)
            {
                var table = new CInterviewCreateViewModel { CInterViewProcessId = id.GetValueOrDefault()};
                ViewBag.id = myHR.TInterViews.Where(n => n.CInterVieweeId == id).Select(n => n.CInterViewProcessId).FirstOrDefault();
                return PartialView("ProcessCreate", table);
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult ProcessCreate(TInterViewProcess a)
        {
            try
            {
                TUser tuser = HttpContext.Session.GetObject<TUser>(CDictionary.Current_User);
                if (a != null)
                {
                    a.CProcessTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    a.CInterViewProcess = a.CInterViewProcess.Replace(System.Environment.NewLine, "<br/>");
                    a.CEditor = tuser.CEmployeeId;
                    myHR.TInterViewProcesses.Add(a);
                    myHR.SaveChanges();
                }
                TempData["Id"] = myHR.TInterViews.Where(n => n.CInterViewProcessId == a.CInterViewProcessId).Select(n => n.CInterVieweeId).FirstOrDefault();
                return RedirectToAction("List");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        [HttpPost]
        public JsonResult isOnboard(int id)
        {      
            string data = myHR.TInterViews.Where(n => n.CInterVieweeId == id).Select(n => n.CInterViewStatusId).FirstOrDefault().ToString();
            return Json(data);
        }
    }
}
