using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult Detail(int categoryId)
        {
            using (Db)
            {
                ViewBag.Category = Db.Categories.Include("ParentCategory.ParentCategory")
                    .First(c => c.Id == categoryId);
            }
            return View();
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
                    .Where(c =>c.ParentCategory !=null && c.ParentCategory.Id ==categoryId)
                    .ToList();
            }
            return View();
        }
    }
}
