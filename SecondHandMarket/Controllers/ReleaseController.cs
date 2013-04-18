using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
            Release model;
            using (Db)
            {
                var categoryId = release.CategoryId;
                var addrId = release.AddressId;
                model = new Release
                    {
                        UserName = User.Identity.Name,
                        Title = release.Title,
                        Price = release.Price,
                        Category = Db.Categories.First(c => c.Id == categoryId),
                        Description = release.Description,
                        Mobile = release.Mobile,
                        Linkman = release.Linkman,
                        TradePlace = Db.Addresses.First(a => a.Id == addrId),
                        QQ = release.QQ,
                        ReleaseTime = DateTime.Now
                    };

                Db.Releases.Add(model);
                Db.SaveChanges();
            }

            if (Request.Files.Count > 0)
            {
                var pictures = SaveFiles(Request.Files, model);

                using (Db)
                {
                    Db.Entry(model).State = EntityState.Modified;
                    model.Pictures = pictures;
                    Db.SaveChanges();
                }
            }

            return View("Ok", model);
        }

        private List<Picture> SaveFiles(HttpFileCollectionBase files, Release model)
        {
            var fileDir = "~/Asset/" + model.Id + "/";
            var absFileDir = Server.MapPath(fileDir);

            if (!Directory.Exists(absFileDir))
            {
                Directory.CreateDirectory(absFileDir);
            }

            var pictures = new List<Picture>();

            foreach (string name in files)
            {
                var file = files[name];
                var absFileName = Path.Combine(absFileDir, file.FileName);
                file.SaveAs(absFileName);
                var fileName = Path.Combine(fileDir, file.FileName);
                var picture = new Picture()
                    {
                        Path = fileName
                    };
                pictures.Add(picture);
            }
            return pictures;
        }
    }
}
