using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecondHandMarket.Models;
using SecondHandMarket.ViewModels;

namespace SecondHandMarket.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AuthenticateController : BaseController
    {
        //
        // GET: /Authenticate/

        public ActionResult List(bool? includeAll, PageBarModel pageBar)
        {
            using (Db)
            {
                IQueryable<UserAuthentication> allData;
                var all = includeAll == null ? false : includeAll.Value;
                ViewBag.IncludeAll = all;
                if (!all)
                {
                    allData = Db.Authentications.Where(a => !a.IsAccepted)
                                .OrderBy(a => a.Id);
                }
                else
                {
                    allData = Db.Authentications.OrderBy(a => a.IsAccepted)
                                .ThenBy(a => a.Id);
                }

                if (pageBar.PageSize == 0)
                {
                    pageBar.PageIndex = 1;
                    pageBar.PageSize = 15;
                }

                var model = allData.Skip(pageBar.SkipCount).Take(pageBar.PageSize)
                                   .ToList()
                                   .Select(a => new AuthenticateDetailModel(a).Prepare(Db))
                                   .ToList();

                ViewBag.PageBarModel = new PageBarModel()
                {
                    PageIndex = pageBar.PageIndex,
                    PageSize = pageBar.PageSize,
                    Total = allData.Count(),
                    Url = Url.Action("List") + "?includeAll=" + includeAll.ToString().ToLower()
                };

                return View(model);
            }
        }

        public ActionResult Accept(int id)
        {
            using (Db)
            {

                var authenticate = Db.Authentications.First(a => a.Id == id);
                authenticate.IsAccepted = true;
                authenticate.RejectReason = "";
                Db.SaveChanges();

                TempData["SuccessMsg"] = "操作成功";
                return RedirectToAction("List", new
                    {
                        PageIndex = Request["PageIndex"],
                        PageSize = Request["PageSize"],
                        includeAll = Request["includeAll"]
                    });
            }
        }

        public ActionResult Reject(int id, string msg)
        {
            using (Db)
            {
                var authenticate = Db.Authentications.First(a => a.Id == id);
                authenticate.IsAccepted = false;
                authenticate.RejectReason = msg;
                Db.SaveChanges();

                TempData["SuccessMsg"] = "操作成功";
                return RedirectToAction("List", new
                    {
                        PageIndex = Request["PageIndex"],
                        PageSize = Request["PageSize"],
                        includeAll = Request["includeAll"]
                    });
            }
        }
    }
}
