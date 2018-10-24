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
    public class AdminViewComponent :ViewComponent
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private Data.ApplicationDbContext _db;

        public AdminViewComponent(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, Data.ApplicationDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var id = _userManager.GetUserAsync(HttpContext.User).Result.Id;
            var results = new Core.UserManager(_db).GetUserDetails(id);

            return View(new ViewModels.ApplicationUserViewModel { Id = results.Id, FirstName = results.FirstName });
        }
    }
}
