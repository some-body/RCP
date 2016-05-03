using AdminFrontend.Auth;
using AdminFrontend.ViewModels;
using Distributed;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    [RCPAuthorize(Roles = "Admin")]
    public class SystemPasswordController : RCPController
    {
        private ApiQueryProvider<ICollection<SystemUser>, SystemUser> _usersQueryProvider;

        public SystemPasswordController()
        {
            var backendUrl = Properties.Resources.SystemUsersBackendURL;
            _usersQueryProvider = new ApiQueryProvider<ICollection<SystemUser>, SystemUser>(backendUrl, "SystemUsers");
        }

        public ActionResult Index()
        {
            return View(new ChangePasswordViewModel());
        }

        [HttpPost]
        public JsonResult Save(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid || model.NewPassword != model.NewPasswordConfirm)
            {
                return Json(new
                {
                    success = false,
                    msg = "Проверьте правильность введенных данных"
                });
            }

            int adminId = 1;
            try
            {
                var admin = new SystemUser
                {
                    Id = adminId,
                    Login = null,
                    Role = null,
                    PasswordHash = model.NewPassword
                };

                var result = _usersQueryProvider.Post(admin);
                return Json(new
                {
                    success = result.Success,
                    msg = result.Success,
                    returnTo = Url.Action("Index", "Home")
                });
            }
            catch(Exception ex)
            {
                return Json(new
                {
                    success = false,
                    msg = ex.GetBaseException().Message
                });
            }
        }
    }
}