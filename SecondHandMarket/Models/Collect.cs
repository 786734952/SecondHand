using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandMarket.Models
{
    public class Collect 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Place { get; set; }
        public string LinkMan { get; set; }
        public string Telephone { get; set; }
        public string QQ { get; set; }
        public string Describe { get; set; }
        public string Photo { get; set; }
        public string UserName { get; set; }

    }
}
