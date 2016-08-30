using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cleveroad.Models
{
    public class Order
    {
       [HiddenInput(DisplayValue = true)]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? OrderDate { get; set; }
        public string OrderData { get; set; }
        public int OrderRoundedWeight { get; set; }
        public Order()
        {
            OrderDate = DateTime.Now;
        }
    }
}