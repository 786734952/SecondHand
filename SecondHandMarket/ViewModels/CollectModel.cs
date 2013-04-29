using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SecondHandMarket.Models;
using SecondHandMarket.common;

namespace SecondHandMarket.ViewModels
{
    public class CollectListItemModel
    {
        public CollectListItemModel(ReleaseCollect collect)
        {
            Id = collect.Id;
            Name = collect.Release.Title;
            RelatedId = collect.Release.Id;
            CollectType = 0;
            CollectTime = collect.CreateTime;
            CollectTimeDesc = CollectTime.GetDateTimeDesc();
            Price = collect.Release.Price;
        }

        public CollectListItemModel(BuyCollect collect)
        {
            Id = collect.Id;
            Name = collect.Buy.Name;
            RelatedId = collect.Buy.Id;
            CollectType = 1;
            CollectTime = collect.CreateTime;
            CollectTimeDesc = CollectTime.GetDateTimeDesc();
            Price = collect.Buy.Price;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 求购或者发布信息的id
        /// </summary>
        public int RelatedId { get; set; }

        /// <summary>
        /// 0为发布，1为求购
        /// </summary>
        public int CollectType { get; set; }

        /// <summary>
        /// 收藏的时间
        /// </summary>
        public DateTime CollectTime { get; set; }

        public string CollectTimeDesc { get; private set; }

        public decimal Price { get; set; }
    }
}