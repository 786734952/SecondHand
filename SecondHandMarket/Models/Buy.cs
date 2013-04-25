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
        public virtual Category Category { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public virtual Address Place { get; set; }
        public string Mobile { get; set; }
        public string QQ { get; set; }
        public DateTime CreateTime { get; set; }
        public string UserName { get; set; }
    }
}
