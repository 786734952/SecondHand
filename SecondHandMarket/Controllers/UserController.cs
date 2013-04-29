using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SecondHandMarket.Attribute;
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
        public ActionResult Collect(PageBarModel pageBar)
        {
            if (pageBar.PageSize == 0)
            {
                pageBar.PageIndex = 1;
                pageBar.PageSize = 15;
            }

            IQueryable<ReleaseCollect> releaseCollects;
            IQueryable<BuyCollect> buyCollects;
            using (Db)
            {
                var userName = User.Identity.Name;
                releaseCollects = Db.ReleaseCollects.Include("Release").Where(r => r.UserName == userName);
                buyCollects = Db.BuyCollects.Include("Buy").Where(b => b.UserName == userName);

                var allCollects = releaseCollects
                    .ToList()
                    .Select(r => new CollectListItemModel(r))
                    .Union(buyCollects.ToList().Select(b => new CollectListItemModel(b)))
                    .ToList();

                var models = allCollects
                    .OrderByDescending(c => c.CollectTime)
                    .Skip(pageBar.SkipCount)
                    .Take(pageBar.PageSize)
                    .ToList();

                ViewBag.PageBarModel = new PageBarModel()
                {
                    PageIndex = pageBar.PageIndex,
                    PageSize = pageBar.PageSize,
                    Total = allCollects.Count(),
                    Url = Url.Action("Collect")
                };
                return View(models);
            }
        }
        public ActionResult Evaluate(string userName)
        {
            using (Db)
            {
                var user = Db.UserInfo
                    .Include("Authentication")
                    .FirstOrDefault(u => u.UserName == userName);

                var membershipUser = Membership.GetUser(userName);

                var reputations = Db.Reputations
                                    .Include("FromUser")
                                    .Where(r => r.ToUser == userName)
                                    .ToList();

                var currUserName = User.Identity.Name;
                var currUser = Db.UserInfo
                                 .Include("Authentication")
                                 .FirstOrDefault(u => u.UserName == currUserName);

                var model = new EvaluateIndexModel()
                    {
                        Reputations = reputations,
                        User = membershipUser,
                        UserInfo = user,
                        CurrentUser = currUser
                    };

                return View(model);
            }
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
            using (Db)
            {
                var userName = User.Identity.Name;
                var user = Db.UserInfo.Include("Authentication").FirstOrDefault(u => u.UserName == userName);
                if (user == null)
                {
                    ViewBag.HasUserInfo = false;
                    return View();
                }
                else
                {
                    if (user.Authentication == null)
                    {
                        user.Authentication = new UserAuthentication();
                        Db.SaveChanges();
                    }
                    else if (user.Authentication.IsAccepted)
                    {
                        ViewBag.IsAccepted = true;
                        return View();
                    }
                    return View(new AuthenticateAddModel(user.Authentication));
                }
            }
        }

        [HttpPost]
        [IgnoreModelErrors("IDCard1.*,IDCard2.*,StudentCard.*")]
        public ActionResult Authenticate(AuthenticateAddModel authenticate)
        {
            if (ModelState.IsValid)
            {
                var isValid = true;
                var id = authenticate.Id;
                var model = Db.Authentications
                    .First(a => a.Id == id);
                if (Request.Files["StudentCardPath"].ContentLength == 0
                    && model.StudentCardPath == null)
                {
                    ModelState.AddModelError("StudentCardPath", "请上传学生证照片");
                    isValid = false;
                }
                if (Request.Files["IDCard1Path"].ContentLength == 0
                    && model.IDCard1Path == null)
                {
                    ModelState.AddModelError("IDCard1Path", "请上传身份证正面照片");
                    isValid = false;
                }
                if (Request.Files["IDCard2Path"].ContentLength == 0
                    && model.IDCard2Path == null)
                {
                    ModelState.AddModelError("IDCard2Path", "请上传身份证背面照片");
                    isValid = false;
                }

                if (isValid)
                {
                    model.StudentNo = authenticate.StudentNo;
                    model.IDCardNo = authenticate.IDCardNo;

                    var picPath = SavePicture(Request.Files["StudentCardPath"], "学生证");
                    if (picPath != null)
                    {
                        model.StudentCardPath = picPath;
                    }

                    picPath = SavePicture(Request.Files["IDCard1Path"], "身份证正面");
                    if (picPath != null)
                    {
                        model.IDCard1Path = picPath;
                    }

                    picPath = SavePicture(Request.Files["IDCard2Path"], "身份证背面");
                    if (picPath != null)
                    {
                        model.IDCard2Path = picPath;
                    }
                    Db.SaveChanges();

                    return View(new AuthenticateAddModel(model));
                }
            }
            return View(authenticate);
        }

        private string SavePicture(HttpPostedFileBase httpPostedFileBase, string fileName)
        {
            if (httpPostedFileBase.ContentLength > 0)
            {
                var userName = User.Identity.Name;

                var dir = string.Format("~/Asset/Authentication/{0}/", userName);
                var absDir = Server.MapPath(dir);

                if (!Directory.Exists(absDir))
                {
                    Directory.CreateDirectory(absDir);
                }

                fileName = fileName + Path.GetExtension(httpPostedFileBase.FileName);
                var filePath = Path.Combine(dir, fileName);
                httpPostedFileBase.SaveAs(Server.MapPath(filePath));

                return filePath;
            }
            return null;
        }
    }
}
