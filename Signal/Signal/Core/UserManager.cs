using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Signal.Models;
using System.Security.Claims;

namespace Signal.Core
{
    public class UserManager
    {
        private readonly Data.ApplicationDbContext _db;


        public UserManager(Data.ApplicationDbContext db)
        {
            _db = db;
        }

        public bool AddUserDetails(ApplicationUser user)
        {
            try
            {
                _db.ApplicationUser.Add(user);
                _db.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public ApplicationUser GetUserDetails(string id)
        {
            return _db.ApplicationUser.Where(u => u.Id == id).Take(1).ToArray()[0];
        }

        public ApplicationUser AddContact(ApplicationUser user, string toEmail)
        {
            string toId = _db.Users.Where(u => u.Email == toEmail).Take(1).ToArray()[0].Id;
            var exists = _db.Contact.Where(d => d.FromId == user.Id && d.ToId == toId).ToArray();
            if (exists.Count() == 0)
            {
                _db.Contact.Add(new Contact { FromId = user.Id, ToId = toId });
                _db.SaveChanges();
                return GetUserDetails(toId);
            }
            return null;
        }

        public List<ViewModels.ApplicationUserViewModel> GetUserContacts(string id)
        {
            var contactList = _db.Contact.Where(u => u.FromId == id).ToList();
            List<ViewModels.ApplicationUserViewModel> contacts = new List<ViewModels.ApplicationUserViewModel>();
            foreach(var contact in contactList)
            {
                var usr = _db.ApplicationUser.Where(m => m.Id == contact.ToId).ToList()[0];
                contacts.Add(new ViewModels.ApplicationUserViewModel {Id = usr.Id, FirstName = usr.FirstName, LastName = usr.LastName,
                    Email = _db.Users.Where(m => m.Id == usr.Id).ToList()[0].Email
                });
            }
            return contacts;
        }

        public bool IsUserAdmin(string userId)
        {
            return _db.ApplicationUser.Where(u => u.Id == userId).ToArray()[0].IsAdmin;
        }

        #region sessions

        public bool AttachSession(ApplicationUser user, string signalSessionId)
        {
            try
            {
                _db.SignalSessions.Add(new SignalSessions
                {
                    UserId = user.Id,
                    SessionId = signalSessionId,
                    TimeStamp = DateTime.Now
                });
                _db.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }


        public string GetLatestSignalSessionIdForUser(ApplicationUser user)
        {
            return _db.SignalSessions.Where(s => s.UserId == user.Id).OrderByDescending(s => s.TimeStamp).Take(1).ToArray()[0].SessionId;
        }
        #endregion

    }
}
