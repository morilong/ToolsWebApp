using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Haooyou.Tool.WebApp.Controllers
{
    public class homeController : Controller
    {
        //
        // GET: /home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestUtf8()
        {
            return Json(new { a = 1, b = 2 }, "text/html;charset=utf8", JsonRequestBehavior.AllowGet);
        }


    }
}
