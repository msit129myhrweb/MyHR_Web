using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using prjCoreDemo.ViewModel;

namespace MyHR_Web.Controllers
{
    public class Travel_Expense_ApplicationController : Controller
    {
        public IActionResult List()
        {
            int DepId = int.Parse(HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERDEPARTMENTID));
            //IEnumerable<TTravelExpenseApplication> table = null;
            List<CTravelViewModel> list = new List<CTravelViewModel>();
            dbMyCompanyContext DB = new dbMyCompanyContext();

            //searching
            if (!string.IsNullOrEmpty(Request.ContentType))
            {
                string AppNum = Request.Form["txtAppNum"];
                string Id = Request.Form["txtId"]; 
                string Name= Request.Form["txtName"];
                string Reason = Request.Form["txtReason"]; 

                //全部條件為空白
                if (string.IsNullOrEmpty(AppNum)&&string.IsNullOrEmpty(Id)&&string.IsNullOrEmpty(Name)&&string.IsNullOrEmpty(Reason))
                {
                    var table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                            join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                            where travel.CDepartmentId == DepId
                            select travel;
                                
                    //todo inner join TUser Name

                }
                //其一有值
                else if (!string.IsNullOrEmpty(AppNum) || !string.IsNullOrEmpty(Id) || !string.IsNullOrEmpty(Name) ||!string.IsNullOrEmpty(Reason))
                {
                    var table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                            join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                            where travel.CDepartmentId == DepId &&
                                  travel.CReason.Contains(Reason) ||
                                  travel.CApplyNumber.ToString().Contains(AppNum) ||
                                  travel.CEmployeeId.ToString().Contains(Id) ||
                                  user.CEmployeeName.Contains(Name)
                            select travel;
                }
                ////其二有值
                else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Reason))
                {
                    var table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                                join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      travel.CReason.Contains(Reason) &&
                                      travel.CApplyNumber.ToString().Contains(AppNum) &&
                                      travel.CEmployeeId.ToString().Contains(Id) &&
                                      user.CEmployeeName.Contains(Name)
                                select travel;
                }
                ////其三有值
                else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Reason))
                {
                    var table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                                join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                                where travel.CDepartmentId == DepId &&
                                      travel.CReason.Contains(Reason) &&
                                      travel.CApplyNumber.ToString().Contains(AppNum) &&
                                      travel.CEmployeeId.ToString().Contains(Id) &&
                                      user.CEmployeeName.Contains(Name)
                                select travel;
                }
                //全部有值
                else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Reason))
                {
                    var table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                            join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                            where travel.CDepartmentId == DepId && 
                                  travel.CReason.Contains(Reason)&&
                                  travel.CApplyNumber.ToString().Contains(AppNum)&&
                                  travel.CEmployeeId.ToString().Contains(Id)&&
                                  user.CEmployeeName.Contains(Name)
                            select travel;
                }
            }
            //全部條件為空白
            else
            {
                var table = from travel in DB.TTravelExpenseApplications
                            join user in DB.TUsers
                            on travel.CEmployeeId equals user.CEmployeeId
                            where travel.CDepartmentId == DepId
                            select new
                            {
                                user.CEmployeeName,
                                travel.CApplyDate,
                                travel.CEmployeeId,
                                travel.CAmont,
                                travel.CCheckStatus,
                                travel.CTravelStartTime,
                                travel.CTravelEndTime,
                                travel.CReason,
                                travel.CApplyNumber
                            };

                foreach (var item in table)
                {
                    CTravelViewModel traObj = new CTravelViewModel()
                    {
                        employeeName = item.CEmployeeName,
                        CApplyNumber = item.CApplyNumber,
                        CApplyDate = item.CApplyDate,
                        CTravelStartTime = item.CTravelStartTime,
                        CTravelEndTime = item.CTravelEndTime,
                        CAmont = item.CAmont,
                        CReason = item.CReason,
                        CEmployeeId = item.CEmployeeId,
                        CCheckStatus = item.CCheckStatus
                    };
                    list.Add(traObj);
                }
            }
            return View(list);
        }

        #region Edit
        //通過或退件
        public IActionResult Edit(int? capplyNum)
        {
            if (capplyNum != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TTravelExpenseApplication travel = db.TTravelExpenseApplications.FirstOrDefault(t => t.CApplyNumber == capplyNum);
                if (travel != null)
                {
                    return View(new CTravelViewModel(travel));
                }
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Edit(CTravelViewModel travelEdit)
        {
            if (travelEdit != null)
            {
                dbMyCompanyContext db = new dbMyCompanyContext();
                TTravelExpenseApplication travel_Edited = db.TTravelExpenseApplications.FirstOrDefault(t => t.CApplyNumber == travelEdit.CApplyNumber);
                if (travel_Edited != null)
                {
                    travel_Edited.CCheckStatus = travelEdit.CCheckStatus;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
        #endregion
        public IActionResult Detail(int? capplyNum)
        {
            return View();
        }
    }
    
}
