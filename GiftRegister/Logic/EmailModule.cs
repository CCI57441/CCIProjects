using GiftRegister.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace GiftRegister.Logic
{
    public class EmailModule
    {

        private static bool _mailSent = false;


        public static string Pass, FromEmailid, HostAdd;

        public static void Email_Without_Attachment(String ToEmail, String Subj, string Message)
        {
            //Reading sender Email credential from web.config file
            HostAdd = ConfigurationManager.AppSettings["Host"].ToString();
            FromEmailid = ConfigurationManager.AppSettings["FromMail"].ToString();
            Pass = ConfigurationManager.AppSettings["Password"].ToString();

            //creating the object of MailMessage
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(FromEmailid); //From Email Id
            mailMessage.Subject = Subj; //Subject of Email
            mailMessage.Body = Message; //body or message of Email
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(new MailAddress(ToEmail)); //reciver's Email Id

            SmtpClient smtp = new SmtpClient(); // creating object of smptpclient
            smtp.Host = HostAdd; //host of emailaddress for example smtp.gmail.com etc

            //network and security related credentials

            smtp.EnableSsl = true;
            //NetworkCredential NetworkCred = new NetworkCredential();
            //NetworkCred.UserName = mailMessage.From.Address;
            //NetworkCred.Password = Pass;
            smtp.UseDefaultCredentials = true;
            //smtp.Credentials = NetworkCred;
            smtp.Port = 25;
            smtp.Send(mailMessage); //sending Email
        }
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            _mailSent = true;

        }

       
        public static void ProcessEmail(GiftEntertainment gift, ActiveUserDirectory userDir, bool isGiven = false)
        {
            SmtpClient client = new SmtpClient("webmail.cci-sa.co.za", 25);


            MailAddress from = new MailAddress("noreply@cci-sa.com");

            //MailAddress to = new MailAddress("RiskAndCompliance@ccisouthafrica.com");
            MailAddress to = new MailAddress("Tumelo.Mofokeng@ccisouthafrica.com");
            // Specify the message content.
            MailMessage message = new MailMessage(from, to);

            var date = gift.Date;
            var name = gift.NameOfEmployee;
            var dateGiftWasGiven = gift.DateGiftWasGiven;
            var descriptionOfGiftGiven = gift.DescriptionOfGiftGiven;
            var NameandCompany = gift.GiftGivenTo;
            var Cost = gift.ActualCost;
            var nameReceive = gift.NameOfEmployee;
            var dateGiftWasReceived = gift.DateGiftWasReceived;
            var descriptionOfGiftReceived = gift.DescriptionOfGiftReceived;
            var NameandCompanyReceived = gift.GiftReceivedFrom;
            var CostReceived = gift.ActualCost;
            message.Body = "<div style='width:300px; height:300px;'><h3 style='color:#121f47'>A Gift has just been registered with the following details</h3></div>";
            message.Body += Environment.NewLine;
            message.Body += "<ul>";
            if (isGiven)
            {
                message.Body += "<li style='list-style-type:none'>Entry Date:  " + date + "</li>";
                message.Body += "<li style='list-style-type:none'>Name Of Employee:  " + userDir.GetCurrentDisplayName() + "</li>";
                message.Body += "<li style='list-style-type:none'>Username:  " + name + "</li>";
                message.Body += "<li style='list-style-type:none'>Date Gift Was Given:  " + dateGiftWasGiven + "</li>";
                message.Body += "<li style='list-style-type:none'>Description Of Gift Given:  " + descriptionOfGiftGiven + "</li>";
                message.Body += "<li style='list-style-type:none'>Name and Company:  " + NameandCompany + "</li>";
                message.Body += "<li style='list-style-type:none'>Actual/Estimated Rand Value (ZAR):  " + Cost + "</li>";
            }
            else
            {
                message.Body += "<li style='list-style-type:none'>Entry Date:  " + date + "</li>";
                message.Body += "<li style='list-style-type:none'>Name Of Employee:  " + userDir.GetCurrentDisplayName() + "</li>";
                message.Body += "<li style='list-style-type:none'>Username:  " + nameReceive + "</li>";
                message.Body += "<li style='list-style-type:none'>Date Gift Was Received:  " + dateGiftWasReceived + "</li>";
                message.Body += "<li style='list-style-type:none'>Description Of Gift Received:  " + descriptionOfGiftReceived + "</li>";
                message.Body += "<li style='list-style-type:none'>Name and Company:  " + NameandCompanyReceived + "</li>";
                message.Body += "<li style='list-style-type:none'>Actual/Estimated Rand Value (ZAR):  " + CostReceived + "</li>";
            }
            message.Body += "</ul>";
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "Gift Register";
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;

            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new
            SendCompletedEventHandler(EmailModule.SendCompletedCallback);

            string userState = "test message1";
            client.SendAsync(message, userState);
            Console.WriteLine("Sending message... press c to cancel mail. Press any other key to exit.");
            string answer = Console.ReadLine();

            if (answer.StartsWith("c") && _mailSent == false)
            {
                client.SendAsyncCancel();
            }
            // Clean up.
            message.Dispose();
            
        }
    }


}