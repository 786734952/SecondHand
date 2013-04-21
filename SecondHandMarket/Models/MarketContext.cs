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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Category>()
            //            .HasOptional(c => c.SubCategories)
            //            .WithMany();

            base.OnModelCreating(modelBuilder);

        }
        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }

        public bool IsDisposed { get; private set; }

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

        /// <summary>
        /// 图片
        /// </summary>
        public DbSet<Picture> Pictures { get; set; }
    }
}