using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandMarket.Models
{
    /// <summary>
    /// 商品发布信息
    /// </summary>
    public class Release 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// 商品分类
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// 商品标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 商品价格
        /// </summary>
        public float Price { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 关联的图片
        /// </summary>
        public List<Picture> Pictures { get; set; }

        /// <summary>
        /// 交易地点
        /// </summary>
        public string TradePlace { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Linkman { get; set; }

        /// <summary>
        /// 联系人QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 发布功能
        /// </summary>
        public DateTime ReleaseTime { get; set; }
    }
}
