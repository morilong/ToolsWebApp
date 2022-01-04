using Microsoft.AspNetCore.Mvc;

namespace Haooyou.Tool.Web.Controllers
{
    public class JsonViewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
