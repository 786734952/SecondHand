using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SecondHandMarket.Models;

namespace SecondHandMarket.ViewModels
{
    public class IndexAddressModel
    {
        public IndexAddressModel(Address address)
        {
            Id = address.Id;
            Name = address.Name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
    }
}