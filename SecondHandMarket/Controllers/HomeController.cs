using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecondHandMarket.Models;
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

                //获取最新发布

                ViewBag.LatestRelease = Db.Releases.OrderByDescending(r => r.Id)
                    .Where(r => r.Pictures.Count > 0)
                    .Take(3).ToList()
                  .Select(r => new ListItemReleaseModel(r))
                  .ToList();

                var categories = Db.Categories.Where(c => c.ParentCategory == null).ToList();

                var releaseOfCategory = new Dictionary<Category, List<ListItemReleaseModel>>();
                foreach (var category in categories)
                {
                    var categoryId = category.Id;
                    var releases = Db.Releases
                        .Where(r => r.Category.ParentCategory.ParentCategory.Id == categoryId
                        && r.Pictures.Count > 0)
                        .OrderByDescending(r => r.Id)
                        .Take(3)
                        .ToList()
                        .Select(r => new ListItemReleaseModel(r))
                        .ToList();
                    releaseOfCategory.Add(category, releases);
                }

                ViewBag.ReleaseOfCategory = releaseOfCategory;
            }
            return View();
        }

    }
}
