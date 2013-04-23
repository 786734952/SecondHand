using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecondHandMarket.Attribute;
using SecondHandMarket.Models;
using SecondHandMarket.ViewModels;

namespace SecondHandMarket.Controllers
{
    public class BuyController : BaseController
    {
        //
        // GET: /Buy/

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Add(int categoryId)
        {
            using (Db)
            {
                var category = Db.Categories.Include("ParentCategory.ParentCategory")
                    .First(c => c.Id == categoryId);

                var model = new BuyAddModel()
                    {
                        CategoryId = category.Id
                    };
                model.Prepare(Db);

                return View(model);
            }
        }

        [HttpPost]
        [Authorize]
        [IgnoreModelErrors("Category.*,Address.*,FirstLvlAddr")]
        public ActionResult Add(BuyAddModel buy)
        {
            using (Db)
            {
                if (ModelState.IsValid)
                {
                    var model = buy.GetBuyModel(Db, User.Identity.Name);
                    Db.Buys.Add(model);
                    Db.SaveChanges();
                    return View("Ok");
                }
                return View(buy.Prepare(Db));
            }
        }

        public ActionResult FirstStep()
        {
            using (Db)
            {
                ViewBag.Categories = Db.Categories.Where(c => c.ParentCategory == null).ToList();
            }
            return View();
        }
        public ActionResult SecondStep(int categoryId)
        {
            using (Db)
            {
                ViewBag.Categories = Db.Categories.Include("SubCategories")
                    .Where(c => c.ParentCategory != null && c.ParentCategory.Id == categoryId)
                    .ToList();
            }
            return View();
        }
    }
}
