using AdminFrontend.Auth;
using AdminFrontend.ViewModels;
using Distributed;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminFrontend.Controllers
{
    [RCPAuthorize(Roles = "Admin")]
    public class TeachersController : RCPController
    {
        private ApiQueryProvider<ICollection<SystemUser>, SystemUser> _teachersQueryProvider;

        public TeachersController()
        {
            var backendUrl = Properties.Resources.SystemUsersBackendURL;
            _teachersQueryProvider = new ApiQueryProvider<ICollection<SystemUser>, SystemUser>(backendUrl, "SystemUsers");
        }

        public ActionResult Index()
        {
            var teachers = _teachersQueryProvider.Get()
                .Where(e => e.Role == "Teacher")
                .ToList();

            var model = new TeachersViewModel
            {
                Teachers = teachers
            };
            return View(model);
        }
    }
}