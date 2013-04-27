using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using SecondHandMarket.Models;

namespace SecondHandMarket.ViewModels
{
    public class UserEditModel
    {
        public UserEditModel()
        {
            
        }
        public UserEditModel(User user, string userName)
        {
            UserName = userName;
            if (user != null)
            {
                RealName = user.RealName;
                Mobile = user.Mobile;
                QQ = user.QQ;
                Gender = user.Gender;
                EntranceYear = user.EntranceYear;
            }
        }

        [Required]
        public string UserName { get; set; }

        [DisplayName("真实姓名")]
        [Required]
        public string RealName { get; set; }

        [DisplayName("联系电话")]
        public string Mobile { get; set; }

        [DisplayName("QQ")]
        public string QQ { get; set; }

        [Required]
        [DisplayName("性别")]
        public int Gender { get; set; }

        [Required]
        [DisplayName("入学年份")]
        public string EntranceYear { get; set; }

        public List<SelectListItem> Years
        {
            get
            {
                var temp = new List<SelectListItem>();
                var year = DateTime.Now.Year;
                for (int i = year - 5; i <= year; i++)
                {
                    temp.Add(new SelectListItem()
                        {
                            Text = i.ToString(),
                            Value = i.ToString()
                        });
                }
                temp.Insert(0, new SelectListItem());
                return temp;
            }
        }
    }

    public class UserDetailModel
    {
        public UserDetailModel(User user, string userName)
        {
            if (user != null)
            {
                RealName = user.RealName;
                Mobile = user.Mobile;
                QQ = user.QQ;
                if (user.Gender == 1)
                {
                    Gender = "男";
                }
                else
                {
                    Gender = "女";
                }
                EntranceYear = user.EntranceYear;
                IsAuthenticated = user.Authentication != null
                                  && user.Authentication.IsAccepted;
            }

            UserName = userName;
            var membershipUser = Membership.GetUser(userName);
            CreateTime = membershipUser.CreationDate;
            Email = membershipUser.Email;
        }

        public string UserName { get; set; }

        [DisplayName("真实姓名")]
        public string RealName { get; set; }

        [DisplayName("联系电话")]
        public string Mobile { get; set; }

        [DisplayName("QQ")]
        public string QQ { get; set; }

        [DisplayName("性别")]
        public string Gender { get; set; }

        [DisplayName("入学年份")]
        public string EntranceYear { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否已实名认证
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 好评数
        /// </summary>
        public int Lvl1Count { get; set; }

        /// <summary>
        /// 中评数
        /// </summary>
        public int Lvl2Count { get; set; }

        /// <summary>
        /// 差评数
        /// </summary>
        public int Lvl3Count { get; set; }

        /// <summary>
        /// 评价详情
        /// </summary>
        public List<Reputation> Reputations { get; private set; }

        public UserDetailModel Prepare(MarketContext db)
        {

            return this;
        }
    }
}