using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandMarket.Controllers
{
    public class GoodsController : BaseController
    {
        //
        // GET: /Goods/

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
        public ActionResult Detail(int id)
        {
            return View();
        }

    }
}
