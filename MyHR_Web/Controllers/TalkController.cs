using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyHR_Web.Models;
using prjCoreDemo.ViewModel;
using System.IO;
using System.Linq;

namespace MyHR_Web.Controllers
{
    public class TalkController : FilterController
    {
        public IActionResult Index() //全部人聊天
        {
            return View();
        }
      
    }
}
