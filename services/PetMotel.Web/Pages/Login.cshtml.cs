using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PetMotel.Web.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {

        }

        public void OnPost()
        {
            string user = Request.Form["username"];
            string pass = Request.Form["password"];
            bool rememberMe = Boolean.Parse(Request.Form["rememberme"]);
        }
    }
}