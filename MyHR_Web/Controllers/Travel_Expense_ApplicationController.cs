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
            IEnumerable<TTravelExpenseApplication> table = null;

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
                    table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                            join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                            where travel.CDepartmentId == DepId
                            select travel;
                                
                    //todo inner join TUser Name

                }
                //其一有值
                else if (!string.IsNullOrEmpty(AppNum) || !string.IsNullOrEmpty(Id) || !string.IsNullOrEmpty(Name) ||!string.IsNullOrEmpty(Reason))
                {
                    table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                            join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                            where travel.CDepartmentId == DepId &&
                                  travel.CReason.Contains(Reason) ||
                                  travel.CApplyNumber.ToString().Contains(AppNum) ||
                                  travel.CEmployeeId.ToString().Contains(Id) ||
                                  user.CEmployeeName.Contains(Name)
                            select travel;
                }
                ////其二有值
                //else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Reason))
                //{
                //    table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                //            join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                //            where travel.CDepartmentId == DepId &&
                //                  travel.CReason.Contains(Reason) &&
                //                  travel.CApplyNumber.ToString().Contains(AppNum) &&
                //                  travel.CEmployeeId.ToString().Contains(Id) &&
                //                  user.CEmployeeName.Contains(Name)
                //            select travel;
                //}
                ////其三有值
                //else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Reason))
                //{
                //    table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                //            join user in (new dbMyCompanyContext()).TUsers on travel.CEmployeeId equals user.CEmployeeId
                //            where travel.CDepartmentId == DepId &&
                //                  travel.CReason.Contains(Reason) &&
                //                  travel.CApplyNumber.ToString().Contains(AppNum) &&
                //                  travel.CEmployeeId.ToString().Contains(Id) &&
                //                  user.CEmployeeName.Contains(Name)
                //            select travel;
                //}
                //全部有值
                else if (!string.IsNullOrEmpty(AppNum) && !string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Reason))
                {
                    table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
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
                table = from travel in (new dbMyCompanyContext()).TTravelExpenseApplications.AsEnumerable()
                        where travel.CDepartmentId == DepId
                        select travel;
            }
            List<CTravelViewModel> list = new List<CTravelViewModel>();
            foreach (TTravelExpenseApplication item in table)
                list.Add(new CTravelViewModel(item));

            return View(list);
        }

        //public static IEnumerable<CTravelViewModel> cTravel(this IEnumerable<CTravelViewModel> source,string reason,int applyNumber,int employeeId, decimal amont)
        //{
        //    return source.Where((x) => (string.IsNullOrEmpty(reason) || x.CReason.Contains(reason)) &&
        //                            (applyNumber.ToString().Contains(applyNumber.ToString()) || x.CApplyNumber.ToString().Contains(applyNumber.ToString())) &&
        //                            //(employeeId.ToString().Contains(employeeId.ToString())) || x.CEmployeeId.ToString().Contains(employeeId.ToString())) &&
        //                            (amont == null || amont.ToString().Contains(amont.ToString())  ));
        //}
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
