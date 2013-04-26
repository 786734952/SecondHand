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

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 性别, 1为男， 2为女
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 入学年份
        /// </summary>
        public string EntranceYear { get; set; }

        /// <summary>
        /// 用户的实名认证信息
        /// </summary>
        public virtual UserAuthentication Authentication { get; set; }
    }
}