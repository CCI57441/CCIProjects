using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftRegister.Models
{
    public class GiftReceivedModel
    {
        public Nullable<System.DateTime> Date { get; set; }
        public string NameOfEmployee { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string EmployeeCode { get; set; }
        public Nullable<System.DateTime> DateGiftWasReceived { get; set; }
        public string DescriptionOfGiftReceived { get; set; }
        public string GiftReceivedFrom { get; set; }
        public Nullable<decimal> ActualCost { get; set; }
    }
}