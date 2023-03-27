using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DealCompanyNew.Models
{
    public class InformationVM
    {
        [Key]
        public int ID { get; set; }
        public string Server_DateTime { get; set; }
        public string DateTime_UTC { get; set; }
        public string Update_DateTime_UTC { get; set; }
        public string Last_Login_DateTime_UTC { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string Gender { get; set; }
        public string Date_Of_Birth { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string UserType { get; set; }
        public List<ClaimedDeal> Amount { get; set; }
        public double Total { get; set; }

    }
}