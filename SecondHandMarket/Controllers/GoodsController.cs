using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecondHandMarket.ViewModels;

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
            using (Db)
            {
                var release = Db
                    .Releases
                    .Include("Category.ParentCategory.ParentCategory")
                    .Include("TradePlace.ParentAddress")
                    .Include("Pictures")
                    .First(r => r.Id == id);

                var userName = release.UserName;
                ViewBag.User = Db.UserInfo.Where(u => u.UserName == userName);

                ViewBag.LatestRelease = Db.Releases.OrderByDescending(r => r.Id)
                    .Where(r => r.Pictures.Count > 0)
                    .Take(3).ToList()
                  .Select(r => new ListItemReleaseModel(r))
                  .ToList();

                return View(release);
            }
        }

    }
}
