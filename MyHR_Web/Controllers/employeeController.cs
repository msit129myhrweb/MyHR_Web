using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using MyHR_Web.MyClass;
using MyHR_Web.ViewModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using prjCoreDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class employeeController : Controller
    {
        private dbMyCompanyContext db = new dbMyCompanyContext();
        private IHostingEnvironment iv_host;
        public employeeController(IHostingEnvironment p)
        {
            iv_host = p;
        }
        public IActionResult register()
        {
            ViewBag.CDepartmentId = db.TUserDepartments.ToList();
            ViewBag.CJobTitleId = db.TUserJobTitles.ToList();
            ViewBag.COnBoardStatusId = db.TUserOnBoardStatuses.ToList();
           
            return View();
        }


        [HttpPost]
        public IActionResult register(TUserViewModel _user)
        {
           
            //新增員工
            db.TUsers.Add(_user.tuserVM);
            db.SaveChanges();

            //開始寄信
            int userid = db.TUsers.OrderByDescending(n => n.CEmployeeId).Select(c => c.CEmployeeId).FirstOrDefault(); //撈取新註冊的員工ID
            string name = db.TUsers.Where(n => n.CEmployeeId == userid).Select(c => c.CEmployeeName).FirstOrDefault(); //撈取新註冊的員工NAME
            string email = db.TUsers.Where(n => n.CEmployeeId==userid).Select(c => c.CEmail).FirstOrDefault(); //撈取新註冊的員工EMAIL
            string password = db.TUsers.Where(n => n.CEmployeeId == userid).Select(c => c.CPassWord).FirstOrDefault(); //撈取新註冊的員工PASSWORD
            try
            {
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
               
                
                msg.To.Add(email);

                msg.From = new MailAddress("msit129hellowork@gmail.com", "HELLOWORK公司", System.Text.Encoding.UTF8);
                /* 上面3個參數分別是發件人地址，發件人姓名，編碼*/
                msg.Subject = "錄取報到通知";//郵件標題
                msg.SubjectEncoding = System.Text.Encoding.UTF8;//郵件標題編碼
                                                                //郵件內容    
                msg.Body = name + "  您好，" + Environment.NewLine;
                msg.Body += "您已錄取本公司，請至公司官網報到啟用帳號。"+ Environment.NewLine;
                msg.Body += "帳號:" + userid + Environment.NewLine;
                msg.Body += "密碼:" + password + Environment.NewLine;
                //郵件內容
                msg.BodyEncoding = System.Text.Encoding.UTF8;//郵件內容編碼 
                /*     msg.Attachments.Add(new Attachment(@"D:\test2.docx")); */ //附件
                msg.IsBodyHtml = true;//是否是HTML郵件 
                                      //msg.Priority = MailPriority.High;//郵件優先級 

                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("msit129hellowork@gmail.com", "izougqdehrjrufoh"); //這裡要填正確的帳號跟密碼
                client.Host = "smtp.gmail.com"; //設定smtp Server
                client.Port = 25; //設定Port
                client.EnableSsl = true; //gmail預設開啟驗證
                client.Send(msg); //寄出信件
                client.Dispose();
                msg.Dispose();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return RedirectToAction("employeeList");
        }

      
        public IActionResult employeeList()
        {

            
             var table = from r in db.TUsers
                    select r;
            List<TUserViewModel> list = new List<TUserViewModel>();
            foreach (TUser i in table)
            list.Add(new TUserViewModel(i));
            return View(list);


        }

        public JsonResult updateall(string x)
        {
            string a = x;
            string[] ids = a.Split('\\', '"', '[', ',', ']');

            List<int> list = new List<int>();
            foreach (var item in ids)
            {

                if (item != "")
                {
                    list.Add(int.Parse(item));
                }

            }
            foreach (var i in list)
            {
                
                TUser u = db.TUsers.FirstOrDefault(u => u.CEmployeeId == i);
                if (u != null)
                {
                    u.CByeByeDay = DateTime.Today;
                    u.COnBoardStatusId = 2;
                    u.CAccountEnable = 0;
                    db.SaveChanges();
                }

            }

            return Json(new { result = true, msg = "成功" });



        }


        //用URL下載檔案
        [HttpGet]
        public string Export()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string sWebRootFolder = iv_host.WebRootPath;
            string sFileName = $"User_{DateTime.Now.ToString("yyyyMMddHHssfff")}.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));


            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                var userlist = db.TUsers.ToList();
               
               
                // add a new worksheet to the empty workbook
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Employee");
                int row = 2;

                foreach (var item in userlist)
                {   //標題
                    ws.Cells[1,1].Value = "CEmployeeId";
                    ws.Cells[1,2].Value = "CEmployeeName";
                    ws.Cells[1,3].Value = "CEmployeeEnglishName";
                    ws.Cells[1,4].Value = "CPassWord";
                    ws.Cells[1,5].Value = "CGender";
                    ws.Cells[1,6].Value = "CEmail";
                    ws.Cells[1,7].Value = "CJobTitleId";
                    ws.Cells[1,8].Value = "CDepartmentId";
                    ws.Cells[1,9].Value = "CSupervisor";
                    ws.Cells[1,10].Value ="CAddress";
                    ws.Cells[1,11].Value ="CBirthday";
                    ws.Cells[1,12].Value ="CByeByeDay";
                    ws.Cells[1,13].Value ="COnBoardDay";
                    ws.Cells[1,14].Value ="CPhone";
                    ws.Cells[1,15].Value ="CEmergencyPerson";
                    ws.Cells[1,16].Value ="CEmergencyContact";
                    ws.Cells[1,17].Value ="COnBoardStatusId";
                    ws.Cells[1,18].Value ="CAccountEnable";

                    //資料
                    ws.Cells[row, 1].Value = item.CEmployeeId;
                    ws.Cells[row, 2].Value = item.CEmployeeName;
                    ws.Cells[row, 3].Value = item.CEmployeeEnglishName;
                    ws.Cells[row, 4].Value = item.CPassWord;
                    ws.Cells[row, 5].Value = item.CGender;
                    ws.Cells[row, 6].Value = item.CEmail;
                    ws.Cells[row, 7].Value = item.CJobTitleId;
                    ws.Cells[row, 8].Value = item.CDepartmentId;
                    ws.Cells[row, 9].Value = item.CSupervisor;
                    ws.Cells[row, 10].Value = item.CAddress;
                    ws.Cells[row, 11].Value = item.CBirthday;
                    ws.Cells[row, 12].Value = item.CByeByeDay;
                    ws.Cells[row, 13].Value = item.COnBoardDay;
                    ws.Cells[row, 14].Value = item.CPhone;
                    ws.Cells[row, 15].Value = item.CEmergencyPerson;
                    ws.Cells[row, 16].Value = item.CEmergencyContact;
                    ws.Cells[row, 17].Value = item.COnBoardStatusId;
                    ws.Cells[row, 18].Value = item.CAccountEnable;
                    row++;
                }
               
                package.Save(); //Save the workbook.
            }
            return URL;
            //https://stackoverflow.com/questions/47897807/export-object-to-xlsx-to-client-machine-asp-net-core
        }

        public IActionResult ExporttoExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //檔案會到wwwroot\theme
            //string sWebRootFolder = iv_host.WebRootPath;

            //檔案會到wwwroot\ExcelFile，但會報錯
            string sWebRootFolder = iv_host.WebRootPath; /*+ @"\ExcelFile\";*/
           
            string sFileName = $"User_{DateTime.Now.ToString("yyyyMMddHHssfff")}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            using (ExcelPackage package = new ExcelPackage(file))
            {
                var userlist = db.TUsers.ToList();


                // add a new worksheet to the empty workbook
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("員工清單");
                int row = 2;
               
                foreach (var item in userlist)
                {   //標題

                    //ws.Cells[1, 1] .Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 2] .Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 3] .Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 4] .Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 5] .Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 6] .Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 7] .Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 8] .Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 9] .Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 10].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 11].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 12].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 13].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 14].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 15].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 16].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 17].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    //ws.Cells[1, 18].Style.Fill.BackgroundColor.SetColor(Color.LightGray);


                    ws.Cells[1, 1].Value = "員工編號";
                    ws.Cells[1, 2].Value = "姓名";
                    ws.Cells[1, 3].Value = "英文姓名";
                    ws.Cells[1, 4].Value = "密碼";
                    ws.Cells[1, 5].Value = "性別";
                    ws.Cells[1, 6].Value = "Email"; 
                    ws.Cells[1, 7].Value = "到職日";
                    ws.Cells[1, 8].Value = "離職日";
                    ws.Cells[1, 9].Value = "地址";
                    ws.Cells[1, 10].Value = "部門";
                    ws.Cells[1, 11].Value = "職務";
                    ws.Cells[1, 12].Value = "主管";
                    ws.Cells[1, 13].Value = "生日";
                    ws.Cells[1, 14].Value = "電話";
                    ws.Cells[1, 15].Value = "緊急聯絡人";
                    ws.Cells[1, 16].Value = "緊急連絡電話";
                    ws.Cells[1, 17].Value = "到職狀態";
                    ws.Cells[1, 18].Value = "帳號狀態";

                    //資料
                    ws.Cells[row, 1].Value =  item.CEmployeeId;
                    ws.Cells[row, 2].Value =  item.CEmployeeName;
                    ws.Cells[row, 3].Value =  item.CEmployeeEnglishName;
                    ws.Cells[row, 4].Value =  item.CPassWord;
                    ws.Cells[row, 5].Value =  item.CGender;
                    ws.Cells[row, 6].Value =  item.CEmail;
                    ws.Cells[row, 7].Value =  Convert.ToString(string.Format("{0:yyyy/MM/dd}", item.COnBoardDay));
                    ws.Cells[row, 8].Value =  Convert.ToString(string.Format("{0:yyyy/MM/dd}", item.CByeByeDay));
                    ws.Cells[row, 9].Value =  item.CAddress;
                    ws.Cells[row, 10].Value = (eDepartment)item.CDepartmentId;
                    ws.Cells[row, 11].Value = (eJobTitle)item.CJobTitleId;
                    ws.Cells[row, 12].Value = item.CSupervisor;
                    ws.Cells[row, 13].Value = Convert.ToString(string.Format("{0:yyyy/MM/dd}", item.CBirthday));
                    ws.Cells[row, 14].Value = item.CPhone;
                    ws.Cells[row, 15].Value = item.CEmergencyPerson;
                    ws.Cells[row, 16].Value = item.CEmergencyContact;
                    ws.Cells[row, 17].Value = (eOnBoard)item.COnBoardStatusId;
                    ws.Cells[row, 18].Value = (eAccount)item.CAccountEnable;
                    row++;
                }

                package.Save(); //Save the workbook.
            }
            var fs = System.IO.File.OpenRead(file.ToString());
            return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        
        }


        //可以存EXCEL 但檔案是壞的
        //public IActionResult ExporttoExcel()
        //{
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    string fileName = $"User_{DateTime.Now.ToString("yyyyMMddHHssfff")}.xlsx";
        //    //Guid.NewGuid().ToString() + ".xlsx";
        //    var fileStream = new FileStream(
        //        iv_host.ContentRootPath + @"\wwwroot\ExcelFile\" + fileName,
        //        FileMode.Create);

        //    var excel = new ExcelPackage(new System.IO.FileInfo(fileStream.ToString()));
        //    var ws = excel.Workbook.Worksheets.Add("sheet5");

        //    int row = 1;

        //    var userlist = db.TUsers.ToList();
        //    foreach (var item in userlist)
        //    {
        //        ws.Cells[row, 1].Value = item.CEmployeeId.ToString();
        //        ws.Cells[row, 2].Value = item.CEmployeeName;
        //        ws.Cells[row, 3].Value = item.CEmployeeEnglishName;
        //        ws.Cells[row, 4].Value = item.CPassWord;
        //        ws.Cells[row, 5].Value = item.CGender;
        //        ws.Cells[row, 6].Value = item.CEmail;
        //        ws.Cells[row, 7].Value = item.CJobTitleId;
        //        ws.Cells[row, 8].Value = item.CDepartmentId;
        //        ws.Cells[row, 9].Value = item.CSupervisor;
        //        ws.Cells[row, 10].Value = item.CAddress;
        //        ws.Cells[row, 11].Value = item.CBirthday;
        //        ws.Cells[row, 12].Value = item.CByeByeDay;
        //        ws.Cells[row, 13].Value = item.COnBoardDay;
        //        ws.Cells[row, 14].Value = item.CPhone;
        //        ws.Cells[row, 15].Value = item.CEmergencyPerson;
        //        ws.Cells[row, 16].Value = item.CEmergencyContact;
        //        ws.Cells[row, 17].Value = item.COnBoardStatusId;
        //        ws.Cells[row, 18].Value = item.CAccountEnable;
        //        row++;
        //    }
        //    excel.Save();
        //    return RedirectToAction("employeeList");
        //}


        public IActionResult employeeEdit(int? id)
        {
            ViewBag.User = db.TUsers.ToList();

            ViewBag.CDepartmentId = db.TUserDepartments.ToList();
            ViewBag.CJobTitleId = db.TUserJobTitles.ToList();
            ViewBag.COnBoardStatusId = db.TUserOnBoardStatuses.ToList();
            ViewData[CDictionary.CURRENT_LOGINED_USERID] = HttpContext.Session.GetString(CDictionary.CURRENT_LOGINED_USERID);
            
            if (id != null)
            {
                ViewData["USERID"] = id;
                TUser u = db.TUsers.FirstOrDefault(p => p.CEmployeeId == id);

                if (u != null)
                {
                    return View(new TUserViewModel(u));
                }

            }
            return RedirectToAction("employeeList");
        }


        [HttpPost]
        public async Task<IActionResult> employeeEdit(TUser user, TUserViewModel Tuser_vm, List<IFormFile> CPhoto, int id)
        {
            //user = HttpContext.Session.GetObject<TUser>(CDictionary.Current_User);//取一個在session中的TUser物件(可抓到id)
             user.CEmployeeId = id;
            foreach (var item in CPhoto)
            {
                if (item.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await item.CopyToAsync(stream);
                        user.CPhoto = stream.ToArray();
                    }
                }

                if (user != null)
                {
                    db.Update(user);
                    db.SaveChanges();
                }
            }

                if (Tuser_vm != null)
                {

                TUser u = db.TUsers.FirstOrDefault(p => p.CEmployeeId == Tuser_vm.CEmployeeId);
                if (u != null)
                {
                    u.CEmployeeName = Tuser_vm.CEmployeeName;
                    u.CEmployeeEnglishName = Tuser_vm.CEmployeeEnglishName;
                    u.CPassWord = Tuser_vm.CPassWord;
                    u.CGender = Tuser_vm.CGender;
                    u.CEmail = Tuser_vm.CEmail;
                    u.CJobTitleId = Tuser_vm.CJobTitleId;
                    u.CDepartmentId = Tuser_vm.CDepartmentId;
                    u.CSupervisor = Tuser_vm.CSupervisor;
                    u.CAddress = Tuser_vm.CAddress;
                    u.CBirthday =  Tuser_vm.CBirthday;
                    u.CByeByeDay = Tuser_vm.CByeByeDay;
                    u.COnBoardDay = Tuser_vm.COnBoardDay;
                    u.CPhone = Tuser_vm.CPhone;
                    u.CEmergencyPerson = Tuser_vm.CEmergencyPerson;
                    u.CEmergencyContact = Tuser_vm.CEmergencyContact;
                    u.COnBoardStatusId = Tuser_vm.COnBoardStatusId;
                    u.CAccountEnable = Tuser_vm.CAccountEnable;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("employeeList");
        }


    }
}
