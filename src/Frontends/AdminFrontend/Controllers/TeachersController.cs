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

            var tableData = new TableViewModel
            {
                ColumnsNames = new string[] { "Имя", "Пароль" },
                Rows = teachers.Select(t => new Row
                {
                    Id = t.Id.Value,
                    Columns = new string[] { t.Login, t.PasswordHash }
                }).ToList(),
                EditAction = "/Teachers/Edit/", // new ActionLink("Edit", "Teachers"),
                DeleteAction = "Teachers/Delete/" //new ActionLink("Delete", "Teachers")
            };

            var model = new TeachersViewModel
            {
                TableData = tableData
            };
            return View(model);
        }
    }
}