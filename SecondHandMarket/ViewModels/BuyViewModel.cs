﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecondHandMarket.Models;

namespace SecondHandMarket.ViewModels
{
    public class BuyAddModel
    {
        [Required]
        [DisplayName("类别")]
        public int CategoryId { get; set; }

        public Category Category { get; private set; }

        [Required]
        [DisplayName("标题")]
        public string Title { get; set; }

        [Required]
        [DisplayName("价格")]
        public decimal Price { get; set; }

        [DisplayName("描述")]
        public string Description { get; set; }

        public List<SelectListItem> FirstLvlAddr { get; private set; }

        [Required]
        [DisplayName("交易地点")]
        public int AddressId { get; set; }

        public Address Address { get; private set; }

        public List<SelectListItem> SecondLvlAddr { get; private set; }

        [Required]
        [DisplayName("联系电话")]
        public string Mobile { get; set; }

        [DisplayName("QQ")]
        public string QQ { get; set; }

        /// <summary>
        /// 填充好列表属性
        /// </summary>
        /// <param name="db"></param>
        public BuyAddModel Prepare(MarketContext db)
        {
            SecondLvlAddr = new List<SelectListItem>();
            if (AddressId != 0)
            {
                var addrId = AddressId;
                Address = db.Addresses.Include("ParentAddress.SubAddresses")
                            .First(a => a.Id == addrId);

                SecondLvlAddr = Address.ParentAddress.SubAddresses
                                       .Select(a => new SelectListItem()
                                           {
                                               Text = a.Name,
                                               Value = a.Id.ToString(),
                                               Selected = a.Id == Address.Id
                                           })
                                       .ToList();
            }

            FirstLvlAddr = db.Addresses.Where(a => a.ParentAddress == null)
                             .ToList()
                             .Select(a => new SelectListItem()
                                 {
                                     Text = a.Name,
                                     Value = a.Id.ToString(),
                                     Selected = Address != null && Address.ParentAddress.Id == a.Id
                                 })
                             .ToList();

            FirstLvlAddr.Insert(0, new SelectListItem()
                {
                    Selected = Address == null
                });

            var categoryId = CategoryId;
            Category = db.Categories.Include("ParentCategory")
                         .First(c => c.Id == categoryId);

            return this;
        }

        public Buy GetBuyModel(MarketContext db, string userName)
        {
            var categoryId = CategoryId;
            var addrId = AddressId;
            var buy = new Buy()
                {
                    CreateTime = DateTime.Now,
                    Category = db.Categories.First(c => c.Id == categoryId),
                    Description = Description,
                    Mobile = Mobile,
                    Name = Title,
                    Place = db.Addresses.First(a => a.Id == addrId),
                    Price = Price,
                    QQ = QQ,
                    UserName = userName
                };

            return buy;
        }
    }
}