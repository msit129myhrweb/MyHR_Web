using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyHR_Web.Controllers
{
    public class Covid_19Controller : Controller
    {
        public readonly string RSS_URL = @"https://www.cdc.gov.tw/RSS/RssXml/Hh094B49-DRwe2RR4eFfrQ?type=1";
        public IActionResult ReadRss()
        {
            try
            {
                var webRequest = WebRequest.Create(RSS_URL);
                var reader = new StreamReader(
                    webRequest.GetResponse().GetResponseStream());
                var strContent = reader.ReadToEnd();

                return Content(strContent, "text/xml");
            }
            catch (Exception ex)
            {
                return Content("", "text/xml");
            }
        }
        public IActionResult Covid_19()
        {
            return View();
        }
    }
}
