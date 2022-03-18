using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftRegister.Logic
{
    public class GiftRegisterDBAccessLayer
    {
        SqlConnection con = new SqlConnection("data source = localhost\SQLEXPRES; database=GiftRegisterDB; integrated security = True;");
        public string AddGivenGiftRecord(EmployeeEntities employeeEntities)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spAddNewGift", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NameOfEmployee", employeeEntities.NameOfEmployee);
                cmd.Parameters.AddWithValue("@DateGiftWasGiven", employeeEntities.DateGiftWasGiven);
                cmd.Parameters.AddWithValue("@DescriptionOfGiftGiven", employeeEntities.DescriptionOfGiftGiven);
                cmd.Parameters.AddWithValue("@GiftGivenTo", employeeEntities.GiftGivenTo);
                cmd.Parameters.AddWithValue("@ctualCost", employeeEntities.ctualCost);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return ("Data save Successfully");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return (ex.Message.ToString());
            }
        }
    }
}