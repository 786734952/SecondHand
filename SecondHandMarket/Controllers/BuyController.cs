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

        public ActionResult Index(int? firstCategoryId, int? secondCategoryId,
            int? thirdCategoryId, PageBarModel pageBar)
        {
            using (Db)
            {
                if (pageBar.PageSize == 0)
                {
                    pageBar.PageIndex = 1;
                    pageBar.PageSize = 15;
                }

                var firstLvlCategories = GetSubCategories(null);
                firstLvlCategories.Insert(0, new SelectListItem());
                ViewBag.FirstLvlCategorise = firstLvlCategories;
                ViewBag.SecondLvlCategories = new List<SelectListItem>();
                ViewBag.ThirdLvlCategories = new List<SelectListItem>();

                var buys = Db.Buys.AsQueryable();
                if (thirdCategoryId != null)
                {
                    var categoryId = thirdCategoryId.Value;
                    buys = buys.Where(b => b.Category.Id == categoryId);
                }
                else if (secondCategoryId != null)
                {
                    var categoryId = secondCategoryId.Value;
                    buys = buys.Where(b => b.Category.ParentCategory.Id == categoryId);
                }
                else if (firstCategoryId != null)
                {
                    var categoryId = firstCategoryId.Value;
                    buys = buys.Where(b => b.Category.ParentCategory.ParentCategory.Id == categoryId);
                }

                if (secondCategoryId != null)
                {
                    var thirdLvlCategories = GetSubCategories(secondCategoryId.Value);
                    thirdLvlCategories.Insert(0, new SelectListItem());
                    ViewBag.ThirdLvlCategories = thirdLvlCategories;
                }
                if (firstCategoryId != null)
                {
                    var secondLvlCategories = GetSubCategories(firstCategoryId.Value);
                    secondLvlCategories.Insert(0, new SelectListItem());
                    ViewBag.SecondLvlCategories = secondLvlCategories;
                }

                var model = buys
                    .OrderByDescending(b => b.Id)
                    .Skip(pageBar.SkipCount).Take(pageBar.PageSize)
                    .ToList()
                    .Select(b => new BuyListItemModel(b))
                    .ToList();

                ViewBag.PageBarModel = new PageBarModel()
                {
                    PageIndex = pageBar.PageIndex,
                    PageSize = pageBar.PageSize,
                    Total = buys.Count(),
                    Url = Request.Url.ToString()
                };

                return View(model);
            }
        }

        private List<SelectListItem> GetSubCategories(int? categoryId)
        {
            if (categoryId == null)
            {
                return Db.Categories.Where(c => c.ParentCategory == null)
                  .ToList().Select(c => new SelectListItem()
                      {
                          Text = c.Name,
                          Value = c.Id.ToString()
                      }).ToList();
            }
            else
            {
                var id = categoryId.Value;
                var category = Db.Categories.Include("SubCategories")
                    .First(c => c.Id == id);

                var categories = category.SubCategories;

                return categories.Select(c => new SelectListItem()
                    {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                        Selected = c.Id == id
                    }).ToList();
            }
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

        public ActionResult Edit(int id)
        {
            using (Db)
            {
                var buy = Db.Buys
                    .Include("Category")
                    .Include("Place")
                    .First(b => b.Id == id);
                var model = new BuyEditModel(buy);

                return View(model.Prepare(Db));
            }
        }

        [HttpPost]
        [IgnoreModelErrors("Category.*")]
        public ActionResult Edit(BuyEditModel buy)
        {
            if (ModelState.IsValid)
            {
                using (Db)
                {
                    var model = buy.Update(Db);
                    Db.SaveChanges();
                }
                return Content("修改成功");
            }
            return Content("修改失败");
        }

        public ActionResult List()
        {
            return View();
        }
    }
}
