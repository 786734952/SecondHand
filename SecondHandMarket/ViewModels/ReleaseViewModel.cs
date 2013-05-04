using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SecondHandMarket.Models;
using SecondHandMarket.common;
using System.IO;

namespace SecondHandMarket.ViewModels
{
    public class ListItemReleaseModel
    {
        public ListItemReleaseModel(Release release)
        {
            Id = release.Id;
            Title = release.Title;
            Price = release.Price;
            ReleaseTime = release.ReleaseTime;
            ReleaseTimeDesc = ReleaseTime.GetDateTimeDesc();
            Description = release.Description;

            if (release.Pictures.Count > 0)
            {
                ImgUrl = release.Pictures[0].Path;
                ThumbnailUrl = release.Pictures[0].ThumbnailPath;
            }
            else
            {
                ImgUrl = "~";
                ThumbnailUrl = "~";
            }
        }

        public int Id { get; private set; }
        public string Title { get; private set; }
        public decimal Price { get; private set; }
        public DateTime ReleaseTime { get; private set; }

        public string Description { get; private set; }

        /// <summary>
        /// 发布了多少分钟
        /// </summary>
        public string ReleaseTimeDesc { get; private set; }
        public string ImgUrl { get; private set; }
        public string ThumbnailUrl { get; private set; }
    }

    public class ReleaseAddModel
    {
        /// <summary>
        /// 商品发布信息标题
        /// </summary>
        [DisplayName("标题")]
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [Required]
        [DisplayName("价格")]
        public decimal Price { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        public string Description { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// 交易地址
        /// </summary>
        [DisplayName("地址")]
        [Required]
        public int AddressId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [DisplayName("联系电话")]
        [Required]
        public string Mobile { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DisplayName("联系人")]
        [Required]
        public string Linkman { get; set; }

        /// <summary>
        /// 联系人QQ
        /// </summary>
        [DisplayName("联系人QQ")]
        public string QQ { get; set; }
    }

    public class ReleaseEditModel
    {
        public ReleaseEditModel()
        {

        }
        public ReleaseEditModel(Release release)
        {
            Id = release.Id;
            Title = release.Title;
            Category = release.Category;
            Price = release.Price;
            Description = release.Description;
            Pictures = release.Pictures;
            SecondLvlAddressList = release.TradePlace.ParentAddress
                .SubAddresses
                .Select(a=> new SelectListItem()
                    {
                        Text = a.Name,
                        Value = a.Id.ToString(),
                        Selected = a.Id == release.TradePlace.Id
                    }).ToList();
            Address = release.TradePlace;
            Mobile = release.Mobile;
            Linkman = release.Linkman;
            QQ = release.QQ;
        }

        public Release Update(MarketContext db)
        {
            var model = db.Releases.First(r => r.Id == Id);
            model.Title = Title;
            model.Price = Price;
            model.Description = Description;
            if (RemovedPictures != null)
            {
                var removedPictures = db.Pictures.Where(p => RemovedPictures.Contains(p.Id)).ToList();
                removedPictures.ForEach(p =>
                    {
                        model.Pictures.Remove(p);
                        db.Pictures.Remove(p);
                        try
                        {
                            File.Delete(HttpContext.Current.Server.MapPath(p.Path));
                        }
                        catch { }
                    });
            }

            var addrId = Address.Id;
            model.TradePlace = db.Addresses.First(a => a.Id == addrId);
            model.Mobile = Mobile;
            model.Linkman = Linkman;
            model.QQ = QQ;

            return model;
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("标题")]
        public string Title { get; set; }

        public Category Category { get; set; }

        [Required]
        [DisplayName("价格")]
        public decimal Price { get; set; }

        [MaxLength(500, ErrorMessage = "描述不能超过500个字符")]
        [DisplayName("描述")]
        public string Description { get; set; }

        public List<Picture> Pictures { get; set; }

        public List<int> RemovedPictures { get; set; }

        public List<SelectListItem> SecondLvlAddressList { get; set; }

        [Required]
        public Address Address { get; set; }

        [DisplayName("联系电话")]
        [Required]
        public string Mobile { get; set; }

        [DisplayName("联系人")]
        [Required]
        public string Linkman { get; set; }

        [DisplayName("联系人QQ")]
        public string QQ { get; set; }
    }
}