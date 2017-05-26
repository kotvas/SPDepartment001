using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SPDepartment001.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPDepartment001.Components
{
    public class UserInfoViewComponent : ViewComponent
    {

        public UserInfoViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            string userName = HttpContext.User.Identity.Name;
            return View("Default", userName);
        }
    }
}
