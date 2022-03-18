using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using GiftRegister.Models;

namespace GiftRegister.Models
{
    public class Gift_DB
    {
        // declare connection string
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DBGR"].ToString();
            con = new SqlConnection(constr);

        }


        //To add Records into database     
        private void AddGift(GiftGivenModel giveEntities)
        {
            connection();
            SqlCommand com = new SqlCommand("spAddNewGift", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@NameOfEmployee", giveEntities.NameOfEmployee);

            com.Parameters.AddWithValue("@DateGiftWasGiven", giveEntities.DateGiftWasGiven);
            com.Parameters.AddWithValue("@DescriptionOfGiftGiven", giveEntities.DescriptionOfGiftGiven);

            com.Parameters.AddWithValue("@GiftGivenTo", giveEntities.GiftGivenTo);
            ;
            com.Parameters.AddWithValue("@ActualCost", giveEntities.ActualCost);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

        }
        //Return list of all Employees
        //public List<GiftGivenModel> ListAll()
        //{
        //    List<GiftGivenModel> lst = new List<GiftGivenModel>();
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        con.Open();
        //        SqlCommand com = new SqlCommand("SelectEmployee", con);
        //        com.CommandType = CommandType.StoredProcedure;
        //        SqlDataReader rdr = com.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            lst.Add(new Employee
        //            {
        //                EmployeeID = Convert.ToInt32(rdr["EmployeeId"]),
        //                Name = rdr["Name"].ToString(),
        //                Age = Convert.ToInt32(rdr["Age"]),
        //                State = rdr["State"].ToString(),
        //                Country = rdr["Country"].ToString(),
        //            });
        //        }
        //        return lst;
        //    }
        //}

        //Method for Adding an Employee
        //public int Add(GiftGivenModel emp)
        //{
        //    int i;
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        con.Open();
        //        SqlCommand com = new SqlCommand("spAddNewGift", con);
        //        com.CommandType = CommandType.StoredProcedure;
        //        com.Parameters.AddWithValue("@NameOfEmployee", emp.NameOfEmployee);
        //        com.Parameters.AddWithValue("@DateGiftWasGiven", emp.DateGiftWasGiven);
        //        com.Parameters.AddWithValue("@DescriptionOfGiftGiven", emp.DescriptionOfGiftGiven);
        //        com.Parameters.AddWithValue("@GiftGivenTo", emp.GiftGivenTo);
        //        com.Parameters.AddWithValue("@ctualCost", emp.ActualCost);
        //        //com.Parameters.AddWithValue("@Action", "Insert");
        //        i = com.ExecuteNonQuery();
        //    }
        //    return i;
        //}

        // Method for Updating Employee record
        //public int Update(Employee emp)
        //{
        //    int i;
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        con.Open();
        //        SqlCommand com = new SqlCommand("InsertUpdateEmployee", con);
        //        com.CommandType = CommandType.StoredProcedure;
        //        com.Parameters.AddWithValue("@Id", emp.EmployeeID);
        //        com.Parameters.AddWithValue("@Name", emp.Name);
        //        com.Parameters.AddWithValue("@Age", emp.Age);
        //        com.Parameters.AddWithValue("@State", emp.State);
        //        com.Parameters.AddWithValue("@Country", emp.Country);
        //        com.Parameters.AddWithValue("@Action", "Update");
        //        i = com.ExecuteNonQuery();
        //    }
        //    return i;
        //}

        //Method for Deleting an Employee
        //public int Delete(int ID)
        //{
        //    int i;
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        con.Open();
        //        SqlCommand com = new SqlCommand("DeleteEmployee", con);
        //        com.CommandType = CommandType.StoredProcedure;
        //        com.Parameters.AddWithValue("@Id", ID);
        //        i = com.ExecuteNonQuery();
        //    }
        //    return i;
        //}
    }
}
