using GiftRegister.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;

namespace GiftRegister.Logic
{
    public class ActiveUserDirectory
    {

        public List<DomainUser> GetAllUsers(string domain, string filter)
        {
            var users = new List<DomainUser>();
            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                UserPrincipal userFirstName = new UserPrincipal(context);
                userFirstName.Name = filter + "*";
                using (var searcher = new PrincipalSearcher(userFirstName))
                {

                    foreach (var result in searcher.FindAll().Take(10))
                    {
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                        try
                        {
                            var user = new DomainUser
                            {
                                firstName = de.Properties["givenName"].Value.ToString(),
                                lastName = de.Properties["sn"].Value.ToString(),
                                samAccountName = de.Properties["samAccountName"].Value.ToString(),
                                userPrincipalName = de.Properties["userPrincipalName"].Value.ToString()
                            };
                            users.Add(user);
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }
            }

            return users;
        }

        public string GetCurrentDisplayName()
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
                    return up.DisplayName;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public string GetCurrentUserName()
        {
            Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            WindowsPrincipal principal = (WindowsPrincipal)Thread.CurrentPrincipal;

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(pc, principal.Identity.Name);
                return up.GivenName;
            }

        }
        public string GetCurrentEmpCode()
        {
            Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            WindowsPrincipal principal = (WindowsPrincipal)Thread.CurrentPrincipal;

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(pc, principal.Identity.Name);
                return up.EmployeeId;
            }

        }

        public string GetCurrentSamUserName()
        {
            Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
            WindowsPrincipal principal = (WindowsPrincipal)Thread.CurrentPrincipal;

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
            {
                UserPrincipal up = UserPrincipal.FindByIdentity(pc, principal.Identity.Name);
                return up.SamAccountName;
            }

        }
    }
}