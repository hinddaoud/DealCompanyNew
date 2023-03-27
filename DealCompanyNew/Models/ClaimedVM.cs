using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DealCompanyNew.Models
{
    public class ClaimedVM
    {

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        public int User_ID { get; set; }
        public int Deal_ID { get; set; } 
        public double Amount { get; set; }
        public string Currency { get; set; }

       
    }
}