using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using SecondHandMarket.Models;
using SecondHandMarket.ViewModels;

namespace SecondHandMarket.Controllers
{
    public class GoodsController : BaseController
    {
        //
        // GET: /Goods/

        public ActionResult Index(PageBarModel pageBar, int? categoryId, string name)
        {
            using (Db)
            {
                ViewBag.FirstLevelCategories = Db
                    .Categories
                    .Include("ParentCategory")
                    .Include("SubCategories")
                    .Include("SubCategories.SubCategories")
                    .Where(c => c.ParentCategory == null)
                    .ToList();

                if (pageBar.PageSize == 0)
                {
                    pageBar.PageIndex = 1;
                    pageBar.PageSize = 25;
                }

                IQueryable<Release> allRelease;

                if (categoryId == null)
                {
                    allRelease = Db.Releases.AsQueryable();
                }
                else
                {
                    var cId = categoryId.Value;
                    allRelease = Db.Releases
                                   .Where(r => r.Category.Id == cId
                                               || r.Category.ParentCategory.Id == cId
                                               || r.Category.ParentCategory.ParentCategory.Id == cId);
                }

                if (!string.IsNullOrWhiteSpace(name))
                {
                    ViewBag.SearchName = name;
                    allRelease = allRelease.Where(r => r.Title.Contains(name));
                }

                var model = allRelease
                    .OrderByDescending(r => r.Id)
                    .Skip(pageBar.SkipCount).Take(pageBar.PageSize)
                                      .ToList()
                                      .Select(r => new ListItemReleaseModel(r))
                                      .ToList();

                ViewBag.PageBarModel = new PageBarModel()
                    {
                        PageIndex = pageBar.PageIndex,
                        PageSize = pageBar.PageSize,
                        Total = allRelease.Count(),
                        Url = Url.Action("Index")
                    };
                return View(model);
            }
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

                if (Request.IsAuthenticated)
                {
                    var currUserName = User.Identity.Name;
                    ViewBag.Collect = Db
                        .ReleaseCollects
                        .FirstOrDefault(c => c.Release.Id == id && c.UserName == currUserName);
                }

                return View(release);
            }
        }

    }
}
