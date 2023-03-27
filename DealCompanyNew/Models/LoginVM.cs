using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DealCompanyNew.Models
{
    public class LoginVM
    {

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }




        [Required]
        [Display(Name = "UserName")]
        //  [Display(Name = "User Name")]
        public string Name { get; set; }


        [Required]
        
        //  [Display(Name = "User Name")]
        public string Email{ get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}