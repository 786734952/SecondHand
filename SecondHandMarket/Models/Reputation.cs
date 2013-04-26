using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SecondHandMarket.Models
{
    public class Reputation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 评价等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 留言
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 评价者
        /// </summary>
        public User FromUser { get; set; }

        /// <summary>
        /// 被评价者
        /// </summary>
        public User ToUser { get; set; }
    }
}