using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SecondHandMarket.Models;

namespace SecondHandMarket.ViewModels
{
    public class AuthenticateAddModel
    {
        public AuthenticateAddModel()
        {
            
        }

        public AuthenticateAddModel(UserAuthentication authentication)
        {
            Id = authentication.Id;
            IDCard1Path = authentication.IDCard1Path;
            IDCard2Path = authentication.IDCard2Path;
            StudentCardPath = authentication.StudentCardPath;
            StudentNo = authentication.StudentNo;
            IDCardNo = authentication.IDCardNo;
            IsAccepted = authentication.IsAccepted;
        }

        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 身份证正面
        /// </summary>
        public string IDCard1Path { get; set; }

        /// <summary>
        /// 身份证反面
        /// </summary>
        public string IDCard2Path { get; set; }

        /// <summary>
        /// 学生证照片
        /// </summary>
        public string StudentCardPath { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        [Required]
        public string StudentNo { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Required]
        public string IDCardNo { get; set; }

        /// <summary>
        /// 是否通过实名认证
        /// </summary>
        public bool IsAccepted { get; set; }

    }

    public class AuthenticateDetailModel
    {
        public AuthenticateDetailModel(UserAuthentication authentication)
        {
            Id = authentication.Id;
            IDCard1Path = authentication.IDCard1Path;
            IDCard2Path = authentication.IDCard2Path;
            StudentCardPath = authentication.StudentCardPath;
            StudentNo = authentication.StudentNo;
            IDCardNo = authentication.IDCardNo;
            IsAccepted = authentication.IsAccepted;
            RejectMsg = authentication.RejectReason;
        }

        [Required]
        public int Id { get; set; }
        /// <summary>
        /// 身份证正面
        /// </summary>
        public string IDCard1Path { get; set; }

        /// <summary>
        /// 身份证反面
        /// </summary>
        public string IDCard2Path { get; set; }

        /// <summary>
        /// 学生证照片
        /// </summary>
        public string StudentCardPath { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        [Required]
        public string StudentNo { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Required]
        public string IDCardNo { get; set; }

        /// <summary>
        /// 是否通过实名认证
        /// </summary>
        public bool IsAccepted { get; set; }

        public string RejectMsg { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Name { get; set; }

        public AuthenticateDetailModel Prepare(MarketContext db)
        {
            var user = db.UserInfo.First(u => u.Authentication.Id == Id);
            Name = user.RealName;

            return this;
        }
    }
}