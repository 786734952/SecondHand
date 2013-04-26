using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecondHandMarket.Models;
using SecondHandMarket.ViewModels;

namespace SecondHandMarket.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            using (Db)
            {
                var userName = User.Identity.Name;

                var user = Db.UserInfo.Include("Authentication").FirstOrDefault(u => u.UserName == userName);

                var model = new UserDetailModel(user, userName);
                return View(model.Prepare(Db));
            }
        }
        public ActionResult Buy(PageBarModel pageBar)
        {
            using (Db)
            {
                if (pageBar.PageSize == 0)
                {
                    pageBar.PageIndex = 1;
                    pageBar.PageSize = 15;
                }
                var userName = User.Identity.Name;
                var myBuys = Db.Buys.Where(b => b.UserName == userName);

                var pageData = myBuys.OrderByDescending(b => b.Id)
                                     .Skip(pageBar.SkipCount)
                                     .Take(pageBar.PageSize)
                                     .ToList()
                                     .Select(b => new BuyListItemModel(b))
                                     .ToList();

                ViewBag.PageBarModel = new PageBarModel()
                    {
                        PageIndex = pageBar.PageIndex,
                        PageSize = pageBar.PageSize,
                        Total = myBuys.Count(),
                        Url = Url.Action("Buy")
                    };

                return View(pageData);
            }
        }

        [HttpPost]
        public ActionResult DeleteBuys(List<int> buysId)
        {
            using (Db)
            {
                var buys = Db.Buys.Where(b => buysId.Contains(b.Id)).ToList();
                foreach (var buy in buys)
                {
                    Db.Buys.Remove(buy);
                }
                Db.SaveChanges();

                TempData["SuccessMsg"] = string.Format("成功删除了{0}条记录", buys.Count);

                return RedirectToAction("Buy", new
                    {
                        PageIndex = Request["PageIndex"],
                        PageSize = Request["PageSize"]
                    });
            }
        }

        public ActionResult Release(PageBarModel pageBar)
        {
            using (Db)
            {
                if (pageBar.PageSize == 0)
                {
                    pageBar.PageIndex = 1;
                    pageBar.PageSize = 15;
                }

                var userName = User.Identity.Name;
                var myReleases = Db.Releases.Where(r => r.UserName == userName);

                var pageData = myReleases.OrderByDescending(r => r.Id)
                                         .Skip(pageBar.SkipCount)
                                         .Take(pageBar.PageSize)
                                         .ToList()
                                         .Select(r => new ListItemReleaseModel(r))
                                         .ToList();

                ViewBag.MyReleases = pageData;
                ViewBag.PageBarModel = new PageBarModel()
                    {
                        PageIndex = pageBar.PageIndex,
                        PageSize = pageBar.PageSize,
                        Total = myReleases.Count(),
                        Url = Url.Action("Release")
                    };
                return View();
            }
        }
        public ActionResult Collect()
        {
            return View();
        }
        public ActionResult Evaluate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteRelease(List<int> releaseIds)
        {
            using (Db)
            {
                var releases = Db.Releases.Where(r => releaseIds.Contains(r.Id)).ToList();
                foreach (var release in releases)
                {
                    Db.Releases.Remove(release);
                }
                Db.SaveChanges();
                TempData["SuccessMsg"] = string.Format("成功删除{0}条记录", releases.Count);
                return RedirectToAction("Release", new
                    {
                        PageIndex = Request["PageIndex"],
                        PageSize = Request["PageSize"]
                    });
            }
        }
        public ActionResult Edit()
        {
            var userName = User.Identity.Name;
            using (Db)
            {
                var user = Db.UserInfo.FirstOrDefault(u => u.UserName == userName);
                return View(new UserEditModel(user, userName));
            }
        }

        [HttpPost]
        public ActionResult Edit(UserEditModel user)
        {
            if (ModelState.IsValid)
            {
                using (Db)
                {
                    var userName = user.UserName;
                    var model = Db.UserInfo.FirstOrDefault(u => u.UserName == userName);
                    if (model == null)
                    {
                        model = new User();
                        Db.UserInfo.Add(model);
                    }
                    model.EntranceYear = user.EntranceYear;
                    model.Gender = user.Gender;
                    model.Mobile = user.Mobile;
                    model.QQ = user.QQ;
                    model.RealName = user.RealName;
                    model.UserName = user.UserName;
                    Db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        public ActionResult Authenticate()
        {
            return View();
        }
    }
}
