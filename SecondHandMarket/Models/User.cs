using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SecondHandMarket.Models
{
    [Table("UserInfo")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户QQ
        /// </summary>
        public string QQ { get; set; }

        /// <summary>
        /// 用户手机
        /// </summary>
        public string Mobile { get; set; }
    }
}