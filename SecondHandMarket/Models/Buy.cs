using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandMarket.Models
{
    public class Buy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Category { get; set; }
        public string Name { get; set; }
        public float price { get; set; }
        public string Describe { get; set; }
        public string Place { get; set; }
        public string Telephone { get; set; }
        public string Linkman { get; set; }
        public string QQ { get; set; }
        public DateTime BuyTime { get; set; }
        public string UserName { get; set; }
    }
}
