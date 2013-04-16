using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            throw new NotImplementedException();
        }
            
    }
}
