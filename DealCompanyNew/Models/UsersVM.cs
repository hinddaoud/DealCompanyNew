using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DealCompanyNew.Models
{
    public class UsersVM
    {

        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        //// [Display(Name = "Server string")]
        // [HiddenInput(DisplayValue = false)]
        // public string Server_string { get; set; }

        // // [Display(Name = "string UTC")]
        // [HiddenInput(DisplayValue = false)]
        // public string string_UTC { get; set; }

        // // [Display(Name = "Update string UTC")]
        // [HiddenInput(DisplayValue = false)]
        // public string Update_string_UTC { get; set; }

        // // [Display(Name = "Last Login string UTC")]
        // [HiddenInput(DisplayValue = false)]
        // public string Last_Login_string_UTC { get; set; }

        [Required(ErrorMessage = "UserName is Required")]
        [Display(Name = "User Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Date Of Birth")]
        public string Date_Of_Birth { get; set; }
    }
}