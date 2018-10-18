using System;
using System.Collections.Generic;
using System.Text;

namespace Signal.DataLayer.Library
{
    public class UserManager : Core.Communicator
    {
        public static void AddUserDetails()
        {
            _db.ApplicationUser.Add(new Models.ApplicationUser { FROM_EMAIL = from, MSG1 = msg, TO_EMAIL = to, MSG_IV = iv, MSG_KEY = key });
            _db.SaveChanges();
        }
    }
}
