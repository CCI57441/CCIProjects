using System;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Text;

/// <summary>
/// Summary description for SendEmail
/// Article by vithal wadje http://www.c-sharpcorner.com/Authors/0c1bb2/
/// Facebook Profile:   www.facebook.com/vithal.wadje
//twitter Profile      :https://twitter.com/vithalwadjeC97
//LinedIn Profile    : http://www.linkedin.com/pub/vithal-wadje/69/83a/330

/// </summary>
namespace GiftRegister.Logic
{
    public static class SendEmail
     {
        public static void GiftGiven(string NameOfEmployee, string DateGiftWasGiven, string DescriptionOfGiftGiven, string GiftGivenTo, string ActualCost)

        {
            try
            {
                //if (ModelState.IsValid)
                //{

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
                    SMTPMail.Port = 587;
                    //string sToSD = "Nishern.Dheepnarayan@ccisouthafrica.com";
                    string sToSD = "Tumelo.Mofokeng@ccisouthafrica.com";
                    //string sToSD = "RiskAndCompliance@ccisouthafrica.com";
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress("noreply@cci-sa.co.za");
                    mailMessage.To.Add(sToSD);
                    mailMessage.Subject = "Gift Register";
                    mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.Body = sBody;
                    mailMessage.Priority = MailPriority.High;
                    SMTPMail.Send(mailMessage);
                    //ViewBag.Error = "Email send to the receiver";
                    //AddGift(giveEntities);

                //}

            }
            catch (Exception ex)
            {
                //TempData["msg"] = ex.Message;
                //ViewBag.Error = "we have problem of sending the email...!";
            }
            //ViewBag.Name = userDir.GetCurrentDisplayName();
            //ViewBag.empcode = userDir.GetCurrentSamUserName();
            //return View("GiftGiven");
        }



    }
}