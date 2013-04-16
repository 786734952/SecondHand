using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecondHandMarket.ViewModels;

namespace SecondHandMarket.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            using (Db)
            {
                ViewBag.FirstLevelCategories = Db.Categories
                    .Include("ParentCategory")
                    .Include("SubCategories")
                    .Include("SubCategories.SubCategories")
                    .Where(c => c.ParentCategory == null)
                                                 .ToList();
            }
            return View();
        }

    }
}
