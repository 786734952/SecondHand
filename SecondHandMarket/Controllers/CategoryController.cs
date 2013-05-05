using SecondHandMarket.Models;
using SecondHandMarket.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandMarket.Controllers
{
    public class CategoryController : BaseController
    {
        //
        // GET: /Category/

        [Authorize(Roles = "Administrator")]
        public ActionResult List(int? categoryId)
        {
            using (Db)
            {
                IQueryable<Category> query;
                if (categoryId == null)
                {
                    query = Db.Categories.Where(c => c.ParentCategory == null);
                }
                else
                {
                    var pId = categoryId.Value;
                    query = Db.Categories.Where(c => c.ParentCategory.Id == pId);
                    ViewBag.ParentCategory = Db.Categories.Include("ParentCategory").First(f => f.Id == pId);
                }

                var categories = query.ToList();
                var models = categories.Select(c => new IndexCategoryViewModel(c)).ToList();
                return View(models);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Add(string name, int? ParentId)
        {
            using (Db)
            {
                var subCategory = new Category()
                {
                    Name = name
                };
                if (ParentId == null)
                {
                    Db.Categories.Add(subCategory);
                }
                else
                {
                    var category = Db.Categories.First(c => c.Id == ParentId);
                    category.SubCategories.Add(subCategory);
                }
                Db.SaveChanges();
                return RedirectToAction("List", new { categoryId = ParentId });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int categoryId)
        {
            using (Db)
            {
                var category = Db.Categories.Include("SubCategories").First(c => c.Id == categoryId);
                var parentCategory = category.ParentCategory;
                var subCategories = category.SubCategories.ToList();

                foreach (var subCategory in subCategories)
                {
                    Db.Categories.Remove(subCategory);
                }

                Db.Categories.Remove(category);
                Db.SaveChanges();
                if (parentCategory == null)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("List", new { categoryId = parentCategory.Id });
                }
            }
        }

        public ActionResult GetSubCategories(int id)
        {
            using (Db)
            {
                var categories = Db.Categories.Include("SubCategories")
                                   .First(c => c.Id == id)
                                   .SubCategories
                                   .Select(c => new CategoryListItemModel(c))
                                   .ToList();

                return Json(categories, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
