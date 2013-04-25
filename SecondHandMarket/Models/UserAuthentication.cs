using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SecondHandMarket.Models
{
    public class UserAuthentication
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// 身份证正面
        /// </summary>
        public Picture IDCard1 { get; set; }

        /// <summary>
        /// 身份证反面
        /// </summary>
        public Picture IDCard2 { get; set; }

        /// <summary>
        /// 学生证照片
        /// </summary>
        public Picture StudentCard { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNo { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IDCardNo { get; set; }

        /// <summary>
        /// 是否通过实名认证
        /// </summary>
        public bool IsAccepted { get; set; }
    }
}