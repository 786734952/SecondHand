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
                    ViewBag.ParentCategory = Db.Categories.First(f => f.Id == pId);
                }

                var categories = query.ToList();
                var models = categories.Select(c => new IndexCategoryViewModel(c)).ToList();
                return View(models);
            }
        }

        [HttpPost]
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
        public ActionResult Delete(int categoryId)
        {
            using (Db)
            {
                var category = Db.Categories.First(c => c.Id == categoryId);
                var parentCategory = category.ParentCategory;

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
    }
}
