using Microsoft.AspNetCore.Mvc;

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
