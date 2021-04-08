using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace MyHR_Web.Controllers
{
    public class ServiceController : Controller
    {
        private IWebHostEnvironment iv_host;
        
        public IActionResult Excel<T>(List<T> data)
        {

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string sWebRootFolder = iv_host.WebRootPath + @"\ExcelFile\";
            string sFileName = $"User_{DateTime.Now.ToString("yyyyMMddHHssfff")}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            using (ExcelPackage package = new ExcelPackage(file))
            {

                ExcelWorksheet ws = package.Workbook.Worksheets.Add("員工清單");
                
                int colIdx = 1;
                foreach (var item in typeof(T).GetProperties())
                {
                    ws.Cells[1, colIdx++].Value = item.Name;
                }

                int rowIdx = 2;
                foreach (var item in data)
                {
                    int conlumnIndex = 1;
                    foreach (var jtem in item.GetType().GetProperties())
                    {
                        ws.Cells[rowIdx,conlumnIndex].Value = string.Concat("'", Convert.ToString(jtem.GetValue(item, null)));
                        conlumnIndex++;
                    }
                    rowIdx++;
                }
                package.Save();
            }
            var fs = System.IO.File.OpenRead(file.ToString());
            return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);            
        }
    }
}
