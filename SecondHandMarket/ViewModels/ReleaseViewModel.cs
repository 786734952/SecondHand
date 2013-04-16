using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SecondHandMarket.ViewModels
{
    public class ReleaseViewModel
    {
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
        /// 交易地址
        /// </summary>
        [DisplayName("地址")]
        public int AddressId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [DisplayName("联系电话")]
        public string Mobile { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [DisplayName("联系人")]
        public string Linkman { get; set; }

        /// <summary>
        /// 联系人QQ
        /// </summary>
        [DisplayName("联系人QQ")]
        public string QQ { get; set; }
    }
}