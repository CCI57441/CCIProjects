using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiftRegister.Models
{
    public class DomainUser
    {
        [Required(ErrorMessage = "Name is required")]
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string samAccountName { get; set; }

        public string userPrincipalName { get; set; }
    }
}