using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandMarket.Models
{
    public class ReleaseCollect 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Release Release { get; set; }
        public string UserName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
