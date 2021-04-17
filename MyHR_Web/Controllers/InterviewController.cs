﻿using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class InterviewController : Controller
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
                TInterView I = myHR.TInterViews.FirstOrDefault(n => n.CInterVieweeId == Id);
                if (I != null)
                {
                    return PartialView("Edit", new CInterviewListViewModel(I));
                }
            }
            return RedirectToAction("List");
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
                    table.CInterViewStatusId = I.CInterViewStatusId;
                    myHR.SaveChanges();
                }
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
                    (x, d) => new { X = x, D = d }
                    ).Select(s => new CInterviewDetailsViewModel
                    {
                        processId = s.X.P.CInterViewProcessId,
                        name = s.X.I.CInterVieweeName,
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
                ViewBag.id = id;
                return PartialView("ProcessCreate", table);
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult ProcessCreate(TInterViewProcess a)
        {
            try
            {
                if (a != null)
                {
                    a.CProcessTime = DateTime.Now.ToString();
                    a.CInterViewProcess = a.CInterViewProcess.Replace(System.Environment.NewLine, "<br/>");
                    myHR.TInterViewProcesses.Add(a);
                    myHR.SaveChanges();
                }
                return RedirectToAction("List");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
