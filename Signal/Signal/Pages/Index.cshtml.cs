using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Signal.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
                Response.Redirect("/Identity/Account/Login");

        }
    }
}
