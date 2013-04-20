using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SecondHandMarket.Models;

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
            var pastTimespan = DateTime.Now - ReleaseTime;

            if (pastTimespan.TotalMinutes < 60)
            {
                var minutes = (int)Math.Floor(pastTimespan.TotalMinutes);
                ReleaseTimeDesc =minutes + "分钟前";
            }
            else if (pastTimespan.TotalHours <= 12)
            {
                var hours = (int) Math.Floor(pastTimespan.TotalHours);
                ReleaseTimeDesc = hours + "小时" + Math.Floor((pastTimespan.TotalHours - hours) * 60) + "分钟前";
            }
            else if (pastTimespan.TotalHours <= 24 && DateTime.Now.Day == ReleaseTime.Day)
            {
                var hours = (int) Math.Floor(pastTimespan.TotalHours);
                ReleaseTimeDesc = hours + "小时" + Math.Floor((pastTimespan.TotalHours - hours) * 60) + "分钟前";
            }
            else
            {
                ReleaseTimeDesc = ReleaseTime.ToString("yyyy-MM-dd HH:mm");
            }

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
        [MaxLength(500, ErrorMessage = "描述不能超过500个字符")]
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
}