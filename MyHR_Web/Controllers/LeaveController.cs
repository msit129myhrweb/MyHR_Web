using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using Newtonsoft.Json;
using prjCoreDemo.ViewModel;

namespace MyHR_Web.Controllers
{
    public class LeaveController : FilterController
    {
        dbMyCompanyContext MyHr = new dbMyCompanyContext();

        public IActionResult LeaveList()  //請假清單查詢
        {

            List<TLeave> listLeave = GetLeaveList();  //從資料庫得下拉是選單
            ViewBag.LeaveCate = listLeave;

            List<TCheckStatus> listStatus = GetStatus();
            ViewBag.LeaveStatus = listStatus;

            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENTID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID);
            ViewData[CDictionary.CURRENT_LOGINED_USERNAME] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
            ViewBag.Name = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
         

            int UserID = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));

            var table = from i in MyHr.TLeaveApplications
                        join d in MyHr.TUserDepartments on i.CDepartmentId equals d.CDepartmentId
                        join u in MyHr.TUsers on i.CEmployeeId equals u.CEmployeeId
                        orderby i.CApplyDate descending /*依照申請日期降冪排序*/
                        select new TLeaveApplicationViewModel
                        {
                            CApplyNumber = i.CApplyNumber,
                            CEmployeeId = i.CEmployeeId,
                            CEmployeeName = u.CEmployeeName,
                            CDepartmentId = i.CDepartmentId,
                            CDepartmentName = d.CDepartment,
                            CApplyDate = i.CApplyDate,
                            CLeaveCategory = i.CLeaveCategory,
                            CLeaveStartTime = i.CLeaveStartTime,
                            CLeaveEndTime = i.CLeaveEndTime,
                            CReason = i.CReason,
                            CCheckStatus = i.CCheckStatus,
                            CLeaveHours=i.CLeaveHours,
                            Leave_Psy = 6,
                            
                            
                        };

            return View(table);

            

          

            //******************************************************************************************************
            //int useraccount = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            //List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();

            //list = //from i in 
            //           MyHr.TLeaveApplications
            //            .Include(c => c.CDepartment)
            //            .Include(c => c.CLeaveCategoryNavigation)
            //            .Include(c => c.CCheckStatusNavigation)
            //            .Select(c => new TLeaveApplicationViewModel
            //            {
            //                CApplyNumber = c.CApplyNumber,
            //                CDepartmentId = c.CDepartment.CDepartmentId,
            //                CDepartmentName = c.CDepartment.CDepartment,
            //                CEmployeeId = c.CEmployeeId,
            //                CApplyDate = c.CApplyDate,
            //                CLeaveCategoryId = c.CLeaveCategoryNavigation.CLeaveId,
            //                CLeaveCategory = c.CLeaveCategoryNavigation.CLeaveCategory,
            //                CLeaveStartTime = c.CLeaveStartTime,
            //                CLeaveEndTime = c.CLeaveEndTime,
            //                CReason = c.CReason,
            //                CCheckStatusId = c.CCheckStatusNavigation.CCheckStatusId,
            //                CCheckStatus = c.CCheckStatusNavigation.CCheckStatus
            //            })
            //            .Where(c => c.CEmployeeId == useraccount)
            //            .OrderByDescending(c => c.CApplyDate).ToList();

            //return View(list);
            //int UserID = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            //******************************************************************************************************

        }

        public IActionResult LeaveCreate()
        {


            List<TLeave> listLeave = GetLeaveList();  //從資料庫得下拉是選單
            ViewBag.LeaveCate = listLeave;
            //TempData.Keep["leave_cate"] = "listLeave";


            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENTID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID);
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            ViewData[CDictionary.CURRENT_LOGINED_USERID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            ViewData[CDictionary.CURRENT_LOGINED_USERNAME] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
            ViewData["Today"] = DateTime.Now.ToString("yyyy-MM-dd");


            var departmentsQuery = from d in MyHr.TUserDepartments
                                   orderby d.CDepartmentId // Sort by name.
                                   select d;

            //DepartmentNameSL = new SelectList(departmentsQuery.AsNoTracking(),
            //            "DepartmentID", "Name", selectedDepartment);


            return View();
        }

        [HttpPost] /*我有修改TLeaveApplicationViewModel裡面的全域變數名稱*/
        [ValidateAntiForgeryToken]
        public IActionResult LeaveCreate(TLeaveApplicationCreateViewModel T)
        {
            //using (var context = new dbMyCompanyContext())
            //{
            //    var item = new item { };
            //    context.Blogs.Add(blog);
            //    context.SaveChanges();
            //}


            //List<TLeaveApplicationViewModel> list = new List<TLeaveApplicationViewModel>();
            //list = MyHr.TLeaveApplications
            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid) //[Reurired]在CORE可以成功使用
            {
                MyHr.TLeaveApplications.Add(T.Leave);
                //await MyHr.SaveChangesAsync();
                MyHr.SaveChanges();
                return RedirectToAction("LeaveList");
            }
            else
            {

                //ModelState.AddModelError("CReason", "幹這個錯誤訊息跳出來，就算輸入資料了，也不會驗證過，不懂存在的意義");
                
                List<TLeave> listLeave = GetLeaveList();  //★我怎麼這抹巧!!!★
                ViewBag.LeaveCate = listLeave;
                return View();
            }


            //return RedirectToAction("LeaveCreate");
        }



        public IActionResult Delete(int? Id) //刪除
        {
            if (Id != null)
            {
                TLeaveApplication T = MyHr.TLeaveApplications.FirstOrDefault(i => i.CApplyNumber == Id);
                if (T != null)
                {
                    MyHr.TLeaveApplications.Remove(T);
                    MyHr.SaveChanges();
                }
            }

            return RedirectToAction("LeaveList");
        }


        public IActionResult Edit(int? Id)
        {


            List<TLeave> listLeave = GetLeaveList();  //從資料庫取假別下拉是選單
            ViewBag.LeaveCate = listLeave;

         

            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENTID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID);
            ViewData[CDictionary.CURRENT_LOGINED_USERDEPARTMENT] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENT);
            ViewData[CDictionary.CURRENT_LOGINED_USERID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            ViewData[CDictionary.CURRENT_LOGINED_USERNAME] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);
            ViewData["Today"] = DateTime.Now.ToString("yyyy-MM-dd");

            if (Id != null)
            {
                TLeaveApplication T = MyHr.TLeaveApplications.FirstOrDefault(i => i.CApplyNumber == Id);
                if (T != null)
                {
                    return View(new TLeaveApplicationEditViewModel(T));
                }
            }
            return RedirectToAction("LeaveList");
        }

        [HttpPost]
        public IActionResult Edit(TLeaveApplicationCreateViewModel T)
        {

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                if (T != null)
                {
                    TLeaveApplication _revised = MyHr.TLeaveApplications.FirstOrDefault(i => i.CApplyNumber == T.CApplyNumber);

                    if (_revised != null)
                    {
                        _revised.CApplyDate = T.CApplyDate;
                        _revised.CLeaveCategory = T.CLeaveCategory;
                        _revised.CLeaveStartTime = T.CLeaveStartTime;
                        _revised.CLeaveEndTime = T.CLeaveEndTime;
                        _revised.CReason = T.CReason;
                        _revised.CLeaveHours = T.CLeaveHours;

                        MyHr.SaveChanges();
                    }
                }

                return RedirectToAction("LeaveList");
            }
            else
            {
                List<TLeave> listLeave = GetLeaveList();
                ViewBag.LeaveCate = listLeave;
                return View();
            }
            
           
        }

        private List<TLeave> GetLeaveList() //獲取類別種類(使用在下拉是選單)
         {
            try
            {
                List<TLeave> list = new List<TLeave>();
                list = (from i in MyHr.TLeaves
                             select i).ToList();
                return list;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
                return null;
            }
         }

        private List<TCheckStatus> GetStatus() //獲取審核狀態(使用在下拉是選單)
        {
            try
            {
                List<TCheckStatus> list = new List<TCheckStatus>();
                list = MyHr.TCheckStatuses.ToList();
                return list;

            }
            catch(Exception ex)
            {
                string erroe = ex.ToString();
                return null;
            }
        }


        public IActionResult Mutiple_search(int? cate, int? status, string? start, string? end)
        {
            //List<TLeaveApplication> table = MyHr.TLeaveApplications.Where(n => n.CLeaveCategory == cate).ToList();


            //string str_json = JsonConvert.SerializeObject(table, Formatting.Indented);
            //return str_json;
            //顯示JSON字串
            //li_showData.Text = str_json;
            ViewBag.UserId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID));
            ViewBag.UserName = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERNAME);


            var table = MyHr.TLeaveApplications
                .Join(MyHr.TUserDepartments, d => d.CDepartmentId, u => u.CDepartmentId, (d, u) => new
                {

                    CApplyNumber = d.CApplyNumber,
                    CEmployeeId = d.CEmployeeId,
                    //CEmployeeName = u.CEmployeeName,
                    CDepartmentId = d.CDepartmentId,
                    CDepartmentName = u.CDepartment,
                    CApplyDate = d.CApplyDate,
                    CLeaveCategory = d.CLeaveCategory,
                    CLeaveStartTime = d.CLeaveStartTime,
                    CLeaveEndTime = d.CLeaveEndTime,
                    CReason = d.CReason,
                    CCheckStatus = d.CCheckStatus,
                    CLeaveHours = d.CLeaveHours,
                    
                    
                }).OrderBy(du => du.CApplyDate).AsEnumerable().Where(du =>
                (cate != null ? du.CLeaveCategory == cate:true) &&
                (status != null ? du.CCheckStatus == status : true) &&
                (start != null ? DateTime.Parse(du.CLeaveEndTime) >= DateTime.Parse(start) : true) &&
                (end !=null? DateTime.Parse(du.CLeaveStartTime) <= DateTime.Parse(end):true) &&
                ((end !=null && start!=null)? DateTime.Parse(du.CLeaveEndTime) >= DateTime.Parse(start) && DateTime.Parse(du.CLeaveStartTime) <= DateTime.Parse(end) : true)
                ).ToList();

            //int abc = table.Count;   ■ 這邊搜尋的資料並未針對USER，當沒有資料想要在前端做變化時，應當加入此條件。
            List<TLeaveApplicationViewModel> T = new List<TLeaveApplicationViewModel>();
           
            foreach (var item in table)
            {
                TLeaveApplicationViewModel obj = new TLeaveApplicationViewModel()
                {
                    CApplyNumber = item.CApplyNumber,
                    CEmployeeId = item.CEmployeeId,
                    CDepartmentId = item.CDepartmentId,
                    CDepartmentName = item.CDepartmentName,
                    CApplyDate = item.CApplyDate,
                    CLeaveCategory = item.CLeaveCategory,
                    CLeaveStartTime = item.CLeaveStartTime,
                    CLeaveEndTime = item.CLeaveEndTime,
                    CReason = item.CReason,
                    CCheckStatus = item.CCheckStatus,
                    CLeaveHours = item.CLeaveHours

                };
                T.Add(obj);
               
            }


            //foreach (TLeaveApplication C in table)                
            //    T.Add(new TLeaveApplicationViewModel(C,null));


           
            return PartialView("Mutiple_search", T);


        }

      
    }
}