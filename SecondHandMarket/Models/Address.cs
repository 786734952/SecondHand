using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SecondHandMarket.Models
{
    public class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("地址")]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Address ParentAddress { get; set; }

        public virtual List<Address> SubAddresses { get; set; }
    }
}