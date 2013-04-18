using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecondHandMarket.Models;
using SecondHandMarket.ViewModels;

namespace SecondHandMarket.Controllers
{
    public class AddressController : BaseController
    {
        //
        // GET: /Address/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List(int? addressId)
        {
            using (Db)
            {
                if (addressId == null)
                {
                    ViewBag.Addresses = Db.Addresses
                        .Include("SubAddresses")
                        .Where(a => a.ParentAddress == null)
                                          .ToList();
                }
                else
                {
                    var pId = addressId.Value;
                    ViewBag.Addresses = Db.Addresses
                        .Include("SubAddresses")
                                          .Where(a => a.ParentAddress != null && a.ParentAddress.Id == pId)
                                          .ToList();
                    ViewBag.ParentAddress = Db.Addresses.Include("ParentAddress").First(a => a.Id == pId);
                }

                return View();
            }
        }

        [HttpPost]
        public ActionResult Add(string address, int? parentId)
        {
            using (Db)
            {
                var subAddress = new Address()
                    {
                        Name = address
                    };

                if (parentId == null)
                {
                    Db.Addresses.Add(subAddress);
                }
                else
                {
                    var pId = parentId.Value;
                    Db.Addresses.First(a => a.Id == pId).SubAddresses.Add(subAddress);
                }

                Db.SaveChanges();

                return RedirectToAction("List", new { addressId = parentId });
            }
        }

        [HttpPost]
        public ActionResult Delete(int addressId)
        {
            using (Db)
            {
                var address = Db.Addresses.Include("SubAddresses").First(a => a.Id == addressId);
                var parentAddress = address.ParentAddress;
                var subAddresses = address.SubAddresses.ToList();
                for (int i = 0; i < subAddresses.Count; i++)
                {
                    Db.Addresses.Remove(subAddresses[i]);
                }
                Db.Addresses.Remove(address);
                Db.SaveChanges();

                if (parentAddress == null)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    return RedirectToAction("List", new { addressId = parentAddress.Id });
                }
            }
        }

        [HttpGet]
        public ActionResult GetSubAddresses(int addressId)
        {
            using (Db)
            {
                var addr = Db.Addresses.Include("SubAddresses").First(a => a.Id == addressId);

                var addresses = addr.SubAddresses.Select(s => new IndexAddressModel(s))
                                    .ToList();

                return Json(addresses, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
