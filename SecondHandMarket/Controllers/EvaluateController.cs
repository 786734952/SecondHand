using System.Web.Mvc;
using SecondHandMarket.ViewModels;

namespace SecondHandMarket.Controllers
{
    public class EvaluateController : BaseController
    {
        [HttpPost]
        public ActionResult Add(EvaluateAddModel evaluate)
        {
            using (Db)
            {
                var reputation = evaluate.GetReputation(Db, User.Identity.Name);
                Db.Reputations.Add(reputation);
                Db.SaveChanges();

                return RedirectToAction("Evaluate", "User",
                                        new
                                            {
                                                userName = evaluate.ToUserName
                                            });
            }
        }
    }
}