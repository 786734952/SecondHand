using SecondHandMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandMarket.Controllers
{
    public class BaseController : Controller
    {
        private MarketContext _db;

        public MarketContext Db
        {
            get
            {
                if (_db == null)
                {
                    _db = new MarketContext();
                }
                return _db;
            }
        }
        
    }
}
