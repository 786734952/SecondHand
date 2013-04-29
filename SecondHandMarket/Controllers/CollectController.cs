using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecondHandMarket.Models;

namespace SecondHandMarket.Controllers
{
    public class CollectController : BaseController
    {
        //
        // GET: /Collect/

        [HttpPost]
        public ActionResult Release(int id)
        {
            using (Db)
            {
                var userName = User.Identity.Name;
                var release = Db.Releases.Find(id);

                if (!Db.ReleaseCollects
                       .Any(c => c.Release.Id == id && c.UserName == userName))
                {
                    var collect = new ReleaseCollect()
                        {
                            CreateTime = DateTime.Now,
                            Release = release,
                            UserName = userName
                        };

                    Db.ReleaseCollects.Add(collect);
                    Db.SaveChanges();
                    TempData["SuccessMsg"] = "收藏成功";
                }
            }

            return RedirectToAction("Detail", "Goods", new
                {
                    id
                });
        }

        [HttpPost]
        public ActionResult Buy(int id)
        {
            using (Db)
            {
                var userName = User.Identity.Name;
                var buy = Db.Buys.Find(id);

                if (!Db.BuyCollects
                       .Any(b => b.Buy.Id == id && b.UserName == userName))
                {
                    var collect = new BuyCollect()
                        {
                            CreateTime = DateTime.Now,
                            Buy = buy,
                            UserName = userName
                        };

                    Db.BuyCollects.Add(collect);
                    Db.SaveChanges();
                    TempData["SuccessMsg"] = "收藏成功";
                }

                return RedirectToAction("Detail", "Buy", new
                    {
                        id
                    });
            }
        }

        [HttpPost]
        public ActionResult Delete(List<string> collectId)
        {
            var releaseCollectIds = new List<int>();
            var buyCollectIds = new List<int>();

            for (int i = 0; i < collectId.Count; i++)
            {
                var temp = collectId[i].Split(new[] {"||"},
                                              StringSplitOptions.RemoveEmptyEntries);
                var id = int.Parse(temp[0]);
                var type = int.Parse(temp[1]);

                if (type == 0)
                {
                    releaseCollectIds.Add(id);
                }
                else
                {
                    buyCollectIds.Add(id);
                }
            }

            using (Db)
            {
                var releaseCollects = Db.ReleaseCollects
                    .Where(r => releaseCollectIds.Contains(r.Id))
                    .ToList();
                releaseCollects.ForEach(r => Db.ReleaseCollects.Remove(r));

                var buyCollects = Db.BuyCollects
                                    .Where(b => buyCollectIds.Contains(b.Id))
                                    .ToList();
                buyCollects.ForEach(b => Db.BuyCollects.Remove(b));

                Db.SaveChanges();

                TempData["SuccessMsg"] = string
                    .Format("成功删除了{0}条记录",
                            releaseCollects.Count + buyCollects.Count);
                return RedirectToAction("Collect", "User");
            }
        }

        [HttpPost]
        public ActionResult UnBuy(int id)
        {
            using (Db)
            {
                var collect = Db.BuyCollects.Include("Buy").First(b => b.Id == id);
                var buyId = collect.Buy.Id;
                Db.BuyCollects.Remove(collect);
                Db.SaveChanges();
                TempData["SuccessMsg"] = "取消了1条收藏";
                return RedirectToAction("Detail", "Buy", new
                {
                    id = buyId
                });
            }
        }

        [HttpPost]
        public ActionResult UnRelease(int id)
        {
            using (Db)
            {
                var collect = Db.ReleaseCollects.Include("Release").First(r => r.Id == id);
                var releaseId = collect.Release.Id;
                Db.ReleaseCollects.Remove(collect);
                Db.SaveChanges();
                TempData["SuccessMsg"] = "取消了1条收藏";
                return RedirectToAction("Detail", "Release", new
                {
                    id = releaseId
                });
            }
        }
    }
}
