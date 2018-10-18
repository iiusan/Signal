using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Signal.Hubs
{
    public class MessengerHub : Hub
    {

        private readonly Data.ApplicationDbContext _db;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly Core.MessagesManager _dbMessagesManager;
        private readonly Core.UserManager _dbUserManager;

        public MessengerHub(Data.ApplicationDbContext db, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _dbMessagesManager = new Core.MessagesManager(_db);
            _dbUserManager = new Core.UserManager(_db);
        }

        public override Task OnConnectedAsync()
        {
            _dbUserManager.AttachSession(
                  new Models.ApplicationUser
                  {
                      Id = _signInManager.UserManager.Users.Where(u => u.UserName == Context.User.Identity.Name).Take(1).ToArray()[0].Id
                  },
                  Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string user, string to, string message)
        {
            var msg = _dbMessagesManager.AddMessage(
               new Models.ApplicationUser { Id = user },
               new Models.ApplicationUser { Id = to },
               message
               );
            string sessionId = _dbUserManager.GetLatestSignalSessionIdForUser(new Models.ApplicationUser { Id = to });
            await Clients.Client(sessionId).SendAsync("ReceiveMessage", user, message, msg.Id, _dbUserManager.IsUserAdmin(to));
            sessionId = _dbUserManager.GetLatestSignalSessionIdForUser(new Models.ApplicationUser { Id = user });// show the meesage on my screen
            await Clients.Client(sessionId).SendAsync("ReceiveMessage", user, message, msg.Id, _dbUserManager.IsUserAdmin(user));
        }

        public async Task UpdateMessageServer(string id, string message)
        {
            var msg = _dbMessagesManager.UpdateMessage(new Models.Messages { Id = id, Message = message });

            var sessionId = _dbUserManager.GetLatestSignalSessionIdForUser(new Models.ApplicationUser { Id = msg.ToId });
            await Clients.Client(sessionId).SendAsync("UpdateMessageClient", message, msg.Id);
            sessionId = _dbUserManager.GetLatestSignalSessionIdForUser(new Models.ApplicationUser { Id = msg.FromId });
            await Clients.Client(sessionId).SendAsync("UpdateMessageClient", message, msg.Id);
        }

        public async Task GetAllMessages(string user, string toUser)
        {
            var usr = new Models.ApplicationUser
            {
                Id = _signInManager.UserManager.Users.Where(u => u.UserName == Context.User.Identity.Name).Take(1).ToArray()[0].Id
            };
            string sessionId = _dbUserManager.GetLatestSignalSessionIdForUser(usr);
            var msgs = _dbMessagesManager.GetMessagesFor(usr, new Models.ApplicationUser { Id = toUser });
            var admin = _dbUserManager.IsUserAdmin(user);
            foreach (var msg in msgs)
            {
                await Clients.Client(sessionId).SendAsync("ReceiveMessage", msg.FromId, msg.Message, msg.Id, admin);
            }
        }
    }
}
