using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Signal.Models;
using System.Security.Claims;

namespace Signal.Core
{
    public class MessagesManager
    {
        private readonly Data.ApplicationDbContext _db;


        public MessagesManager(Data.ApplicationDbContext db)
        {
            _db = db;
        }

        public Messages AddMessage(ApplicationUser fromUser, ApplicationUser toUser, string message)
        {
            var msg = new Messages
            {
                FromId = fromUser.Id,
                ToId = toUser.Id,
                Message = message,
                TimeStamp = DateTime.Now
            };
            _db.Messages.Add(msg);
            _db.SaveChanges();
            return msg;
        }

        public Messages UpdateMessage(Messages newMessage)
        {
            _db.Messages.SingleOrDefault(m => m.Id == newMessage.Id).Message = newMessage.Message;
            _db.SaveChanges();
            return _db.Messages.SingleOrDefault(m => m.Id == newMessage.Id);
        }

        public List<ViewModels.MessageUserViewModel> GetMessagesFor(ApplicationUser fromUser, ApplicationUser toUser)
        {
            List<ViewModels.MessageUserViewModel> msgs = new List<ViewModels.MessageUserViewModel>();
            var raw = _db.Messages.Where(u => u.FromId == fromUser.Id && u.ToId == toUser.Id).ToList();
            raw.AddRange(_db.Messages.Where(u => u.FromId == toUser.Id && u.ToId == fromUser.Id));
            raw = raw.OrderBy(m => m.TimeStamp).ToList();
            foreach(var m in raw)
            {
                msgs.Add(new ViewModels.MessageUserViewModel
                {
                    Id = m.Id,
                    FromId = m.FromId,
                    Message = m.Message,
                    TimeStamp = m.TimeStamp
                });
            }
            return msgs;
        }
    }
}
