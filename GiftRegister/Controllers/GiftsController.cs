using GiftRegister.Data;
using GiftRegister.Logic;
using GiftRegister.Repos;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;

namespace GiftRegister.Controllers
{
    public class GiftsController : ApiController
    {
        IGiftRepository _repo;
        ActiveUserDirectory userDir = new ActiveUserDirectory();
     
        public GiftsController(IGiftRepository repo)
        {
            _repo = repo;
        }
        // GET: api/Gifts
        public IEnumerable<GiftEntertainment> Get()
        {
            var gifts = _repo.GetAllGifts().ToList();
            return gifts;
        }

        // GET: api/Gifts/5
        public string Get(int id)
        {
            return "value";
        }

        public string GetCurrentDisplayName()
        {
            //Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            WindowsPrincipal principal = (WindowsPrincipal)Thread.CurrentPrincipal;
            // or, if you're in Asp.Net with windows authentication you can use:
            // WindowsPrincipal principal = (WindowsPrincipal)User;
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(pc, principal.Identity.Name);
                var nm = up.GivenName;
                var surname = up.Surname;
                var username = up.EmployeeId;
                return up.DisplayName;
            }

        }
        
       
        // POST: api/Gifts
        public HttpResponseMessage Post([FromBody]GiftEntertainment newGift)
        {

            if (newGift != null)
            {
                var src = DateTime.Now;
                newGift.Date = new DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, 0);
                newGift.NameOfEmployee = userDir.GetCurrentSamUserName(); ;
            }

            if ((newGift.NameOfEmployee != ""
                && newGift.DateGiftWasReceived != null
                && newGift.GiftReceivedFrom != ""
                && newGift.DescriptionOfGiftReceived != ""
                && newGift.ActualCost != 0)
                ||
                (newGift.NameOfEmployee != ""
                && newGift.DateGiftWasGiven != null
                && newGift.GiftGivenTo != ""
                && newGift.DescriptionOfGiftGiven != ""
                && newGift.ActualCost != 0)
                )
            {
                if (_repo.AddGift(newGift) && _repo.Save())
                {
                    if (String.IsNullOrEmpty(newGift.GiftReceivedFrom))
                    {

                        EmailModule.ProcessEmail(newGift, userDir, isGiven: true);
                    }
                    else
                    {
                        EmailModule.ProcessEmail(newGift, userDir);
                    }

                    return Request.CreateResponse(HttpStatusCode.Created, newGift);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        

        // PUT: api/Gifts/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Gifts/5
        public void Delete(int id)
        {
        }
    }
}
