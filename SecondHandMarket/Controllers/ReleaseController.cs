using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecondHandMarket.Models;
using SecondHandMarket.ViewModels;

namespace SecondHandMarket.Controllers
{
    public class ReleaseController : BaseController
    {
        //
        // GET: /Release/

        public ActionResult Index()
        {
            using (Db)
            {
                ViewBag.Categories = Db.Categories.Where(c => c.ParentCategory == null)
                    .ToList();
            }
            return View();
        }

        public ActionResult Detail(int categoryId)
        {
            using (Db)
            {
                ViewBag.Category = Db.Categories.Include("ParentCategory.ParentCategory")
                                     .First(c => c.Id == categoryId);
                ViewBag.Campuses = Db.Addresses.Where(a => a.ParentAddress == null).ToList();
            }
            return View();
        }

        public ActionResult SecondStep(int categoryId)
        {
            using (Db)
            {
                ViewBag.Categories = Db.Categories
                    .Include("SubCategories")
                    .Where(c => c.ParentCategory != null
                        && c.ParentCategory.Id == categoryId)
                    .ToList();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Add(ReleaseAddModel release)
        {
            List<Picture> pictures = null;
            if (Request.Files.Count > 0)
            {
                pictures = SaveFiles(Request.Files);
            }

            using (Db)
            {
                var model = new Release()
                    {
                        UserName = User.Identity.Name,
                        Category = Db.Categories.First(c=>c.Id == release.CategoryId),
                        Title = release.Title,
                        Price = release.Price,
                        Description = release.Description,
                        Pictures = pictures,
                        TradePlace = Db.Addresses.First(a=>a.Id == release.AddressId),
                        Mobile = release.Mobile,
                        Linkman = release.Linkman,
                        QQ = release.QQ,
                        ReleaseTime = DateTime.Now
                    };

                Db.Releases.Add(model);
            }
            
            return null;
        }

        private List<Picture> SaveFiles(HttpFileCollectionBase files)
        {
            //var fileDir = Server.MapPath("~/Asset/");
            //foreach (HttpPostedFile file in files)
            //{
            //    file.SaveAs();
            //}
            return null;
        }
    }
}
