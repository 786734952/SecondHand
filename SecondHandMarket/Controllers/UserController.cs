using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandMarket.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Buy()
        {
            return View();
        }
        public ActionResult Release()
        {
            return View();
        }
        public ActionResult Collect()
        {
            return View();
        }
    }
}
