using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondHandMarket.ViewModels
{
    public class PageBarModel
    {
        public int Total { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int PageCount
        {
            get { return (int)Math.Ceiling(Total / (double)PageSize); }
        }

        public int SkipCount
        {
            get { return (PageIndex - 1)*PageSize; }
        }

        /// <summary>
        /// List?PageIndex={0}&PageSize={1}
        /// </summary>
        public string Url { get; set; }

        public string GetUrl(int index)
        {
            var temp = "?";
            if (Url.Contains("?"))
            {
                temp = "&";
            }
            return String.Format(Url + temp + "PageIndex={0}&PageSize={1}", index, PageSize);
        }
    }
}