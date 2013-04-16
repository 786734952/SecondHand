using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SecondHandMarket.Models
{
    public class MarketContext : DbContext
    {
        public MarketContext()
            : base("DefaultConnection")
        {

        }

        /// <summary>
        /// 商品分类
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// 商品信息
        /// </summary>
        public DbSet<Release> Releases { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<User> UserInfo { get; set; }

        /// <summary>
        /// 交易地址
        /// </summary>
        public DbSet<Address> Addresses { get; set; }
    }
}