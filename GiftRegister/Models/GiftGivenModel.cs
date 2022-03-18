using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftRegister.Models
{
    public class GiftGivenModel
    {
        public int GiftId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string NameOfEmployee { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string EmployeeCode { get; set; }
        
        public Nullable<System.DateTime> DateGiftWasGiven { get; set; }
        public string DescriptionOfGiftGiven { get; set; }
        public string GiftGivenTo { get; set; }
        public Nullable<decimal> ActualCost { get; set; }
    }
}