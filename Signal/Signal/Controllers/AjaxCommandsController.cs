using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Signal.Controllers
{
    public class AjaxCommandsController : Controller
    {
        private readonly Data.ApplicationDbContext _db;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly Core.UserManager _dbUserManager;

        public AjaxCommandsController(Data.ApplicationDbContext db, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _dbUserManager = new Core.UserManager(_db);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddContact(string email)
        {
            try
            {
                var id = _userManager.GetUserAsync(HttpContext.User).Result.Id;
                var user = _dbUserManager.GetUserDetails(id);
                var toUser = _dbUserManager.AddContact(user, email);
                return new JsonResult(new ViewModels.ApplicationUserViewModel { FirstName = toUser.FirstName, LastName = toUser.LastName, Email = email });
            }
            catch {
                return StatusCode(500);
            }
        }
    }
}