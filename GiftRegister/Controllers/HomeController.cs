
using GiftRegister.Data;
using GiftRegister.Logic;
using GiftRegister.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GiftRegister.Controllers
{
    public class HomeController : Controller
    {
       BDGiftsRegisterEntities myDb = new BDGiftsRegisterEntities();
        ActiveUserDirectory userDir = new ActiveUserDirectory();
        EntertainmentDbEntities db = new EntertainmentDbEntities();
      
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DBGR"].ToString();
            con = new SqlConnection(constr);

        }
        public ActionResult Index(string Prefix)
        {
            ViewBag.Name = userDir.GetCurrentDisplayName();
            ViewBag.empcode = userDir.GetCurrentSamUserName();
             return View();
        }
        
        [HttpPost]
        public JsonResult GetName(string Prefix)
        {

            ActiveUserDirectory userDir = new ActiveUserDirectory();
            var usernames = userDir.GetAllUsers("CCI", Prefix); 
            return Json(usernames, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrentName()
        {
            ActiveUserDirectory userDir = new ActiveUserDirectory();
            var username = userDir.GetCurrentDisplayName();
            return Json(username, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
       
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GiftGiven()
        {
            try
            {
                Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                WindowsPrincipal principal = (WindowsPrincipal)Thread.CurrentPrincipal;
                // or, if you're in Asp.Net with windows authentication you can use:
                // WindowsPrincipal principal = (WindowsPrincipal)User;
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
                {
                    UserPrincipal up = UserPrincipal.FindByIdentity(pc, principal.Identity.Name);
                    var nm = up.GivenName;
                    var surname = up.Surname;
                    var username = up.EmployeeId;
                    //return up.DisplayName;
                    ViewBag.fName = nm;
                    ViewBag.sName = surname;
                }
            }
            catch (Exception)
            {
               
            }

            ViewBag.Name = userDir.GetCurrentDisplayName();
            ViewBag.empcode = userDir.GetCurrentSamUserName();
            
            return View();
        }

        // Sending email
        [HttpPost]
        public ActionResult GiftGiven(string NameOfEmployee, string DateGiftWasGiven, string DescriptionOfGiftGiven, string GiftGivenTo, string ActualCost, GiftGivenModel giveEntities)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    string sBody; 
                    SmtpClient SMTPMail = new SmtpClient("webmail.cci-sa.co.za");
                    sBody = "<html><body>";
                    sBody += "<p>Good day,</p>";
                    sBody += "<p>A Gift has just been registered with the following details:</p>";
                    sBody += "<hr />";
                    sBody += "<p>Name: " + NameOfEmployee + "</p>";
                    sBody += "<p>Date Gift Was Given: " + DateGiftWasGiven + "</p>";
                    sBody += "<p>Description Of Gift Given: " + DescriptionOfGiftGiven + "</p>";
                    sBody += "<p>Gift Given To(name and company): " + GiftGivenTo + "</p>";
                    sBody += "<p>Actual Cost: " + ActualCost + "</p>";
                    sBody += "<hr />";
                    sBody += "<p>Do not respond to this email</p>";
                    sBody += "</body></html>";
                    //SMTPMail.Host = "webmail.cci-sa.co.za";
                    SMTPMail.EnableSsl = true;
                    SMTPMail.Timeout = 100000;
                    SMTPMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SMTPMail.UseDefaultCredentials = false;
                    SMTPMail.Port=587;
                    //string sToSD = "Tumelo.Mofokeng@ccisouthafrica.com";

                    string sToSD = "RiskAndCompliance@ccisouthafrica.com";
                    MailMessage mailMessage = new MailMessage(); 
                    mailMessage.From = new MailAddress("noreply@cci-sa.co.za");
                    mailMessage.To.Add(sToSD);
                    mailMessage.Bcc.Add("Tumelo.Mofokeng@ccisouthafrica.com");
                    mailMessage.Subject = "Gift Register";
                    mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = sBody;
                    mailMessage.Priority = MailPriority.High;
                    SMTPMail.Send(mailMessage);
                    ViewBag.Error = "Email send to the receiver";
                    AddGift(giveEntities);                 
                }
                
            }
            catch (Exception ex)
            {
                
                ViewBag.Error = "we have problem of sending the email...! :" + ex.Message;
            }
            ViewBag.Name = userDir.GetCurrentDisplayName();
            ViewBag.empcode = userDir.GetCurrentSamUserName();
            return View("GiftGiven");
        }
 
        //To add Records into database     
        private void AddGift(GiftGivenModel giveEntities)
        {
            connection();
            SqlCommand com = new SqlCommand("procsys_AddNewGift", con);
            //SqlCommand com = new SqlCommand("spAddNewGift", con);
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
        public ActionResult GiftReceived()
        {
            try
            {
                Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                WindowsPrincipal principal = (WindowsPrincipal)Thread.CurrentPrincipal;
                // or, if you're in Asp.Net with windows authentication you can use:
                // WindowsPrincipal principal = (WindowsPrincipal)User;
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
                {
                    UserPrincipal up = UserPrincipal.FindByIdentity(pc, principal.Identity.Name);
                    var nm = up.GivenName;
                    var surname = up.Surname;
                    var username = up.EmployeeId;
                    //return up.DisplayName;
                    ViewBag.fName = nm;
                    ViewBag.sName = surname;
                }
            }
            catch (Exception)
            {

            }

            ViewBag.Name = userDir.GetCurrentDisplayName();
            ViewBag.empcode = userDir.GetCurrentSamUserName();
            return View();
        }

        [HttpPost]
        public ActionResult GiftReceived(string NameOfEmployee, string DateGiftWasReceived, string DescriptionOfGiftReceived, string GiftReceivedFrom, string ActualCost, GiftReceivedModel giveEntitie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string sBody;
                    SmtpClient SMTPMail = new SmtpClient("webmail.cci-sa.co.za");
                    sBody = "<html><body>";
                    sBody += "<p>Good day,</p>";
                    sBody += "<p>A Gift has just been registered with the following details:</p>";
                    sBody += "<hr />";
                    sBody += "<p>Name: " + NameOfEmployee + "</p>";
                    sBody += "<p>Date Gift Was Received: " + DateGiftWasReceived + "</p>";
                    sBody += "<p>Description Of Gift Received: " + DescriptionOfGiftReceived + "</p>";
                    sBody += "<p>Gift Received From (name and company): " + GiftReceivedFrom + "</p>";
                    sBody += "<p>Actual Cost: " + ActualCost + "</p>";
                    sBody += "<hr />";
                    sBody += "<p>Do not respond to this email</p>";
                    sBody += "</body></html>";
                    //SMTPMail.Host = "webmail.cci-sa.co.za";
                    SMTPMail.EnableSsl = true;
                    SMTPMail.Timeout = 100000;
                    SMTPMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                    SMTPMail.UseDefaultCredentials = false;
                    SMTPMail.Port = 587;
                  
                    //string sToSD = "Nishern.Dheepnarayan@ccisouthafrica.com";
                    string sToSD = "RiskAndCompliance@ccisouthafrica.com";
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("noreply@cci-sa.co.za");
                    mailMessage.To.Add(sToSD);
                    mailMessage.Bcc.Add("Tumelo.Mofokeng@ccisouthafrica.com");
                    mailMessage.Subject = "Gift Register";
                    mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = sBody;
                    mailMessage.Priority = MailPriority.High;
                    SMTPMail.Send(mailMessage);
                    ViewBag.Error = "Email send to the receiver";
                    AddReceivedGift(giveEntitie);
                }
            }
            catch (Exception ex)
            {

                ViewBag.Error = "we have problem of sending the email...! :" + ex.Message; 
            }
            ViewBag.Name = userDir.GetCurrentDisplayName();
           // SaveDataInDatabase();
            return View("GiftReceived");
        }

        private void AddReceivedGift(GiftReceivedModel giveEntitie)
        {
            connection();
            SqlCommand com = new SqlCommand("procsys_AddReceivedGift", con);
            //SqlCommand com = new SqlCommand("spAddReceivedGift", con);
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
        public ActionResult Close()
        {
            ViewBag.Name = userDir.GetCurrentDisplayName();
            ViewBag.empcode = userDir.GetCurrentSamUserName();
            return View("Index");
        }


    }
}