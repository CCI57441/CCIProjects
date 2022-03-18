using GiftRegister.Data;
using GiftRegister.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace GiftRegister.Controllers
{
    public class DBAccessLayer
    {
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DBGR"].ToString();
            con = new SqlConnection(constr);

        }
       public void AddGift(GiftGivenModel giveEntities)
        {
            connection();
            //SqlCommand com = new SqlCommand("procsys_AddNewGift", con);
            SqlCommand com = new SqlCommand("spAddNewGift", con);
            com.CommandType = CommandType.StoredProcedure;
            //com.Parameters.AddWithValue("@NameOfEmployee", giveEntities.NameOfEmployee);
            com.Parameters.AddWithValue("@EmployeeCode", giveEntities.EmployeeCode);
            com.Parameters.AddWithValue("@Surname", giveEntities.Surname);
            com.Parameters.AddWithValue("@Firstname", giveEntities.Firstname);
            com.Parameters.AddWithValue("@DateGiftWasGiven", giveEntities.DateGiftWasGiven);
            com.Parameters.AddWithValue("@DescriptionOfGiftGiven", giveEntities.DescriptionOfGiftGiven);
            com.Parameters.AddWithValue("@GiftGivenTo", giveEntities.GiftGivenTo);
            com.Parameters.AddWithValue("@ActualCost", giveEntities.ActualCost);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }

        public void AddReceivedGift(GiftReceivedModel giveEntitie)
        {
            connection();
            //SqlCommand com = new SqlCommand("procsys_AddReceivedGift", con);
            SqlCommand com = new SqlCommand("spAddReceivedGift", con);
            com.CommandType = CommandType.StoredProcedure;
            //com.Parameters.AddWithValue("@NameOfEmployee", giveEntitie.NameOfEmployee);
            com.Parameters.AddWithValue("@Firstname", giveEntitie.Firstname);
            com.Parameters.AddWithValue("@Surname", giveEntitie.Surname);
            com.Parameters.AddWithValue("@EmployeeCode", giveEntitie.EmployeeCode);
            com.Parameters.AddWithValue("@DateGiftWasReceived", giveEntitie.DateGiftWasReceived);
            com.Parameters.AddWithValue("@DescriptionOfGiftReceived", giveEntitie.DescriptionOfGiftReceived);
            com.Parameters.AddWithValue("@GiftReceivedFrom", giveEntitie.GiftReceivedFrom);
            com.Parameters.AddWithValue("@ActualCost", giveEntitie.ActualCost);
            con.Open();
            com.ExecuteNonQuery();
            con.Close();

        }
    }
}