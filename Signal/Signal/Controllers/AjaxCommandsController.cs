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

        [HttpPost]
        [Authorize]
        public IActionResult Search(string search)
        {
            try
            {
                var searchResults = _dbUserManager.SearchUsers(search);
                double count = searchResults.Count;

                int pag = Convert.ToInt32(Math.Floor(count / 5));
                double pag1 = count / 5;
                if (pag != pag1)
                    pag++;

                var results = new ViewModels.TotalUsersViewModel
                {
                    UserList = searchResults,
                    UserCount = count,
                    Pagination = pag
                };
               
                return new JsonResult(results);
            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpPost]
        [Authorize]
        public IActionResult AddAdmin(string email)
        {
            try
            {
                _dbUserManager.AddAdmin(email);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult RemoveAdmin(string email)
        {
            try
            {
                _dbUserManager.RemoveAdmin(email);
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }


    }
}