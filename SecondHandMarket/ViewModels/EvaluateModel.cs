using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;
using SecondHandMarket.Models;

namespace SecondHandMarket.ViewModels
{
    public class EvaluateIndexModel
    {
        public User UserInfo { get; set; }

        public MembershipUser User { get; set; }

        public List<Reputation> Reputations { get; set; }

        public int Lvl1Count
        {
            get { return Reputations.Count(r => r.Level == 1); }
        }

        public int Lvl2Count
        {
            get { return Reputations.Count(r => r.Level == 2); }
        }

        public int Lvl3Count
        {
            get { return Reputations.Count(r => r.Level == 3); }
        }

        /// <summary>
        /// 当前用户
        /// </summary>
        public User CurrentUser { get; set; }

        /// <summary>
        /// 添加评价的视图模型
        /// </summary>
        public EvaluateAddModel AddModel
        {
            get
            {
                return new EvaluateAddModel()
                    {
                        Level = 1,
                        ToUserName = User.UserName
                    };
            }
        }
    }

    public class EvaluateAddModel
    {
        [Required]
        [DisplayName("评价级别")]
        public int Level { get; set; }

        [Required]
        [DisplayName("评价")]
        public string Remark { get; set; }

        /// <summary>
        /// 被评价者
        /// </summary>
        public string ToUserName { get; set; }

        public Reputation GetReputation(MarketContext db, string fromUserName)
        {
            var fromUser = db.UserInfo.First(u => u.UserName == fromUserName);
            var reputation = new Reputation()
                {
                    CreateTime = DateTime.Now,
                    FromUser = fromUser,
                    Level = Level,
                    Remark = Remark,
                    ToUser = ToUserName
                };

            return reputation;
        }
    }
}