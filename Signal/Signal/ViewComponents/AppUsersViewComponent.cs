using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Signal.ViewComponents
{
    [Authorize]
    public class AppUsers : ViewComponent
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private Data.ApplicationDbContext _db;
        private readonly Core.UserManager _dbUserManager;

        public AppUsers(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, Data.ApplicationDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            _dbUserManager = new Core.UserManager(_db);
        }

        public IViewComponentResult Invoke()
        {
            float count = _dbUserManager.GetUserCount();

            int pag = Convert.ToInt32(count / 5);
            float pag1 = count / 5;
            if (pag != pag1)
                pag++;

            var results = new ViewModels.TotalUsersViewModel
            {
                UserList = _dbUserManager.GetDashUsers(5, 0),
                UserCount = count,
                Pagination = pag
            };
            return View(results);
        }
    }
}
