using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using MyHR_Web.Models;
using Newtonsoft.Json;
using MyHR_Web.MyClass;
using prjCoreDemo.ViewModel;
using MyHR_Web.ViewModel;

namespace MyHR_Web.Controllers
{
    
    public class ServiceController : Controller
    {
        private IWebHostEnvironment iv_host;
        public ServiceController(IWebHostEnvironment p)
        {
            iv_host = p;
        }

        //public IActionResult Excel<T>(List<T> data)

        public IActionResult Excel()
        {
            //string sWebRootFolder = iv_host.WebRootPath + @"\ExcelFile\";
            //string sFileName = $"User_{DateTime.Now.ToString("yyyyMMddHHssfff")}.xlsx";
            dbMyCompanyContext myHR = new dbMyCompanyContext();
            //select 要轉成 excel 的 model 資料
            var data = myHR.TInterViewProcesses
                    .Join(
                    myHR.TInterViews.Where(n => n.CInterVieweeId == 6),
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
                        name = s.X.I.CInterVieweeName,
                        process = s.X.P.CInterViewProcess,
                        departname = s.D.CDepartment,
                    }).ToList();

            Excel excelObj = new Excel();
            var xlsx = excelObj.Export(data);//轉換 model 的欄位跟資料
            //xlsx.SaveAs(Path.Combine(sWebRootFolder, sFileName));//存在特定路徑用            
            MemoryStream excelStream = new MemoryStream();
            xlsx.SaveAs(excelStream);
            excelStream.Position = 0;
            string fileName = "Report.xlsx";

            return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        public IActionResult ExcelInterview()
        {
            //string sWebRootFolder = iv_host.WebRootPath + @"\ExcelFile\";
            //string sFileName = $"User_{DateTime.Now.ToString("yyyyMMddHHssfff")}.xlsx";
            dbMyCompanyContext myHR = new dbMyCompanyContext();
            //select 要轉成 excel 的 model 資料
            var table = myHR.TInterViews;
            List<CInterviewListViewModel> data = new List<CInterviewListViewModel>();
            foreach (TInterView i in table)
                data.Add(new CInterviewListViewModel(i));

            Excel excelObj = new Excel();
            var xlsx = excelObj.Export(data);//轉換 model 的欄位跟資料
            //xlsx.SaveAs(Path.Combine(sWebRootFolder, sFileName));//存在特定路徑用            
            MemoryStream excelStream = new MemoryStream();
            xlsx.SaveAs(excelStream);
            excelStream.Position = 0;
            string fileName = "面試資料清單.xlsx";

            return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
