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

        public ActionResult List()
        {
            using (Db)
            {
                var categories = Db.Categories.Where(c => c.ParentCategory == null).ToList();
                var models = categories.Select(c => new IndexCategoryViewModel(c)).ToList();
                return View(models);
            }
        }
        public ActionResult SubList(int pid)
        {
            return View();
        }
    }
}
