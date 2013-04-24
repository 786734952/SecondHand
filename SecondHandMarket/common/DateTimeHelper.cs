using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondHandMarket.common
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// 获取可读性的时间描述
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string GetDateTimeDesc(this DateTime dateTime)
        {
            var pastTimespan = DateTime.Now - dateTime;

            if (pastTimespan.TotalMinutes < 60)
            {
                var minutes = (int)Math.Floor(pastTimespan.TotalMinutes);
                return minutes + "分钟前";
            }
            else if (pastTimespan.TotalHours <= 12)
            {
                var hours = (int)Math.Floor(pastTimespan.TotalHours);
                return hours + "小时" + Math.Floor((pastTimespan.TotalHours - hours) * 60) + "分钟前";
            }
            else if (pastTimespan.TotalHours <= 24 && DateTime.Now.Day == dateTime.Day)
            {
                var hours = (int)Math.Floor(pastTimespan.TotalHours);
                return hours + "小时" + Math.Floor((pastTimespan.TotalHours - hours) * 60) + "分钟前";
            }
            else
            {
                return dateTime.ToString("yyyy-MM-dd HH:mm");
            }
        }
    }
}