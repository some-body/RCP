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
                ColumnsNames = new string[] { "Логин" },
                Rows = teachers.Select(t => new Row
                {
                    Id = t.Id.Value,
                    Columns = new string[] { t.Login }
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var teacher = _teachersQueryProvider.Get(id);
            if (teacher.Role != "Teacher")
                return new HttpStatusCodeResult(401);

            return View(new TeacherEditViewModel
            {
                Id = teacher.Id.Value,
                Login = teacher.Login,
                Password = "***"
            });
        }

        [HttpPost]
        public ActionResult Save(TeacherEditViewModel model)
        {
            var teacher = new SystemUser
            {
                Id = model.Id,
                Login = model.Login,
                PasswordHash = model.Password,
                Role = "Teacher"
            };

            try
            {
                _teachersQueryProvider.Post<
            }
            catch(Exception ex)
            {
                return new HttpStatusCodeResult(501);
            }


        }
    }
}